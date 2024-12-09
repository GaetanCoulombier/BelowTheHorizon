using System;
using Godot;

public partial class World : Node
{
    // TODO: Créer un fichier qui contient la grille 3D en json ou autre format le moins couteux en mémoire.
    private Block[] _blocks;

    public int Width, Height, Depth;

    public World(int width, int height, int depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
        _blocks = new Block[Width * Height * Depth];
    }

    public void EmptyWorld()
    {
        var nbBlocks = Width * Height * Depth;
        for (int i = 0; i < nbBlocks; i++)
        {
            var position = GetPositionFromIndex(i);
            _blocks[i] = new Void(position);
        }
    }

    public override void _Ready()
    {
        EmptyWorld();
    }

    /**
     * Single block methods
     */
    public bool IsBlockInWorld(int x, int y, int z)
    {
        var index = GetIndex(x, y, z);

        if (index >= 0 && index < _blocks.Length)
            return true;
        
        return false;
    }

    public bool IsBlockEmptyAt(int x, int y, int z)
    {
        var index = GetIndex(x, y, z);
        return _blocks[index] is Void;
    }

    public bool SetBlock(Block block)
    {
        var x = block.GetPositionX();
        var y = block.GetPositionY();
        var z = block.GetPositionZ();

        if (!IsBlockInWorld(x, y, z))
            return false; // No Set : out of world
        _blocks[GetIndex(x, y, z)] = block;
        return true;
    }

    public Block GetBlock(int x, int y, int z)
    {
        if (!IsBlockInWorld(x, y, z))
            return null; // No get : out of world
        return _blocks[GetIndex(x, y, z)];
    }

    public bool RemoveBlock(int x, int y, int z)
    {
        if (!IsBlockInWorld(x, y, z))
            return false; // No remove : out of world
        _blocks[GetIndex(x, y, z)] = new Void(new Vector3I(x, y, z));
        return true;
    }

    public bool MoveBlock(int x, int y, int z, int newX, int newY, int newZ)
    {
        if (!IsBlockInWorld(x, y, z) || !IsBlockInWorld(newX, newY, newZ))
            return false; // No move : out of world

        var block = GetBlock(x, y, z);
        if (block == null)
            return false; // No move : no block to move

        if (!IsBlockEmptyAt(newX, newY, newZ))
            return false; // No move : destination is not empty

        RemoveBlock(x, y, z);
        block.SetPosition(new Vector3I(newX, newY, newZ));
        SetBlock(block);
        return true;
    }


    /**
     * Private methods
     */
    private int GetIndex(int x, int y, int z)
    {
        return x + Width * (y + Depth * z);
    }


    private Vector3I GetPositionFromIndex(int index)
    {
        int z = index / (Width * Depth);
        int remainder = index % (Width * Depth);
        int y = remainder / Width;
        int x = remainder % Width;

        return new Vector3I(x, y, z);
    }

}