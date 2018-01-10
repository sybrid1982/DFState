using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public int length = 20;
    public int width = 20;
    public int depth = 20;

    public Texture2D baseTexture;

    Dictionary<Point, Block> map;

    public float blockChance = 0.4f;
    public float dirtChance = 0.6f;

    #region Public
    public void CreateMap()
    {
        map = new Dictionary<Point, Block>();
        for (int i = 0; i < depth; i++)
            CreateLayer(i);
    }

    public Block[,] GetMapLayer(int z)
    {
        return GetLayer(z);
    }
    #endregion

    #region Map Creation
    private void Awake()
    {
        CreateMap();
        UpdateController uc = FindObjectOfType<UpdateController>();
        uc.ChangeState<RunState>();
    }

    private void CreateLayer(int z)
    {
        for (int i = 0; i < width; i++)
            CreateRow(i, z);
    }

    private void CreateRow(int y, int z)
    {
        for (int i = 0; i < length; i++)
        {
            CreateBlock(i, y, z);
        }
    }

    private void CreateBlock(int x, int y, int z)
    {
        Point newPoint = new Point(x, y, z);
        BlockType newType = GetNewType(newPoint);
        // Debug.Log("Creating a block at point " + newPoint.ToString());

        Block newBlock;
        if (newType == BlockType.Empty)
            newBlock = new Block(newPoint);
        else if (newType == BlockType.Slope || newType == BlockType.Solid)
        {
            // We are going to need to assign this a content now
            BlockContents newContents = GetNewContents();
            newBlock = new Block(newType, newContents, newPoint);
        } else
        {
            // This is a water block, so contents and type are water
            newBlock = new Block(newType, BlockContents.Water, newPoint);
        }
        
        if (map.ContainsKey(newPoint))
            Debug.Log("Map contains point " + newPoint.ToString() + " already.");
        else
            map.Add(newPoint, newBlock);

        SetNeighbors(x, y, newBlock);
    }

    // Right now this just randomly makes blocks either solid or not
    // This is an obvious place to tweak things to be more interesting
    private BlockType GetNewType(Point point)
    {
        if (Random.Range(0f, 1f) < blockChance)
            return BlockType.Solid;
        else
            return BlockType.Empty;
    }

    private BlockContents GetNewContents()
    {
        if (Random.Range(0f, 1f) < dirtChance)
            return BlockContents.Dirt;
        else
            return BlockContents.Stone;
    }

    private void SetNeighbors(int x, int y, Block block)
    {
        if (x > 0)
        {
            Point newNeighborPoint = new Point(block.Point.x - 1, block.Point.y, block.Point.z);
            Block newNeighbor = GetBlockAt(newNeighborPoint);
            block.SetNeighbor(SquareDirection.W, newNeighbor);
        }
        if (y > 0)
        {
            Point newNeighborPoint = new Point(block.Point.x, block.Point.y - 1, block.Point.z);
            Block newNeighbor = GetBlockAt(newNeighborPoint);
            block.SetNeighbor(SquareDirection.S, newNeighbor);
            if (x > 0)
            {
                newNeighborPoint = new Point(block.Point.x - 1, block.Point.y - 1, block.Point.z);
                newNeighbor = GetBlockAt(newNeighborPoint);
                block.SetNeighbor(SquareDirection.SW, newNeighbor);
            }
            if (x < length - 1)
            {
                newNeighborPoint = new Point(block.Point.x + 1, block.Point.y - 1, block.Point.z);
                newNeighbor = GetBlockAt(newNeighborPoint);
                block.SetNeighbor(SquareDirection.SE, newNeighbor);
            }
        }
    }


    #endregion

    #region GettingMapInfo
    /// <summary>
    /// This function currently just returns an array of blocks at layer z
    /// 
    /// What it should do is return, from layer Z, return blocks on that layer or the first floor down if that space
    /// doesn't have a block or water
    /// Along with that, it should return a height for each block so the renderer knows where to draw those blocks
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    private Block[,] GetLayer(int z)
    {
        // Future plan: return either the bottom or top layer?
        if (z < 0 || z >= depth)
            return null;
        else
        {
            Block[,] layer = new Block[length, width];
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Block targetBlock = GetBlockAt(new Point(x, y, z));
                    if(targetBlock == null)
                    {
                        Debug.LogError("Tried to find a nonexistant block in GetLayer.");
                        return null;
                    }

                    if (targetBlock.IsEmpty == true) {
                        // Try the next block down
                        int currentZ = z;
                        while (currentZ > 0 && targetBlock.IsEmpty == true)
                        {
                            currentZ--;
                            targetBlock = GetBlockAt(new Point(x, y, currentZ));
                        }
                        if (targetBlock.IsEmpty)
                        {
                            // Then we went all the way down and found nothing
                            // ...what do we do then?
                            // Possibility: return solid block at z level -1 for rendering?
                        }
                    }
                    layer[x, y] = targetBlock;
                }
            }
            return layer;
        }
    }

    private Block GetBlockAt(Point point)
    {
        if (map.ContainsKey(point))
        {
            return map[point];
        }
        else
        {
            Debug.LogWarning("Tried to find non-extant block at " + point.ToString());
            return null;
        }
    }
    #endregion
}