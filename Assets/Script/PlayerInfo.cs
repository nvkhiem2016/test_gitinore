using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using SocketIO;

public class PlayerInfo : MonoBehaviour {
    public static int[] playerScore = new int[Defineds.numPlayerMax];
    public static int[] gunScore = new int[Defineds.numPlayerMax];

    // Use this for initialization
    void Start () {
        for(int i=0; i< Defineds.numPlayerMax; i++)
        {
            playerScore[i] = 0;
            gunScore[i] = 0;
        }
        
        SKIO.socket.On("PlayersScore", (SocketIOEvent msg) => {
            string mId = msg.data["player"].ToString().Split('\"')[1];
            int intId = Convert.ToInt32(mId);
            string mValue = msg.data["score"].ToString().Split('\"')[1];
            int intValue = Convert.ToInt32(mValue);

            playerScore[intId] = intValue;

        });

        SKIO.socket.On("PlayersGunScore", (SocketIOEvent msg) => {
            string mId = msg.data["player"].ToString().Split('\"')[1];
            int intId = Convert.ToInt32(mId);
            string mValue = msg.data["score"].ToString().Split('\"')[1];
            int intValue = Convert.ToInt32(mValue);

            gunScore[intId] = intValue;


        });

        //SKIO.Evt_PlayersScore(page6.intPlayerID, "0", true);
        //SKIO.Evt_PlayersGunScore(page6.intPlayerID, PlayerInfo.gunScore[page6.intPlayerID].ToString(), true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
