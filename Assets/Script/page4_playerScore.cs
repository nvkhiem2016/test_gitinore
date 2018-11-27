using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class page4_playerScore : MonoBehaviour
{
    //public Text mValue;
    public Text playerID;
    private void Start()
    {

    }
    public void upButtonPressed()
    {
        Debug.Log("up");
        //Debug.Log(playerID.text);

        int mPlayerID = Convert.ToInt32(playerID.text) - 1;
        string mMultiphy = page4.Multiphy.ToString();
        Debug.Log(mPlayerID);
        Debug.Log(mMultiphy);
        if (mPlayerID != 99)
        {
            SKIO.Evt_PlayersScore(mPlayerID, mMultiphy, true);
        }
        else
        {
            for(int i=0; i<8; i++)
            {
                SKIO.Evt_PlayersScore(i, mMultiphy, true);

            }
        }
    }
    public void downButtonPressed()
    {
        Debug.Log("down");
        int mPlayerID = Convert.ToInt32(playerID.text) - 1;
        string mMultiphy = page4.Multiphy.ToString();

        if (mPlayerID != 99)
        {
            SKIO.Evt_PlayersScore(mPlayerID, mMultiphy, false);
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                SKIO.Evt_PlayersScore(i, mMultiphy, false);

            }
        }
    }
}
