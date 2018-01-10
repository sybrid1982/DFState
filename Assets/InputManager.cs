using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };
    const string buttonNotification = "Button_Notification";

    KeyCode[] _moveKeys = new KeyCode[] { KeyCode.A, KeyCode.W, KeyCode.D, KeyCode.S };
    const string moveKeyNotification = "Move_Key_Notification";

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		for(int i = 0; i < _buttons.Length; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                NotificationExtensions.PostNotification(this, buttonNotification, i);
            }
        }
        for(int i = 0; i < _moveKeys.Length; ++i)
        {
            if (Input.GetKey(_moveKeys[i]))
            {
                NotificationExtensions.PostNotification(this, moveKeyNotification, _moveKeys[i]);
            }
        }
	}
}
