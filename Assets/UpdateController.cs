using System.Collections;
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
    private float timeToNextCentitick = 0f;

    const string UpdateName = "TICK_POSTED";
    const string CentiUpdate = "CENTITICK_POSTED";

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
        // so to get whether it's time for a tick yet, check
        // whether time.deltatime has reached the threshhold
        // number of seconds for a tick

        // We are also going to need a time per centitick,
        // Which for now will be for movement but in the future may be useful for other things
        float timePerCentiTick = timePerTick / 100f;
        
        // But only do this if we are in the RunState
        if(CurrentState == GetState<RunState>())
        {
            timeToNextUpdate += time;
            timeToNextCentitick += time;

            // Debug.Log("Next Update is " + timeToNextUpdate.ToString() + " / " + timePerTick);
            if (timeToNextCentitick >= timePerCentiTick)
            {
                NotificationExtensions.PostNotification(this, CentiUpdate);
                timeToNextCentitick = 0;
            }

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
