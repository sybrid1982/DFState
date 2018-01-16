using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class Path_AStar {

    Queue<Block> path;

    public Path_AStar(Map map, Block start, Block end)
    {
        path = new Queue<Block>();

        Path_BlockGraph blockGraph = map.GetBlockGraph();

        // Dictionary of all valid, walkable nodes
        Dictionary<Block, Path_Node<Block>> nodes = blockGraph.nodes;

        // Make sure the start/end tiles are in the list of nodes
        if(nodes.ContainsKey(start) == false)
        {
            Debug.LogError("Path_AStar: The starting tile isn't in the list of nodes");
            return;
        }
        if(nodes.ContainsKey(end) == false)
        {

            return;
        }

        List<Path_Node<Block>> closedSet = new List<Path_Node<Block>>();

        SimplePriorityQueue<Path_Node<Block>, float> openSet = new SimplePriorityQueue<Path_Node<Block>, float>();
        openSet.Enqueue(nodes[start], 0);

        Dictionary<Path_Node<Block>, Path_Node<Block>> cameFrom = new Dictionary<Path_Node<Block>, Path_Node<Block>>();

        // gScore is the cost of getting from the start node to that node
        // default score is infinite
        Dictionary<Path_Node<Block>, float> gScoreMap = new Dictionary<Path_Node<Block>, float>();
        // fScore is the cost of getting from the start node to the end
        // node by passing through a given node.  This value is partially
        // known and partially heuristic
        Dictionary<Path_Node<Block>, float> fScoreMap = new Dictionary<Path_Node<Block>, float>();

        foreach (Path_Node<Block> n in nodes.Values)
        {
            gScoreMap.Add(n, Mathf.Infinity);
            fScoreMap.Add(n, Mathf.Infinity);
        }
        gScoreMap[nodes[start]] = 0;
        fScoreMap[nodes[start]] = HeuristicCostEstimate(nodes[start], nodes[end]);

        while (openSet.Count > 0)
        {
            Path_Node<Block> current = openSet.Dequeue();
            if (current == nodes[end])
            {
                // Then return the came-from map
                ReconstructPath(cameFrom, current);
                return;
            }
            closedSet.Add(current);

            foreach(Path_Edge<Block> edge in current.edges)
            {
                if (closedSet.Contains(edge.node))
                {
                    continue;
                }
                // 0 is used for the edge cost of unwalkable tiles
                // However for some jobs (like mining or chopping trees)
                // The goal tile is unwalkable, which is fine because we want a neighbor anyways
                if (edge.cost == 0 && edge.node.data != end)
                    continue;

                float tentative_gScore = gScoreMap[current] + edge.cost;
                
                if (tentative_gScore >= gScoreMap[edge.node])
                    continue;

                if (!cameFrom.ContainsKey(edge.node))
                    cameFrom.Add(edge.node, current);
                else
                    cameFrom[edge.node] = current;

                gScoreMap[edge.node] = tentative_gScore;
                fScoreMap[edge.node] = gScoreMap[edge.node] + HeuristicCostEstimate(edge.node, nodes[end]);
                if(openSet.Contains(edge.node) == false)
                {
                    openSet.Enqueue(edge.node, fScoreMap[edge.node]);
                }
                else
                {
                    openSet.UpdatePriority(edge.node, fScoreMap[edge.node]);
                }
            }   // end of Foreach
        }   // End of While
        // If we get here, then we failed to find a path
        // So there is no valid path from start to goal

        // Our failure state will be that the list of blocks will be null
        Debug.Log("Could not find path");
    }

    public Block GetNextBlock()
    {
        return path.Dequeue();
    }

    public int Length()
    {
        if (path == null)
            return 0;
        else
            return path.Count;
    }

    private void ReconstructPath(Dictionary<Path_Node<Block>, Path_Node<Block>> cameFrom, Path_Node<Block> endNode)
    {
        Stack<Path_Node<Block>> nodeStack = new Stack<Path_Node<Block>>();
        nodeStack.Push(endNode);
        Path_Node<Block> current = endNode;

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            nodeStack.Push(current);
        }

        while(nodeStack.Count > 0)
        {
            current = nodeStack.Pop();
            path.Enqueue(current.data);
        }
    }

    private float HeuristicCostEstimate(Path_Node<Block> start, Path_Node<Block> end)
    {
        Block startBlock = start.data;
        Block endBlock = end.data;
        float xDistance = Mathf.Abs(startBlock.Point.x - endBlock.Point.x);
        float yDistance = Mathf.Abs(startBlock.Point.y - endBlock.Point.y);
        float zDistance = Mathf.Abs(startBlock.Point.y - endBlock.Point.z);

        return xDistance + yDistance + zDistance;
    }
}
