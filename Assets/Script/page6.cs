using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using SocketIO;

public class page6 : MonoBehaviour {
    public Text playerID;
    public static string strPlayerID;
    public static int intPlayerID=0;
    public Text PlayerScoreText;
    public Text GunScoreText;

    private void Awake()
    {
        playerID.text = intPlayerID.ToString();
    }
    public void LeftButtonChangePlayerPressed()
    {
        Debug.Log("LeftButtonChangePlayerPressed()");
        if (intPlayerID > 0)
        {
            intPlayerID -= 1;

        }
        else
        {
            intPlayerID = Defineds.numPlayerMax - 1;

        }
        playerID.text = intPlayerID.ToString();


        SKIO.Evt_PlayersScore_Update(intPlayerID);
        SKIO.Evt_PlayersGunScore_Update(intPlayerID);



    }
    public void RightButtonChangePlayerPressed()
    {
        Debug.Log("RightButtonChangePlayerPressed()");
        if (intPlayerID < Defineds.numPlayerMax - 1)
        {
            intPlayerID += 1;


        }
        else
        {
            intPlayerID = 0;

        }
        playerID.text = intPlayerID.ToString();

        SKIO.Evt_PlayersScore_Update(intPlayerID);
        SKIO.Evt_PlayersGunScore_Update(intPlayerID);


    }

    private void Start()
    {
        SKIO.Evt_PlayersScore_Update(intPlayerID);
        SKIO.Evt_PlayersGunScore_Update(intPlayerID);

        SKIO.socket.On("PlayersScore", (SocketIOEvent msg) => {
            string mId = msg.data["player"].ToString().Split('\"')[1];
            string mValue = msg.data["score"].ToString().Split('\"')[1];

            if (Convert.ToInt32(mId) == intPlayerID)
            {
                PlayerScoreText.text = mValue;

            }

        });

        SKIO.socket.On("PlayersGunScore", (SocketIOEvent msg) => {
            string mId = msg.data["player"].ToString().Split('\"')[1];
            string mValue = msg.data["score"].ToString().Split('\"')[1];
            if (Convert.ToInt32(mId) == intPlayerID)
            {
                GunScoreText.text = mValue;

            }

        });
    }



}
