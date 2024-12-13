using Godot;

public class PlayerClimbingHandler
{

    private Player _player;
    private RayCast3D _raycastLedge;
    private RayCast3D _raycastFacingWall;
    private Node3D _raycastHolder;
    private Node3D _ledgeMarker;

    public PlayerClimbingHandler(Player player)
    {
        _player = player;

        _raycastFacingWall = player.GetNode<RayCast3D>("/root/GameRoot/Player/RayCastFacingWall");
        _raycastLedge = player.GetNode<RayCast3D>("/root/GameRoot/Player/RaycastLedgeChecker/RayCastHead");
        _raycastHolder = player.GetNode<Node3D>("/root/GameRoot/Player/RaycastLedgeChecker");
        _ledgeMarker = player.GetNode<Node3D>("/root/GameRoot/Player/RaycastLedgeChecker/RayCastHead/LedgeMarker");

        GD.Print(_raycastFacingWall);
        GD.Print(_raycastLedge);
        GD.Print(_raycastHolder);
        GD.Print(_ledgeMarker);
    }

    public void LedgeDetection()
    {
        var hitPoint = _raycastFacingWall.GetCollisionPoint();
        var hitPoint2 = _raycastLedge.GetCollisionPoint();
        var offset = new Vector3(0, 3, 0);
        if (_raycastFacingWall.IsColliding())
        {
            Transform3D holderTransform = _raycastHolder.GlobalTransform;
            holderTransform.Origin = hitPoint + offset;
            _raycastHolder.GlobalTransform = holderTransform;

            Transform3D markerTransform = _ledgeMarker.GlobalTransform;
            markerTransform.Origin = hitPoint2;
            _ledgeMarker.GlobalTransform = markerTransform;

            _ledgeMarker.Visible = true;
            _raycastLedge.Enabled = true;
        } else
        {
            _ledgeMarker.Visible = false;
            _raycastLedge.Enabled = false;
        }
    }
}