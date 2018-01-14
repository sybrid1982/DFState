using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public int length = 20;
    public int width = 20;
    public int depth = 20;

    public Character characterPrefab;

    public float sigmoidalInflectionPoint = 10;

    Dictionary<Point, Block> map;

    private SquareGrid squareGrid;

    // Less than 1 and you get spotty buildup in areas that should be solid
    public float blockChance = 1f;

    public float dirtChance = 0.6f;

    public float growthRate = 0.8f;

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
        squareGrid = FindObjectOfType<SquareGrid>();
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
        BlockState newType = GetNewType(newPoint);
        // Debug.Log("Creating a block at point " + newPoint.ToString());

        Block newBlock = new Block(newType, newPoint);
        
        
        if (map.ContainsKey(newPoint))
            Debug.Log("Map contains point " + newPoint.ToString() + " already.");
        else
            map.Add(newPoint, newBlock);

        SetNeighbors(x, y, newBlock);
    }

    // Right now this just randomly makes blocks either solid or not
    // This is an obvious place to tweak things to be more interesting
    /*private BlockType GetNewType(Point point)
    {

    }*/

    /* The above version of this function is fine if you want random
     * empty spaces and filled blocks, but now let's change things up
     * This version of the function will instead look at how far to the
     * right the new block is supposed to be, and the further right it is
     * the more likely it is to put a block there instead of a point
     * 
     * This should lead to maps where the left side is largely empty
     * and the right side is largely full
     * FURTHERMORE, we will only be allowed to put a new solid block in
     * if the block one down is also solid (or is off the map) */

    private BlockState GetNewType(Point point)
    {
        BlockState returnType = new BS_Empty();

        if (sigmoidalInflectionPoint <= 0)
            sigmoidalInflectionPoint = 1;

        float sigmoidPoint = Sigmoid(point.x, sigmoidalInflectionPoint);

        float chanceToProduceSolidBlock = sigmoidPoint * blockChance;

        if (Random.Range(0f, 1f) < chanceToProduceSolidBlock)
            returnType = new BS_Solid(GetNewContents());

        return returnType;
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

    public void SpawnStartingCharacter()
    {
        Character newCharacter = Instantiate(characterPrefab);
        newCharacter.SetupCharacter(map[new Point(0,0,0)]);
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

    #region Math
    private float Sigmoid(int x, float inflectionPoint)
    {
        // We'll use a Gompertz Curve for this, just because
        // a is the maximum value we want out of this function (in this case, 1)
        int a = 1;
        // b is how much displacement occurs of the graph along the x axis
        float b = inflectionPoint * length;
        // c is the growth rate
        // Smaller = more gradual increase over time
        // Larger = sharper, more sudden change
        float c = growthRate;

        // a * e ^ (-b * e ^ (-c * x))
        float returnValue = a * Mathf.Exp(-b * Mathf.Exp(-c*x));
        // Debug.Log("For int " + x + " gompertz is " + returnValue);

        return returnValue;
    }
    #endregion
}