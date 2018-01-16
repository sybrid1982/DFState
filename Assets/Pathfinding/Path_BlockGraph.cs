using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_BlockGraph {

    // This class constructs a simple path-finding compatible graph
    // of our world.  Each tile is a node.  Each walkable neighbor
    // from a tile is linked via an edge connection

    public Dictionary<Block, Path_Node<Block>> nodes;

    public Path_BlockGraph(Map map)
    {
        nodes = new Dictionary<Block, Path_Node<Block>>();

        // Loop through every block of the world
        // For each block, create a node
        foreach(Block block in map.GetAllBlocks())
        {
            CreateNode(block);
        }
        foreach(Block block in nodes.Keys)
        {
            CreateEdgesForNode(block);
        }
    }

    private void CreateEdgesForNode(Block block)
    {
        Path_Node<Block> n = nodes[block];

        List<Path_Edge<Block>> edges = new List<Path_Edge<Block>>();
        // Get a list of neighbors
        // if the neighbor is walkable, create an edge
        // if the block is a ramp, then blocks that are
        // either at the ramp's height or one up from there
        // are walkable neighbors
        // TODO: Implement the part about ramps working

        for (int i = 0; i < 4; i++)
        {
            SquareDirection dir = SquareDirectionExtensions.ReturnOrthogonal(i);
            Block nBlock = block.GetNeighbor(dir);
            if (nBlock != null)
            {
                Path_Edge<Block> e = new Path_Edge<Block>();
                e.cost = nBlock.MoveCost;
                e.node = nodes[nBlock];
                edges.Add(e);
            }
            
        }
        n.edges = edges.ToArray();
    }

    private void CreateNode(Block block)
    {
        Path_Node<Block> n = new Path_Node<Block>();
        n.data = block;
        nodes.Add(block, n);
    }
}
