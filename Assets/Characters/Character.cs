using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    StateMachine stateMachine;
    Block myBlock;

    private void Awake()
    {
        if (stateMachine == null) {
            stateMachine = gameObject.AddComponent<StateMachine>();
        }
        stateMachine.ChangeState<CSIdle>();
    }

    public void SetupCharacter(Block block)
    {
        SetBlock(block);
    }

    public void MoveToNewSpace(SquareDirection direction)
    {
        Debug.Log("Asked to move " + direction.ToString());
        if (myBlock.GetNeighbor(direction) != null)
            SetBlock(myBlock.GetNeighbor(direction));
    }

    private void SetBlock (Block block)
    {
        myBlock = block;

        transform.localPosition = myBlock.Point.ToCellPosition();
    }
}
