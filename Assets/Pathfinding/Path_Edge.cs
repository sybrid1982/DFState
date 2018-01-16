using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_Edge<T> {

    public float cost;  // Cost to traverse this edge (ie cost to enter the tile)

    public Path_Node<T> node;
}
