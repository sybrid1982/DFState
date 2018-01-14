using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block { 
    BlockState _type;

    Character _character;

    public Character Character {
        get { return _character; }
        set { _character = value; }
    }

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

    // Constructor for empty blocks
    public Block(Point point)
    {
        _type = new BS_Empty();
        _point = point;
    }

    // Constructor for non-empty blocks
    public Block(BlockState type, Point point)
    {
        _type = type;
        _point = point;
    }

    public void SetNeighbor(SquareDirection direction, Block block)
    {
        neighbors[(int)direction] = block;
        block.neighbors[(int)direction.Opposite()] = this;
    }

    public Block GetNeighbor(SquareDirection direction)
    {
        return neighbors[(int)direction];
    }
}
