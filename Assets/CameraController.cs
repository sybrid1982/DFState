using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private int _displayZ;

    public Map map;

    public float cameraSpeed = 5f;

    public GameObject focusPoint;

    private const string zChange = "CAMERA_Z_CHANGED";
    const string moveKeyNotification = "Move_Key_Notification";

    Vector3 moveDirection;

    public int DisplayZ {
        get {return _displayZ ;}
        set {
            if (value >= 0 && value < map.depth)
            {
                _displayZ = value;
                NotificationExtensions.PostNotification(this, zChange, value);
            }
        }
    }

    private void Start()
    {
        DisplayZ = 0;

        NotificationExtensions.AddObserver(this, OnMoveCameraKeyCommand, moveKeyNotification);
    }

    private void OnMoveCameraKeyCommand(object sender, object e)
    {
        if (e == null)
            return;
        KeyCode input = (KeyCode)e;

        switch (input)
        {
            case KeyCode.A:
                moveDirection += Vector3.left;
                break;
            case KeyCode.W:
                moveDirection += Vector3.forward;
                break;
            case KeyCode.D:
                moveDirection += Vector3.right;
                break;
            default:
                moveDirection += Vector3.back;
                break;
        }
    }

    private void Update()
    {
        if (moveDirection == Vector3.zero)
            return;

        Vector3 newPosition = focusPoint.transform.position + moveDirection * Time.deltaTime * cameraSpeed;
        newPosition.x = Mathf.Clamp(newPosition.x, 0, (map.length * SquareMetrics.sideLength));
        newPosition.z = Mathf.Clamp(newPosition.z, -5, (map.width * SquareMetrics.sideLength));
        focusPoint.transform.position = newPosition;
        moveDirection = Vector3.zero;
    }
}
