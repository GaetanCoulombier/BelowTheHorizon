using Godot;

public partial class Wall : Block
{
    public Wall(Vector3I position) : base(position, EnumObjects.WALL) { }
}