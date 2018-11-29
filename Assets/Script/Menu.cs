using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Menu : MonoBehaviour {
    public GameObject popup;
    public GameObject Body;
    public void Enablegame()
    {

        if (webservices.userPer > 0)
        {
            MenuTarget.target = "Page1";
            GameObject verifyPopup = Instantiate(popup, transform.position, transform.rotation);
            verifyPopup.transform.SetParent(Body.transform, false);
        }
        else
        {
            Debug.Log("Không có quyền");
        }

    }
    public void Decoder()
    {
        
        //verifyPopup.name = "verifyPopup"; // đặt tên
        if (webservices.userPer > 0)
        {
            MenuTarget.target = "Page3";
            GameObject verifyPopup = Instantiate(popup, transform.position, transform.rotation);
            verifyPopup.transform.SetParent(Body.transform, false);
        }
        else
        {
            Debug.Log("Không có quyền");
        }
    }
    public void PlayerScore()
    {
        MenuTarget.target = "Page4";
        GameObject verifyPopup = Instantiate(popup, transform.position, transform.rotation);
        verifyPopup.transform.SetParent(Body.transform, false);

    }
    public void GameController()
    {
        MenuTarget.target = "Page6";
        GameObject verifyPopup = Instantiate(popup, transform.position, transform.rotation);
        verifyPopup.transform.SetParent(Body.transform, false);

    }
    public void Logout()
    {
        Load("Login");

    }
    public void Load(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }
}
