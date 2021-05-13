using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.Text;
using CallBackMethod;

public class ServerConnection : MonoBehaviour {

    public static ServerConnection g_instance = null;

    private bool complete = false;
    private bool start = false;
    private float runningTime = 0.0f;

    public GameObject loading;


	// Use this for initialization
	void Start () {
        g_instance = this;
        MainData.ServerConnection = true;
        start = false;
        complete = false;
        runningTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (start)
        {
            runningTime += Time.deltaTime;

            if (runningTime > MainData.ServerLimitTime)
            {
                MainData.ServerConnection = false;
            }
        }
	}

    //Title

    public bool isComplete()
    {
        return complete;
    }

    public void SendDiceIncrease()
    {
        Debug.Log("SendMessage");

        /*
        if (!Application.loadedLevelName.Equals("GameScene"))
        {
            loading.SetActive(true);
        }
        */

        complete = false;
        start = true;

        Dictionary<string, string> diceIncrease = new Dictionary<string, string>();

        diceIncrease.Add("option", "dicepluse");
        diceIncrease.Add("fid", FB.UserId);

        POSTINCREASEDICE("", JsonMapper.ToJson(diceIncrease));
    }

    public WWW POSTINCREASEDICE(string page, string data)
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
            CMainData.Dice_Count = int.Parse(jData["dice"].ToString());
            CMainData.Dice_Time = float.Parse(jData["dicetime"].ToString());

            if (CMainData.Dice_Count > 4)
            {
                CMainData.Dice_Time = -1;
            }

            complete = true;
            start = false;
            runningTime = 0.0f;

            loading.SetActive(false);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void SendBuyDice(CallBackClass.MessageCheckCallBack callback)
    {
        Debug.Log("SendMessage");

        loading.SetActive(true);
        complete = false;
        start = true;

        Dictionary<string, string> buyDice = new Dictionary<string, string>();

        buyDice.Add("option", "buy_dice");
        buyDice.Add("fid", FB.UserId);

        POSTBUYDICE("", JsonMapper.ToJson(buyDice), callback);
    }

