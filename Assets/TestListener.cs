using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListener : MonoBehaviour {
    const string UpdateName = "TICK_POSTED";
    int numberOfTicks = 0;

    // Use this for initialization
    void Start () {
        this.AddObserver(OnTest, UpdateName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTest(object sender, object info)
    {
        numberOfTicks++;
        Debug.Log("Notification posted of tick update " + numberOfTicks);
    }
}
