using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler_PlayersGunScore : MonoBehaviour {

    static public Handler_PlayersGunScore instance;
    void Awake()
    {
        instance = this;

    }

    static public void Handler(int _idx, string _messages, bool _up)
    {
        if (instance != null)
        {
            Debug.Log("instance != null");
            if (_messages != null)
            {

                instance.StartCoroutine(instance.Coro_Dat_PlayersGunScore(_idx, _messages, _up));

            }
        }


    }

    IEnumerator Coro_Dat_PlayersGunScore(int _idx, string _messages, bool _up)
    {
        yield return new WaitForSeconds(0.1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = _idx.ToString();
        data["data"] = _messages;
        data["type"] = _up.ToString();

        //Debug.Log(new JSONObject(data));

        if (SKIO.socket != null)
        {
            SKIO.socket.Emit("PlayersGunScore_Change", new JSONObject(data));
        }
    }
    static public void Handler_Update(int _idx)
    {
        if (instance != null)
        {
            Debug.Log("instance != null");


            instance.StartCoroutine(instance.Coro_Dat_PlayersGunScore_Update(_idx));

        }


    }
    IEnumerator Coro_Dat_PlayersGunScore_Update(int _idx)
    {
        yield return new WaitForSeconds(0.1f);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = _idx.ToString();


        //Debug.Log(new JSONObject(data));

        if (SKIO.socket != null)
        {
            SKIO.socket.Emit("PlayersGunScore_Update", new JSONObject(data));
        }
    }
}
