using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class page6_inputkey : MonoBehaviour
{

    public enum HpyInputKey
    {
        Up, Down, Left, Right, Fire, Advance, ScoreUp, LargeScoreUp, ScoreDown, LargeScoreDown, OutBounty
    , BS_Up, BS_Down, BS_Left, BS_Right, BS_Confirm, BS_Cancel, BS_GameLite

    }
    // Use this for initialization
    void Awake()
    {


    }
    //private void Start()
    //{
    //    SKIO.Evt_PlayersScore(page6.intPlayerID, "0", true);
    //    SKIO.Evt_PlayersGunScore(page6.intPlayerID, PlayerInfo.gunScore[page6.intPlayerID].ToString(), true);
    //}
    public void TargetButtonPressed()
    {
        Debug.Log("TargetButtonPressed()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Down, true);

    }
    public void TargetButtonRelease()
    {
        Debug.Log("TargetButtonRelease()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Down, false);

    }

    public void LeftButtonPressed()
    {
        Debug.Log("LeftButtonPressed()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Left, true);

    }
    public void LeftButtonRelease()
    {
        Debug.Log("LeftButtonRelease()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Left, false);

    }

    public void RightButtonPressed()
    {
        Debug.Log("RightButtonPressed()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Right, true);

    }
    public void RightButtonRelease()
    {
        Debug.Log("RightButtonRelease()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Right, false);

    }

    public void HitButtonPressed()
    {
        Debug.Log("HitButtonPressed()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Fire, true);
    }
    public void HitButtonRelease()
    {
        Debug.Log("HitButtonRelease()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Fire, false);
    }

    public void ScoreDownButtonPressed()
    {
        Debug.Log("ScoreDownButtonPressed()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.ScoreDown, true);

    }
    public void ScoreDownButtonRelease()
    {
        Debug.Log("ScoreDownButtonRelease()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.ScoreDown, false);

    }
    public void ScoreUpButtonPressed()
    {
        Debug.Log("ScoreUpButtonPressed()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.ScoreUp, true);

    }
    public void ScoreUpButtonRelease()
    {
        Debug.Log("ScoreUpButtonRelease()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.ScoreUp, false);

    }
    public void GunUpButtonPressed()
    {
        Debug.Log("GunUpButtonPressed()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Advance, true);
    }
    public void GunUpButtonRelease()
    {
        Debug.Log("GunUpButtonRelease()");
        SKIO.Evt_InputKey(page6.intPlayerID, (int)HpyInputKey.Advance, false);
    }


}
