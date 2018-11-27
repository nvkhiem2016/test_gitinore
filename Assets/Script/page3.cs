using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

//using Newtonsoft.Json.Linq;

public class page3 : MonoBehaviour
{
    public Text outputtext;
    public GameObject inputfield1, inputfield2, inputfield3, inputfield4, inputfield5;
    private string strInputField1, strInputField2, strInputField3, strInputField4, strInputField5;
    private ushort[] Jym = new ushort[4];
    private UInt32[] key = new UInt32[4];
    public void getPressed()
    {

        StartCoroutine(getKey());

    }
    public void Decoder()
    {
        strInputField1 = inputfield1.GetComponent<InputField>().text;
        strInputField2 = inputfield2.GetComponent<InputField>().text;
        strInputField3 = inputfield3.GetComponent<InputField>().text;
        strInputField4 = inputfield4.GetComponent<InputField>().text;
        strInputField5 = inputfield5.GetComponent<InputField>().text;

        if (!string.IsNullOrEmpty(strInputField1) &&
            !string.IsNullOrEmpty(strInputField2) &&
            !string.IsNullOrEmpty(strInputField3) &&
            !string.IsNullOrEmpty(strInputField4) &&
            !string.IsNullOrEmpty(strInputField5))
        {

            Jym[0] = Convert.ToUInt16(Convert.ToUInt32(strInputField5) / 1000);
            Jym[1] = Convert.ToUInt16(Convert.ToUInt32(strInputField5) % 1000 / 100);
            Jym[2] = Convert.ToUInt16(Convert.ToUInt32(strInputField5) % 1000 % 100 / 10);
            Jym[3] = Convert.ToUInt16(Convert.ToUInt32(strInputField5) % 1000 % 100 % 10);

            string codePrintSuccess = GlobalMembersDaMa.JiSuan_DKXZM_decrypt(
                                        Convert.ToUInt32(Math.Abs(Convert.ToInt64(strInputField1))),
                                        Convert.ToUInt32(Math.Abs(Convert.ToInt64(strInputField2))),
                                        Convert.ToUInt32(strInputField3),
                                        Convert.ToUInt32(strInputField4),
                                        Jym,
                                        key);
            Debug.Log(codePrintSuccess);

            //Print codePrintSuccess to Output Zone
            outputtext.text = codePrintSuccess;
            //Debug.Log(Jym);
        }
    }
    private IEnumerator getKey()
    {
        //getKey
        var request = webservices.Get("getKey");

        yield return request.Send();

        if (request.isNetworkError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            webservices.CookieString = request.GetResponseHeader("set-cookie");
            //Debug.Log(webservices.CookieString);
            Debug.Log(request.downloadHandler.text);
            String test = request.downloadHandler.text.TrimStart('[').TrimEnd(']');
            String[] test1 = test.Split(',');
            key[0] = Convert.ToUInt32(test1[0]);
            key[1] = Convert.ToUInt32(test1[1]);
            key[2] = Convert.ToUInt32(test1[2]);
            key[3] = Convert.ToUInt32(test1[3]);
            Decoder();

        }
    }


}

[System.Serializable]
public class ResBody
{
    public int password;

}
