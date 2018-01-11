using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSIdle : CSState {
    Character character;

    public override void Enter()
    {
        base.Enter();
        character = transform.root.GetComponent<Character>();
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        NotificationExtensions.AddObserver(this, OnUpdateTick, UpdateNotice);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        NotificationExtensions.RemoveObserver(this, OnUpdateTick, UpdateNotice);
    }

    protected override void OnUpdateTick(object sender, object info)
    {
        base.OnUpdateTick(sender, info);
        // Check for urgent needs
        // Then check for jobs this character should be doing
        // Then check for whether this character is in a social space
        // Then just move to a space in one of the four cardinal directions
        MoveToRandomSpace();
    }

    private void MoveToRandomSpace()
    {
        int random = Random.Range(0, 4);
        random *= 2;
        SquareDirection moveDirection = (SquareDirection)random;
        character.MoveToNewSpace(moveDirection);
    }
}