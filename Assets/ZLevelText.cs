using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZLevelText : MonoBehaviour {
    public Text zValueText;

    private const string zChange = "CAMERA_Z_CHANGED";

    // Use this for initialization
    void Awake () {
        NotificationExtensions.AddObserver(this, OnZLevelUpdated, zChange);
	}

    void OnZLevelUpdated(object sender, object e)
    {
        zValueText.text = e.ToString();
    }
}
