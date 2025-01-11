using System.Collections.Generic;
using Godot;

public partial class RopeController : Node3D
{
    private List<RopeSegment> _ropeSegments;
    private float _segmentLength = 0.5f;
    private int _segmentCount = 20;
    private Vector3 _gravity = new Vector3(0, -9.8f, 0);

    public override void _Ready()
    {
        _ropeSegments = new List<RopeSegment>();

        Vector3 startPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < _segmentCount; i++)
        {
            _ropeSegments.Add(new RopeSegment(startPosition + new Vector3(0, -i * _segmentLength, 0)));
        }
    }
}