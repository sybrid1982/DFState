using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block {
    #region Properties/Variables/Getters/Setters
    BlockState _type;

    Character _character;

    Map map;

    public Character Character {
        get { return _character; }
        set { _character = value; }
    }

    int health;

    Point _point;

    public Point Point {
        get { return _point; }    
    }

    bool _hasFloor = true;          // If this is false, then there's nothing between this block and the block above it
                                    // This should only have the ability to be false if this block is empty or water
                                    // It is possible for both this block and the one above it to be empty and for there to still be a floor
                                    // Setting this block to solid or slope should immediately set this back to true

    public Block[] neighbors = new Block[8];

    public BlockState Type
    {
        get { return _type; }
        // In the future, changing the block type should send a notification
        // Which the display would then read to determine if it needs to update
        // Anything currently
        // Also should determine if we need to update the pathfinding graph
        set {
            if (value == _type)
                return;
            else
            {
                _type = value;
                // When we place a solid or ramp block, we'll automagically add a floor for the block to sit on
                if(_type is BS_Solid || _type is BS_Ramp)
                {
                    HasFloor = true;
                }
            }
        }
    }

    // This seems like overkill now, but when we start adding in modifiers
    // to the move cost for a block like furniture MoveCost, then we'll
    // want this to be able to account for those factors

    // For now, if there's no floor for an empty block, then we will
    // replace the default movecost of 1 with 0 because you can't walk
    // in empty spaces
    public int MoveCost {
        get
        {
            int moveCost = Type.MoveCost;
            if (!HasFloor)
                moveCost = 0;
            return moveCost;
        }
    }

    public BlockContents Contents
    {
        get { return _type.Contents; }
    }

    public bool HasFloor
    {
        get { return _hasFloor; }
        set
        {
            // If we try to remove a floor, and the block we're on is solid or a slope, do we then destroy this block as well?
            // Yes - destroying the floor of a block will also destroy the block its on, and this is how we will dig a hole down
            if(value == false && _hasFloor == true)
            {
                // For now, we'll replace the removed block with a ramp if it was solid
                if (Type is BS_Solid)
                {
                    BlockContents blockContents = Type.Contents;
                    Type = new BS_Ramp(blockContents);
                }
            }

            // In the future, when a floor is removed we should probably have things drop down to the next solid block below
            // Could have falling damage that way too
            _hasFloor = value;
        }
    }

    // In the future we will probably include 'has blocking furniture'
    // In this bool
    public bool IsEmpty
    {
        get { return (Type is BS_Empty && HasFloor == false); }
    }
    
    public bool IsWalkable {
        get { return (Type.MoveCost > 0); }
    }

    #endregion

    #region Constructors
    // Constructor for empty blocks
    public Block(Point point, Map map)
    {
        _type = new BS_Empty();
        _point = point;
        this.map = map;
    }

    // Constructor for non-empty blocks
    public Block(BlockState type, Point point, Map map)
    {
        _type = type;
        _point = point;
        this.map = map;
    }
    #endregion

    #region Public
    #region Neighbors
    public void SetNeighbor(SquareDirection direction, Block block)
    {
        neighbors[(int)direction] = block;
        block.neighbors[(int)direction.Opposite()] = this;
    }

    public Block GetNeighbor(SquareDirection direction)
    {
        return neighbors[(int)direction];
    }

    public bool IsNeighbor(Block block)
    {
        for(int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] == block)
                return true;
        }
        return false;
    }
    #endregion

    public bool DamageBlock(float damage)
    {
        Type.Contents.ReduceHealth(damage);
        if (Type.Contents.Health <= 0)
        {
            Type = new BS_Empty();

            return false;
        }
        return true;
    }

    #endregion


}
