using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.Text;

public class Logo_ServerConnection : MonoBehaviour
{
    private bool complete = false;
    private bool start = false;
    private float runningTime = 0.0f;


    // Use this for initialization
    void Start()
    {
        MainData.ServerConnection = true;
        start = false;
        complete = false;
        runningTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            runningTime += Time.deltaTime;

            if (runningTime > MainData.ServerLimitTime)
            {
                //MainData.ServerConnection = false;
            }
        }
    }

    public bool isComplete()
    {
        return complete;
    }

    public void SendLogin()
    {
        Debug.Log("SendMessage");
        start = true;

        Dictionary<string, string> logindata = new Dictionary<string, string>();
        Debug.Log("ID = " + FB.UserId);
        logindata.Add("option", "login");
        if (FB.UserId != null)
            logindata.Add("fid", FB.UserId);
        else
            logindata.Add("fid", "");

        logindata.Add("name", CMainData.Username);

        POSTLOGIN("", JsonMapper.ToJson(logindata));
    }

    public WWW POSTLOGIN(string page, string data)
    {
        FbDebug.Log("postloing test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));

        StartCoroutine(WaitForRequest(www));
        return www;
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW get Ok!: " + www.text);
            JsonData jData = JsonMapper.ToObject(www.text);
            Debug.Log("Json Data Count is " + jData.Count);

            CMainData.Gold = int.Parse(jData["gold"].ToString());
            CMainData.Dice_Count = int.Parse(jData["dice"].ToString());
            CMainData.Dice_Time = float.Parse(jData["dicetime"].ToString());
            CMainData.UserScore = int.Parse(jData["score"].ToString());
            CMainData.TopUser = jData["top"];
            CMainData.UserRanking = int.Parse(jData["ranking"].ToString());
            int nTutorial = int.Parse(jData["tutorial"].ToString());
            if (nTutorial == 1)
            {
                CMainData.Tutorial = true;
            }
            else
            {
                CMainData.Tutorial = false;
            }

            for (int i = 0; i < CMainData.TopUser.Count; i++)
            {
                string topfid = CMainData.TopUser[i]["fid"].ToString();
                FacebookPictureDownloader.EnQueue(topfid);
            }

            SendGetMessage();
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void SendGetMessage()
    {
        Debug.Log("Send GetMessageList");

        complete = false;
        start = true;

        Dictionary<string, string> getMessage = new Dictionary<string, string>();

        getMessage.Add("option", "message_list");
        getMessage.Add("receiver_fid", FB.UserId);

        POSTGETMESSAGE("", JsonMapper.ToJson(getMessage));
    }

    public WWW POSTGETMESSAGE(string page, string data)
    {
        FbDebug.Log("postgetmessage test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));

        StartCoroutine(WaitForGetMessage(www));
        return www;
    }

    private IEnumerator WaitForGetMessage(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW get Ok!: " + www.text);

            JsonData jData = JsonMapper.ToObject(www.text);
            JsonData messageList = jData["message_list"];

            Debug.Log("Json Data Count is " + messageList.Count);

            for (int i = 0; i < messageList.Count; i++)
            {
                Message temp = new Message(Convert.ToInt32(messageList[i]["type"].ToString()),
                    messageList[i]["senderName"].ToString(), messageList[i]["senderFid"].ToString());
                CMainData.message.Add(temp);
            }

            complete = true;
            start = false;
            runningTime = 0.0f;
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
