using Godot;

public abstract partial class Block : StaticBody3D
{
    public int _objectType;
    public Vector3I GridPosition { get; private set; }
    private GameManager _gameManager;

    public Block(Vector3I position, int objectType)
    {
        this._objectType = objectType;
        
        GridPosition = position;
        GlobalPosition = new Vector3(position.X, position.Y, position.Z);

        // Ajouter une BoxShape3D pour les collisions
        CollisionShape3D collisionShape = new CollisionShape3D();
        collisionShape.Shape = new BoxShape3D { Size = Vector3.One }; // Taille adaptée à un bloc 1x1x1
        AddChild(collisionShape);

        // Ajouter un visuel pour le bloc (optionnel)
        MeshInstance3D mesh = new MeshInstance3D
        {
            Mesh = new BoxMesh()
        };
        AddChild(mesh);
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
