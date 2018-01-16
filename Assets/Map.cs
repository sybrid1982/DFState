using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    // Size of the map
    public int chunkCountX = 5, chunkCountZ = 5;
    public int depth = 20;

    public int cellCountX { get { return chunkCountX * SquareMetrics.chunkSizeX; } }
    public int cellCountZ { get { return chunkCountZ * SquareMetrics.chunkSizeY; } }

    // This will eventually not belong here
    public Character characterPrefab;

    // Used in generating solid/empty block spaces
    public float sigmoidalInflectionPoint = 10;

    // The pathfinding graph used to navigate the map
    public Path_BlockGraph blockGraph;

    Dictionary<Point, Block> map;

    // SquareGrid handles graphics!
    private SquareGrid squareGrid;

    private Dictionary<string, BlockContents> namesToPrototypesMap;

    // Less than 1 and you get spotty buildup in areas that should be solid
    public float blockChance = 1f;

    public float dirtChance = 0.6f;

    public float growthRate = 0.8f;

    #region Public
    public void CreateMap()
    {
        map = new Dictionary<Point, Block>();
        namesToPrototypesMap = new Dictionary<string, BlockContents>();
        CreatePrototypes();

        for (int i = 0; i < depth; i++)
            CreateLayer(i);

        NotificationExtensions.AddObserver(this, InvalidateBlockGraph, BlockMetrics.BlockChangeAnnouncment);
    }

    public Block[,] GetMapLayer(int z)
    {
        return GetLayer(z);
    }

    // We don't care who sent this or what they had to say, we just need to wipe out the blockGraph
    public void InvalidateBlockGraph(object sender, object info)
    {
        blockGraph = null;
    }

    public Path_BlockGraph GetBlockGraph()
    {
        if(blockGraph == null)
            blockGraph = new Path_BlockGraph(this);
        return blockGraph;
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
        for (int i = 0; i < cellCountZ; i++)
            CreateRow(i, z);
    }

    private void CreateRow(int y, int z)
    {
        for (int i = 0; i < cellCountX; i++)
        {
            CreateBlock(i, y, z);
        }
    }

    private void CreateBlock(int x, int y, int z)
    {
        Point newPoint = new Point(x, y, z);
        BlockState newType = GetNewType(newPoint);
        // Debug.Log("Creating a block at point " + newPoint.ToString());

        Block newBlock = new Block(newType, newPoint, this);
        
        
        if (map.ContainsKey(newPoint))
            Debug.Log("Map contains point " + newPoint.ToString() + " already.");
        else
            map.Add(newPoint, newBlock);

        SetNeighbors(x, y, newBlock);
    }

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
            return new BlockContents(namesToPrototypesMap["Dirt"]);
        else
            return new BlockContents(namesToPrototypesMap["Stone"]);
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
            if (x < cellCountX - 1)
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
        newCharacter.SetupCharacter(map[new Point(0,0,0)], this);
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
            Block[,] layer = new Block[cellCountX, cellCountZ];
            for (int x = 0; x < cellCountX; x++)
            {
                for (int y = 0; y < cellCountZ; y++)
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

    public List<Block> GetAllBlocks()
    {
        List<Block> blocks = new List<Block>();
        foreach (Block block in map.Values)
        {
            blocks.Add(block);
        }
        return blocks;
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

    #region Prototyping
    void CreatePrototypes()
    {
        BlockContents dirt = new BlockContents("Dirt", 5, new Color(0.4f, 0.4f, 0f, 1f));
        BlockContents stone = new BlockContents("Stone", 10, Color.gray);
        BlockContents water = new BlockContents("Water", int.MaxValue, Color.blue);
        namesToPrototypesMap.Add("Dirt", dirt);
        namesToPrototypesMap.Add("Stone", stone);
        namesToPrototypesMap.Add("Water", water);
    }
    #endregion

    #region Math
    private float Sigmoid(int x, float inflectionPoint)
    {
        // We'll use a Gompertz Curve for this, just because
        // a is the maximum value we want out of this function (in this case, 1)
        int a = 1;
        // b is how much displacement occurs of the graph along the x axis
        float b = inflectionPoint * cellCountX;
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