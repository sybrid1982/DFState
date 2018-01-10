﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This determines the speed at which the game is running
// It will have two states, Run and Pause
// It also needs a hook to change the game speed
public class UpdateController : StateMachine {
    public enum GameSpeed {
        NORMAL = 1,
        DOUBLE = 2,
        FAST = 3,
        ULTRA = 4
    }

    // this is the number of seconds per tick
    private float defaultSpeed = 1;
    private float timeToNextUpdate = 0f;

    const string UpdateName = "TICK_POSTED";

    public GameSpeed Speed {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }

    GameSpeed _speed;

    void Start()
    {
        Speed = GameSpeed.NORMAL;
        if(CurrentState == null)
            ChangeState<PauseState>();
    }

    void Update()
    {
        // Time -> seconds/frame
        float time = Time.deltaTime;    
        // Seconds / Tick
        float speedModifier = (float)(Mathf.Pow(2, (int)Speed - 1));
        float timePerTick = defaultSpeed / speedModifier;
        // Debug.Log(speedModifier + ", " + timePerTick);
        // so to get whether it's time for a tick yet, check
        // whether time.deltatime has reached the threshhold
        // number of seconds for a tick
        
        // But only do this if we are in the RunState
        if(CurrentState == GetState<RunState>())
        {
            timeToNextUpdate += time;
            // Debug.Log("Next Update is " + timeToNextUpdate.ToString() + " / " + timePerTick);
            if (timeToNextUpdate >= timePerTick)
            {
                NotificationExtensions.PostNotification(this, UpdateName);
                timeToNextUpdate = 0;
            }
        }
        else
        {
            Debug.Log("Currently paused");
        }
    }
}
