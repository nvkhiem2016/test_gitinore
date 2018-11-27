using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class page1 : MonoBehaviour {
    public bool gameMachineStatus;

	public void EnableButtonPressed()
    {
        if (!gameMachineStatus)
        {
            gameMachineStatus = !gameMachineStatus;
        }
        EmitStatus();
    }
    public void DisableButtonPressed()
    {
        if (gameMachineStatus)
        {
            gameMachineStatus = !gameMachineStatus;
        }
        EmitStatus();
    }

    private void EmitStatus()
    {
        SKIO.Evt_gameMachineStatus(gameMachineStatus.ToString()); //SKIO
        Debug.Log(gameMachineStatus.ToString());
    }

}
