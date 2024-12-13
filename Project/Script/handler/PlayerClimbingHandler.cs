using Godot;

public class PlayerClimbingHandler
{
    private Player _player;
    private RayCast3D _raycastFacingWall;
    private RayCast3D _raycastLedge;
    private Node3D _ledgeMarker;
    private Node3D _raycastHolder;
    private Vector3 _ledgePosition;
    public bool IsHanging { get; private set; }

    public PlayerClimbingHandler(Player player, RayCast3D facingWall, RayCast3D ledge, Node3D ledgeMarker, Node3D raycastHolder)
    {
        _player = player;
        _raycastFacingWall = facingWall;
        _raycastLedge = ledge;
        _ledgeMarker = ledgeMarker;
        _raycastHolder = raycastHolder;
        _ledgePosition = Vector3.Zero;
        IsHanging = false;
    }

    public void DetectLedge()
    {
        var hitPoint = _raycastFacingWall.GetCollisionPoint();
        _ledgePosition = _raycastLedge.GetCollisionPoint();
        var offset = new Vector3(0, 3, 0);
        if (_raycastFacingWall.IsColliding())
        {
            Transform3D holderTransform = _raycastHolder.GlobalTransform;
            holderTransform.Origin = hitPoint + offset;
            _raycastHolder.GlobalTransform = holderTransform;

            Transform3D markerTransform = _ledgeMarker.GlobalTransform;
            markerTransform.Origin = _ledgePosition;
            _ledgeMarker.GlobalTransform = markerTransform;

            _ledgeMarker.Visible = true;
            _raycastLedge.Enabled = true;
        } else
        {
            _ledgeMarker.Visible = false;
            _raycastLedge.Enabled = false;
        }
    }

    // GrabLedge is for grabbing the ledge that is in front of the player and hang on it
    public void GrabLedge()
    {
        GD.Print("Grabbing ledge");

        if (!_raycastLedge.IsColliding()) return;

        IsHanging = true;
        _player.GlobalTransform = new Transform3D(_player.GlobalTransform.Basis, _ledgePosition + new Vector3(0, -1.0f, 0));
    }

    public void DropDown()
    {
        GD.Print("Dropping down");

        if (!IsHanging) return;

        IsHanging = false;
    }

    // MoveAlongLedge is for moving along the ledge to the left or right if there is enough space
    public void MoveAlongLedge(float direction)
    {
        GD.Print("Moving along ledge");

        if (!IsHanging) return;

        // Vérify if there is enough space to move along the ledge

        // Move the player along the ledge
        _ledgePosition += new Vector3(direction, 0, 0);
        _player.GlobalTransform = new Transform3D(_player.GlobalTransform.Basis, _ledgePosition + new Vector3(0, -1.0f, 0));
    }

    // Climb up is for climbing up to the ledge and go on top of it
    public void ClimbUp()
    {
        GD.Print("Climbing up");

        if (!IsHanging) return;

        // TODO: Implement climbing up
    }
}