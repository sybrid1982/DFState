using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum IHState
{
    Inspect,
    Mine
}

public class InputHandler : MonoBehaviour {
    SquareGrid grid;
    IHState handlerState = IHState.Inspect;
    public Text inspectionText;

    const string buttonNotification = "Button_Notification";

    public void SetGrid(SquareGrid grid)
    {
        this.grid = grid;
    }

    private void Start()
    {
        NotificationExtensions.AddObserver(this, OnFire, buttonNotification);
    }

    void OnFire(object sender, object e)
    {
        SquareCell cell = null;
        if ((int)e == 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                cell = grid.GetCell(hit.point);
            }
        }
        if(cell != null)
        {
            ActOnCell(cell);
        }
    }

    void ActOnCell(SquareCell cell)
    {
        if(handlerState == IHState.Inspect)
        {
            Inspect(cell);
        } else if (handlerState == IHState.Mine)
        {
            Mine(cell);
        }
    }

    void Inspect(SquareCell cell)
    {
        inspectionText.text = cell.point.ToString() + "\n" + cell.block.Type.ToString();
    }

    void Mine(SquareCell cell)
    {
        if(cell.block.Type is BS_Solid)
        {
            NotificationExtensions.PostNotification(this, JobMetrics.MINING_JOB_POST_NOTICE, cell.block);
        }
    }

    public void SetIHState (int index)
    {
        handlerState = (IHState)index;
    }
}
