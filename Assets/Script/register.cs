using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class register : MonoBehaviour {
    public GameObject fullname,id,password,confirmpassword;

    private string strfullname, strid, strpassword, strconfirmpassword;
    // Use this for initialization
    public void CreateButtonPressed()
    {
        strfullname = fullname.GetComponent<InputField>().text;
        strid = id.GetComponent<InputField>().text;
        strpassword = password.GetComponent<InputField>().text;
        strconfirmpassword=confirmpassword.GetComponent<InputField>().text;

        if (!string.IsNullOrEmpty(strid) && !string.IsNullOrEmpty(strpassword) && !string.IsNullOrEmpty(strfullname) && !string.IsNullOrEmpty(strconfirmpassword))
        {
            if (String.Compare(strpassword, strconfirmpassword) == 0)
            {
                StartCoroutine(SendCreateData());
            }
            else
            {
                Debug.Log("Wrong confirm password!");
            }
        }
    }

    private IEnumerator SendCreateData()
    {
        Dictionary<string, string> passport = new Dictionary<string, string>();
        passport["name"] = strfullname;
        passport["id"] = strid;
        passport["password"] = strpassword;

        // Delete cookie before requesting a new login
        webservices.CookieString = null;

        var request = webservices.Post("signup", new JSONObject(passport).ToString());
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
            if (String.Compare(request.downloadHandler.text, "OK") == 0) //so sanh ket qua tra ve == 0K => correct
            {
                Load("Login");
            }
        }
    }
    public void BackLoginPressed()
    {
        Debug.Log("Back pressed");
        Load("Login");

    }
    public void Load(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }
}
