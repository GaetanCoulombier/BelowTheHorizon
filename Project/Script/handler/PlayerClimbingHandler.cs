using Godot;

public class PlayerClimbingHandler
{
    private Player _player;
    private RayCast3D _raycastRightUp;
    private RayCast3D _raycastRightDown;
    private RayCast3D _raycastLeftUp;
    private RayCast3D _raycastLeftDown;
    private RayCast3D _raycastMiddleUp;
    private RayCast3D _raycastMiddleDown;
    private RayCast3D _raycastFacingWall;
    public bool IsHanging { get; private set; }

    private Vector3 _hitNormal;

    public PlayerClimbingHandler(
        Player player,
        RayCast3D facingWall,
        RayCast3D raycastRightUp,
        RayCast3D raycastRightDown,
        RayCast3D raycastLeftUp,
        RayCast3D raycastLeftDown,
        RayCast3D raycastMiddleUp,
        RayCast3D raycastMiddleDown,
        RayCast3D raycastSideLeft,
        RayCast3D raycastSideRight
    ) {
        _player = player;
        _raycastFacingWall = facingWall;

        _raycastRightDown = raycastRightDown;
        _raycastLeftDown = raycastLeftDown;
        _raycastMiddleUp = raycastMiddleUp;
        _raycastMiddleDown = raycastMiddleDown;

        _raycastRightUp = raycastRightUp;
        _raycastLeftUp = raycastLeftUp;


        IsHanging = false;
    }


    /* Public methods */
    public void AttachToLedge()
    {
        // Gérer l'état du joueur
        if (CanAttach() && HasSpaceForHands())
        {
            if (!IsHanging && _raycastFacingWall.IsColliding())
            {
                _hitNormal = _raycastFacingWall.GetCollisionNormal();

                // Repositionner le joueur face au mur
                AlignPlayerToWall(_hitNormal, _raycastFacingWall.GetCollisionPoint());
            }
            IsHanging = true;
        }
    }

    public void MoveAlongLedge(float wantedDirection)
    {
        if (!IsHanging) return;

        // Vérifier les collisions et l'état des côtés
        bool canMoveLeft = HasSpaceForHands((int)ClimbingDirection.Left);
        bool canMoveRight = HasSpaceForHands((int)ClimbingDirection.Right);
        bool canMove = (wantedDirection < 0 && canMoveLeft) || (wantedDirection > 0 && canMoveRight);
        bool isGoodSide = (wantedDirection < 0 && canMoveLeft) || (wantedDirection > 0 && canMoveRight);

        // Obtenir la normale du mur
        Vector3 wallNormal = _raycastMiddleDown.IsColliding() ? _raycastMiddleDown.GetCollisionNormal() : Vector3.Zero;
        if (wallNormal == Vector3.Zero) return;

        // Déterminer la direction de mouvement
        bool isWallX = Mathf.Abs(wallNormal.X) > 0;
        Vector3 moveDirection = isWallX 
            ? new Vector3(0, 0, wantedDirection * Mathf.Sign(-wallNormal.X)) 
            : new Vector3(wantedDirection * Mathf.Sign(wallNormal.Z), 0, 0);

        // Appliquer la translation si possible
        if (canMove || isGoodSide)
        {
            _player.GlobalTransform = new Transform3D(
                _player.GlobalTransform.Basis, 
                _player.GlobalTransform.Origin + moveDirection * 0.1f
            );
        }
    }

    public void DropDown()
    {
        GD.Print("Dropping down");

        if (!IsHanging) return;

        IsHanging = false;
    }

    public void ClimbUp()
    {
        GD.Print("Climbing up");

        if (!IsHanging) return;

        // TODO: Implement climbing up
    }


    /* Private methods */
    private bool CanAttach()
    {
        bool canAttach = false;

        Vector3 wallNormal = _raycastMiddleDown.GetCollisionNormal();
        float angle = Mathf.RadToDeg(wallNormal.AngleTo(Vector3.Up));
        if (angle > 80 && angle < 100) // Tolérance pour la perpendicularité
        {
            canAttach = true;
        }

        return canAttach;
    }

    // You can attatch to a ledge if it is at least 0.1f space for it you need eather up ray not coliding or the down ray closer to the player than the up ray
    private bool HasSpaceForHands(int direction = 0)
    {
        RayCast3D raycastUp, raycastDown;

        if (direction == (int)ClimbingDirection.Left)
        {
            raycastUp = _raycastLeftUp;
            raycastDown = _raycastLeftDown;
        }
        else if (direction == (int)ClimbingDirection.Right)
        {
            raycastUp = _raycastRightUp;
            raycastDown = _raycastRightDown;
        }
        else
        {
            raycastUp = _raycastMiddleUp;
            raycastDown = _raycastMiddleDown;
        }

        if (!raycastDown.IsColliding()) return false;
        if (!raycastUp.IsColliding()) return true;

        float diff = raycastUp.GetCollisionPoint().DistanceTo(_player.GlobalTransform.Origin) -
                    raycastDown.GetCollisionPoint().DistanceTo(_player.GlobalTransform.Origin);

        return diff > 0.1f;
    }


    private void AlignPlayerToWall(Vector3 wallNormal, Vector3 wallPosition)
    {
        float distanceFromWall = -0.5f;  // La distance par défaut peut être de 0.5f, mais vous pouvez la modifier

        // Z : Direction face au mur (identique à la normale)
        Vector3 forward = wallNormal.Normalized();

        // Y : Direction verticale (vers le haut)
        Vector3 up = Vector3.Up;

        // X : Produit vectoriel pour trouver la direction droite
        Vector3 right = up.Cross(forward).Normalized();

        // Recalibrer l'axe Y pour s'assurer qu'il est perpendiculaire à Z et X
        up = forward.Cross(right).Normalized();

        // Construire une nouvelle base
        Basis playerBasis = new Basis(right, up, forward);

        // Appliquer uniquement la base pour aligner sur X et Z, tout en préservant Y
        Transform3D playerTransform = _player.GlobalTransform;
        playerTransform.Basis = playerBasis;

        // Préserver la position Y du joueur
        Vector3 currentPosition = _player.GlobalTransform.Origin;
        Vector3 newPosition = new Vector3(
            wallPosition.X - wallNormal.X * distanceFromWall,
            currentPosition.Y, // Conserver Y tel quel
            wallPosition.Z - wallNormal.Z * distanceFromWall
        );

        // Appliquer la nouvelle base et position au joueur
        _player.GlobalTransform = new Transform3D(playerTransform.Basis, newPosition);
    }
}

public enum ClimbingDirection
{
    Left = -1,
    Right = 1
}