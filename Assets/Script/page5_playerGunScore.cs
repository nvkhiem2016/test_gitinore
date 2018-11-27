using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class page5_playerGunScore : MonoBehaviour
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
        string mMultiphy = page5.Multiphy.ToString();
        Debug.Log(mPlayerID);
        Debug.Log(mMultiphy);
        if (mPlayerID != 99)
        {
            SKIO.Evt_PlayersGunScore(mPlayerID, mMultiphy, true);
        }
        else
        {
            for(int i=0; i<8; i++)
            {
                SKIO.Evt_PlayersGunScore(i, mMultiphy, true);

            }
        }
    }
    public void downButtonPressed()
    {
        Debug.Log("down");
        int mPlayerID = Convert.ToInt32(playerID.text) - 1;
        string mMultiphy = page5.Multiphy.ToString();

        if (mPlayerID != 99)
        {
            SKIO.Evt_PlayersGunScore(mPlayerID, mMultiphy, false);
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                SKIO.Evt_PlayersGunScore(i, mMultiphy, false);

            }
        }
    }
}
