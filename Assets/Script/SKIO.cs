using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class SKIO : MonoBehaviour {

    public static  SocketIOComponent socket;

    public delegate void Event_gameMachineStatus(string _messages);
    public static Event_gameMachineStatus Evt_gameMachineStatus;

    public delegate void Event_PlayersScore(int _idx, string _messages, bool _up);
    public static Event_PlayersScore Evt_PlayersScore;

    public delegate void Event_PlayersScore_Update(int _idx);
    public static Event_PlayersScore_Update Evt_PlayersScore_Update;

    public delegate void Event_PlayersGunScore(int _idx, string _messages, bool _up);
    public static Event_PlayersGunScore Evt_PlayersGunScore;

    public delegate void Event_PlayersGunScore_Update(int _idx);
    public static Event_PlayersGunScore_Update Evt_PlayersGunScore_Update;

    public delegate void Event_InputKey(int _p, int _hpyinputkey, bool _down);
    public static Event_InputKey Evt_InputKey;
    private void Awake()
    {
        socket = this.gameObject.GetComponent<SocketIOComponent>();
        DontDestroyOnLoad(socket);

        Evt_gameMachineStatus += Handler_gameMachineStatus.Handler;
        Evt_PlayersScore += Handler_PlayersScore.Handler;
        Evt_PlayersScore_Update += Handler_PlayersScore.Handler_Update;

        Evt_PlayersGunScore += Handler_PlayersGunScore.Handler;
        Evt_PlayersGunScore_Update += Handler_PlayersGunScore.Handler_Update;

        Evt_InputKey += Handler_InputKey.Handler;

        //StartCoroutine(Coro_mobileAppConnectedConnected());
    }
    //IEnumerator Coro_mobileAppConnectedConnected()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    if (SKIO.socket != null)
    //    {
    //        SKIO.socket.Emit("mobileAppConnected");
    //    }
    //}



}
