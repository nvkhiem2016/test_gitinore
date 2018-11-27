using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class login : MonoBehaviour {

    public GameObject id;
    public GameObject password;
    private string strid;
    private string strpassword;
    // Use this for initialization
    public void RegisterButtonPressed()
    {
        Load("Register");

    }
    public void Load(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }
    public void LoginButtonPressed()
    {
        strid = id.GetComponent<InputField>().text;
        strpassword = password.GetComponent<InputField>().text;
        if (!string.IsNullOrEmpty(strid) && !string.IsNullOrEmpty(strpassword))
        {
            StartCoroutine(SendLoginData());
        }
    }

    private IEnumerator SendLoginData()
    {
        Dictionary<string, string> passport = new Dictionary<string, string>();
        passport["id"] = strid;
        passport["password"] = strpassword;

        // Delete cookie before requesting a new login
        webservices.CookieString = null;

        var request = webservices.Post("login", new JSONObject(passport).ToString());
        yield return request.Send();

        if (request.isNetworkError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            webservices.CookieString = request.GetResponseHeader("set-cookie");
            Debug.Log(webservices.CookieString);
            Debug.Log(request.downloadHandler.text);
            if(String.Compare(request.downloadHandler.text, "OK") == 0) //so sanh ket qua tra ve == 0K => correct
            {
                Load("Menu");
                
            }

        }
    }
    
}
