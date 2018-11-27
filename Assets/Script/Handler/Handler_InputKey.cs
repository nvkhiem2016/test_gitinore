using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_InputKey : MonoBehaviour {

    static public Handler_InputKey instance;
    void Awake()
    {
        instance = this;

    }

     static public void Handler(int _p, int _hpyinputkey, bool _down)
    {
        if (instance != null)
        {
            Debug.Log("instance != null");
            instance.StartCoroutine(instance.Coro_InputKey(_p, _hpyinputkey, _down));

        }



    }
    IEnumerator Coro_InputKey(int _p, int _hpyinputkey, bool _down)
    {
        yield return new WaitForSeconds(0.1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = _p.ToString();
        data["data"] = _hpyinputkey.ToString();
        data["type"] = _down.ToString();

        //Debug.Log(new JSONObject(data));

        if (SKIO.socket != null)
        {
            SKIO.socket.Emit("InputKey", new JSONObject(data));
        }
    }
}
