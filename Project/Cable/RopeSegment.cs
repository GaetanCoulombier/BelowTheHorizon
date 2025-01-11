using Godot;

public class RopeSegment
{
    public Vector3 CurrentPosition;
    public Vector3 PreviousPosition;

    public RopeSegment(Vector3 position)
    {
        CurrentPosition = position;
        PreviousPosition = position;
    }
}
