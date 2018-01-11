using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {
    Camera mainCamera;
    CameraController cc;
    Map map;
    SquareGrid squareGrid;

    Block[,] displayLayer;

    private void Awake()
    {
        squareGrid = GetComponent<SquareGrid>();
    }

    // We need the map and the camera
    // The camera has the map, however
    private void Start()
    {
        mainCamera = Camera.main;
        cc = mainCamera.GetComponent<CameraController>();
        if (cc == null)
            Debug.LogError("No CameraController found!");

        map = cc.map;

        Block[,] layer = map.GetMapLayer(cc.DisplayZ);

        squareGrid.CreateMap(layer, map.length, map.width);
        map.SpawnStartingCharacter();
    }
}