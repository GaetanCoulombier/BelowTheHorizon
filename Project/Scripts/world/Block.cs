using Godot;

public abstract partial class Block : Node3D
{
    public int _objectType;
    private GameManager _gameManager;

    public Block(Vector3I position, int objectType)
    {
        this._objectType = objectType;
        GlobalPosition = position;
    }

    public new Vector3I GetPosition()
    {
        return (Vector3I) GlobalPosition.Round();
    }

    public int GetPositionX()
    {
        return Mathf.FloorToInt(GlobalPosition.X);
    }

    public int GetPositionY()
    {
        return Mathf.FloorToInt(GlobalPosition.Y);
    }

    public int GetPositionZ()
    {
        return Mathf.FloorToInt(GlobalPosition.Z);
    }

    public int GetObjectType()
    {
        return _objectType;
    }
}
