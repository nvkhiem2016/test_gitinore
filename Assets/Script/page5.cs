using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using SocketIO;

public class page5 : MonoBehaviour {
    public Scrollbar mScrollbar;
    public Text ticketText;

    public Text[] textField = new Text[9];
    public static int Multiphy = 10;
    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        mScrollbar.onValueChanged.AddListener(delegate { ChangeTicketTextValue(); });
        //textField[0].text = "1234"; 
        //socket listen
        long tempEqual = 0;
        SKIO.socket.On("PlayersGunScore", (SocketIOEvent msg) => {
            string mId = msg.data["player"].ToString().Split('\"')[1];
            string mValue = msg.data["score"].ToString().Split('\"')[1];

            textField[Convert.ToInt32(mId)].text = mValue;

            //tempEqual +=Convert.ToInt32(textField[Convert.ToInt32(mId)].text);
            for (int i = 0; i < 8; i++)
            {
                tempEqual += Convert.ToInt32(textField[i].text);
            }
            textField[8].text = tempEqual.ToString();
            tempEqual = 0;

        });

    }
    // Invoked when the value of the slider changes.
    public void ChangeTicketTextValue()
    {
        int mScrollbarValue = (int)(mScrollbar.value * 10); //with out 9
        Debug.Log(mScrollbarValue); // 
        switch (mScrollbarValue)
        {
            case 0:
                Multiphy = 10;
                ticketText.text = "10";
                break;
            case 1:
                Multiphy = 20;
                ticketText.text = "20";
                break;
            case 2:
                Multiphy = 50;
                ticketText.text = "50";
                break;
            case 3:
                Multiphy = 100;
                ticketText.text = "100";
                break;
            case 4:
                Multiphy = 200;
                ticketText.text = "200";
                break;
            case 5:
                Multiphy = 500;
                ticketText.text = "500";
                break;
            case 6:
                Multiphy = 1000;
                ticketText.text = "1000";
                break;
            case 7:
                Multiphy = 2000;
                ticketText.text = "2000";
                break;
            case 8:
                Multiphy = 5000;
                ticketText.text = "5000";
                break;
            case 10:
                Multiphy = 9000;
                ticketText.text = "9000";
                break;
            default:
                Multiphy = 0;
                ticketText.text = "0";
                break;
        }
    }
}
