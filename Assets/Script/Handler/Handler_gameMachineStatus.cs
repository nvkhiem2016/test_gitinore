using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_gameMachineStatus : MonoBehaviour {
    static public Handler_gameMachineStatus instance;
    private void Awake()
    {
             instance = this;

    }
    public static void Handler(string _message)
    {
        if (_message != null)
        {
            instance.StartCoroutine(instance.Coro_gameMachineStatus(_message));

        }

    }
    IEnumerator Coro_gameMachineStatus(string _message)
    {
        yield return new WaitForSeconds(0.1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["data"] = _message;

        if (SKIO.socket != null)
        {
            SKIO.socket.Emit("gamemachinestatus", new JSONObject(data));
        }
    }
}