    public WWW POSTBUYDICE(string page, string data, CallBackClass.MessageCheckCallBack callback)
    {
        FbDebug.Log("postmessagecheck test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));
        StartCoroutine(WaitForMessageCheck(www, callback));
        return www;
    }

    public void ReceiveMessage(Message msg, int action)
    {
        Debug.Log("ReceiveMessage");

        loading.SetActive(true);

        complete = false;
        start = true;

        Dictionary<string, string> receivemessage = new Dictionary<string, string>();

        receivemessage.Add("option", "receive_message");

        switch (action)
        {
            case (int)RECEIVEMESSAGEACTIONTYPE.ACCEPT:
                receivemessage.Add("sub_option", "accept");
                break;
            case (int)RECEIVEMESSAGEACTIONTYPE.CANCLE:
                receivemessage.Add("sub_option", "cancle");
                break;
            default: // ERROR
                return;
        }

        receivemessage.Add("sender_fid", msg.Fid);
        receivemessage.Add("receiver_fid", FB.UserId);
        receivemessage.Add("message_type", msg.Type.ToString());

        POSTRECEIVEMESSAGE("", JsonMapper.ToJson(receivemessage));
    }

    public WWW POSTRECEIVEMESSAGE(string page, string data)
    {
        FbDebug.Log("postreceivemessage test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));

        StartCoroutine(WaitForReceiveMessage(www));
        return www;
    }

    private IEnumerator WaitForReceiveMessage(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW get Ok!: " + www.text);

            JsonData jData = JsonMapper.ToObject(www.text);
            CMainData.Dice_Count = Convert.ToInt32(jData["result"].ToString());

            complete = true;
            start = false;
            runningTime = 0.0f;

            loading.SetActive(false);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void MessageCheck(string friend_list, CallBackClass.MessageCheckCallBack callback)
    {
        Debug.Log("SendMessage");

        loading.SetActive(true);

        complete = false;
        start = true;

        Dictionary<string, string> sendmessage = new Dictionary<string, string>();

        sendmessage.Add("option", "message_check");

        //sendmessage.Add("sender_fid", "100003244051522");
        sendmessage.Add("sender_fid", FB.UserId);

        sendmessage.Add("friend_list", friend_list);
        POSTMESSAGECHECK("", JsonMapper.ToJson(sendmessage), callback);
    }

    public WWW POSTMESSAGECHECK(string page, string data, CallBackClass.MessageCheckCallBack callback)
    {
        FbDebug.Log("postmessagecheck test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));

        StartCoroutine(WaitForMessageCheck(www, callback));
        return www;
    }

    private IEnumerator WaitForMessageCheck(WWW www, CallBackClass.MessageCheckCallBack callback)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW get Ok!: " + www.text);

            complete = true;
            start = false;
            runningTime = 0.0f;

            JsonData result = JsonMapper.ToObject(www.text);

            //Debug.Log("result Data Count is " + result.Count);

            loading.SetActive(false);
            callback(result);
        }
        else
        {
            if (SceneTitle.instance != null)
            {
                SceneTitle.instance.BuyDiceFalse();
            }

            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void SendMessage(string fid, int msgType)
    {
        Debug.Log("SendMessage");

        loading.SetActive(true);

        complete = false;
        start = true;

        Dictionary<string, string> sendmessage = new Dictionary<string, string>();

        sendmessage.Add("option", "send_message_list");

        //sendmessage.Add("sender_fid", "100003244051522");
        sendmessage.Add("sender_fid", FB.UserId);

        sendmessage.Add("receiver_fid", fid);
        sendmessage.Add("message_type", msgType.ToString());

        POSTSENDMESSAGE("", JsonMapper.ToJson(sendmessage));
    }

    public WWW POSTSENDMESSAGE(string page, string data)
    {
        FbDebug.Log("postsendmessage test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));

        StartCoroutine(WaitForSendMessage(www));
        return www;
    }

    private IEnumerator WaitForSendMessage(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW get Ok!: " + www.text);

            complete = true;
            start = false;
            runningTime = 0.0f;

            loading.SetActive(false);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void GameStart()
    {
        Debug.Log("SendMessage");

        loading.SetActive(true);

        complete = false;
        start = true;

        Dictionary<string, string> startmessage = new Dictionary<string, string>();
        startmessage.Add("option", "game_start");
        startmessage.Add("fid", FB.UserId);
        POSTGAMESTART("", JsonMapper.ToJson(startmessage));
    }

    public WWW POSTGAMESTART(string page, string data)
    {
        FbDebug.Log("postsendmessage test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));

        StartCoroutine(WaitForGameStartMessage(www));
        return www;
    }

    private IEnumerator WaitForGameStartMessage(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW get Ok!: " + www.text);

            JsonData result = JsonMapper.ToObject(www.text);

            if (result["result"].ToString().Equals("1"))
            {
                //Server Response True. -> Game Start.

                if (CMainData.Dice_Count > 4)
                {
                    CMainData.Dice_Time = 0.0f;
                }

                CMainData.Dice_Count -= 1;

                CMainData.setGameMode(GameData.GAME_MODE_BLITZ); //set game mode
                CMainData.setGameState(GameData.GAME_STATE_READY); //set game state
                Application.LoadLevel("GameScene"); //load game scene
            }
            else
            {
                //Server Response False. -> Game Start Fail.
            }

            complete = true;
            start = false;
            runningTime = 0.0f;

            loading.SetActive(false);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void PriceCheck(string url, CallBackClass.PriceCheckCallBack callback)
    {
        Debug.Log("PriceCheck");
        loading.SetActive(true);
        runningTime = 0.0f;
        complete = false;
        start = true;
        POSTPRICECHECK(url, callback);
    }

    public WWW POSTPRICECHECK(string url, CallBackClass.PriceCheckCallBack callback)
    {
        WWW www = new WWW(url, Encoding.Default.GetBytes(""));
        StartCoroutine(WaitForPriceCheck(www, callback));
        return www;
    }

    private IEnumerator WaitForPriceCheck(WWW www, CallBackClass.PriceCheckCallBack callback)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW get Ok!: " + www.text);

            complete = true;
            start = false;
            runningTime = 0.0f;

            loading.SetActive(false);
            callback(www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    //Game

    public void SendTutorialEnd(CallBackClass.MessageCheckCallBack callback)
    {
        Debug.Log("GameEndMessage");

        loading.SetActive(true);

        complete = false;
        start = true;

        Dictionary<string, string> endmessage = new Dictionary<string, string>();

        endmessage.Add("option", "tutorial_end");
        endmessage.Add("fid", FB.UserId);

        POSTTutorialEnd("", JsonMapper.ToJson(endmessage), callback);
    }

    public WWW POSTTutorialEnd(string page, string data, CallBackClass.MessageCheckCallBack callback)
    {
        FbDebug.Log("post_tutorial_end test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));

        StartCoroutine(WaitForTutorialEndMessage(www, callback));
        return www;
    }

    private IEnumerator WaitForTutorialEndMessage(WWW www, CallBackClass.MessageCheckCallBack callback)
    {
        yield return www;
        if (www.error == null)
        {
            //서버에서 메세지 받기 성공
            Debug.Log("WWW get Ok!: " + www.text);
            JsonData result = JsonMapper.ToObject(www.text);
            complete = true;

            //지정한 콜백함수 호출
            callback(result);
        }
        else
        {

        }
    }

    //GameOver

    public void SendGameEnd(string score, string friendlist, CallBackClass.MessageCheckCallBack callback)
    {
        Debug.Log("GameEndMessage");
        loading.SetActive(true);
        complete = false;
        start = true;
        Dictionary<string, string> endmessage = new Dictionary<string, string>();
        endmessage.Add("option", "friend_ranking");
        endmessage.Add("score", score);
        endmessage.Add("sender_fid", FB.UserId);
        endmessage.Add("friend_list", friendlist);
        POSTGameEnd("", JsonMapper.ToJson(endmessage), callback);
    }

    public WWW POSTGameEnd(string page, string data, CallBackClass.MessageCheckCallBack callback)
    {
        FbDebug.Log("postgameend test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));

        StartCoroutine(WaitForGameEndMessage(www, callback));
        return www;
    }

    private IEnumerator WaitForGameEndMessage(WWW www, CallBackClass.MessageCheckCallBack callback)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW get Ok!: " + www.text);

            JsonData result = JsonMapper.ToObject(www.text);

            complete = true;
            //start = false;
            //runningTime = 0.0f;

            callback(result);

            if (SceneGameOver.g_instance != null)
                SceneGameOver.g_instance.gameEndUIStart(true);
            SendInitialize();
        }
        else
        {
            if (SceneGameOver.g_instance != null)
                SceneGameOver.g_instance.gameEndUIStart(false);
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void SendInitialize()
    {
        Debug.Log("SendMessage");
        start = true;

        Dictionary<string, string> logindata = new Dictionary<string, string>();

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

        StartCoroutine(WaitForRequest_Login(www));
        return www;
    }

    private IEnumerator WaitForRequest_Login(WWW www)
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

            SendGetMessage();
        }
        else
        {
            if (SceneGameOver.g_instance != null)
                SceneGameOver.g_instance.gameEndUIStart(false);
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void SendGetMessage()
    {
        Debug.Log("Send GetMessageList");

        complete = false;
        start = true;

        Dictionary<string,  string> getMessage = new Dictionary<string, string>();

        getMessage.Add("option", "message_list");
        //getMessage.Add("receiver_fid", "100003244051522");
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
            if (SceneGameOver.g_instance != null)
                SceneGameOver.g_instance.gameEndUIStart(true);
        }
        else
        {
            if (SceneGameOver.g_instance != null)
                SceneGameOver.g_instance.gameEndUIStart(false);
            Debug.Log("WWW Error: " + www.error);
        }

        loading.SetActive(false);
    }

    public void SendMessage(string fid, int msgType, CallBackClass.SendMessageCallBack callback)
    {
        Debug.Log("SendMessage");

        loading.SetActive(true);

        complete = false;
        start = true;

        Dictionary<string, string> sendmessage = new Dictionary<string, string>();

        sendmessage.Add("option", "send_message_list");

        //sendmessage.Add("sender_fid", "100003244051522");
        sendmessage.Add("sender_fid", FB.UserId);

        sendmessage.Add("receiver_fid", fid);
        sendmessage.Add("message_type", msgType.ToString());

        POSTSENDMESSAGE("", JsonMapper.ToJson(sendmessage), fid, callback);
    }

    public WWW POSTSENDMESSAGE(string page, string data, string fid, CallBackClass.SendMessageCallBack callback)
    {
        FbDebug.Log("postsendmessage test : " + data);
        WWW www = new WWW(CDefine.serverURL, Encoding.Default.GetBytes(data));

        StartCoroutine(WaitForSendMessage(www, fid, callback));
        return www;
    }

    private IEnumerator WaitForSendMessage(WWW www, string fid, CallBackClass.SendMessageCallBack callback)
    {
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW get Ok!: " + www.text);

            complete = true;
            start = false;
            runningTime = 0.0f;

            loading.SetActive(false);

            callback(fid);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
