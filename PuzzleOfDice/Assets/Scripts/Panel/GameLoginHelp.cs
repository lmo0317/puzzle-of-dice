using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using CallBackMethod;

public class GameLoginHelp : MonoBehaviour
{
    public SceneTitle sceneTitle;
    public RequestFriendItem[] ItemList;
    public GameObject loading;

    public static GameLoginHelp g_instance = null;
    public static string tempFid = null;

    // Use this for initialization
    void Start()
    {
        string friendlist = "";

        tempFid = null;
        g_instance = this;

        foreach (Dictionary<string, string> temp in CMainData.appFriends)
        {
            friendlist += (string)temp["fid"] + ",";
        }

        if (friendlist.Length != 0)
        {
            friendlist = friendlist.Substring(0, friendlist.Length - 1);
        }

        /*if (Title_ServerConnection.g_instance != null)
        {
            Title_ServerConnection.g_instance.MessageCheck(friendlist, new CallBackClass.MessageCheckCallBack(success));
        }*/
        if (ServerConnection.g_instance != null)
        {
            ServerConnection.g_instance.MessageCheck(friendlist, new CallBackClass.MessageCheckCallBack(success));
        }
    }

    public void success(JsonData result)
    {
        List<Dictionary<string, string>> usingAppFriends = null;

        if (usingAppFriends != null)
        {
            usingAppFriends.Clear();
        }

        usingAppFriends = new List<Dictionary<string, string>>(CMainData.appFriends);

        JsonData resultData = result["result"];

        {
            int i = 0;
            foreach (Dictionary<string, string> temp in CMainData.appFriends)
            {
                //Debug.Log(resultData[i].ToString());
                if (resultData[i].ToString().Equals("0"))
                {
                    foreach (Dictionary<string, string> apptemp in usingAppFriends)
                    {
                        if (((string)temp["fid"]).Equals(apptemp["fid"]))
                        {
                            usingAppFriends.Remove(apptemp);
                            break;
                        }
                    }
                }
                i++;
            }
        }


        if (usingAppFriends.Count <= 0)
        {
            if (sceneTitle != null)
            {
                sceneTitle.changeIsMain(true);
                this.gameObject.SetActive(false);
            }
        }

        loading.SetActive(true);

        if (usingAppFriends.Count <= 6)
        {
            Debug.Log("appFriends Count is <= 6");

            var i = 0;
            foreach (Dictionary<string, string> temp in usingAppFriends)
            {
                ItemList[i].SetFriendContext(temp["name"].ToString(), temp["fid"].ToString());
                ItemList[i].gameObject.SetActive(true);
                i++;
            }

            for (; i < 6; i++)
            {
                ItemList[i].gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("appFriends Count is > 6");

            var i = 0;
            var j = 0;
            List<int> prev = new List<int>();

            while (true)
            {
                var temp = Random.Range(0, CMainData.appFriends.Count - 1);
                Debug.Log("Random temp is " + temp);

                if (!prev.Contains(temp))
                {
                    prev.Add(temp);
                    ItemList[i].SetFriendContext(usingAppFriends[temp]["name"].ToString(), usingAppFriends[temp]["fid"].ToString());
                    ItemList[i].gameObject.SetActive(true);
                    i++;
                }
                j++;

                if (i > 5 || j > 500)
                {
                    break;
                }
            }
        }

        loading.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RequestButton()
    {
        SceneTitle.ButtonClickSound();

        string receiverFidList = "";
        // Server 전송

        foreach (RequestFriendItem temp in ItemList)
        {
            if (temp.btn_Toggle.value && temp.Fid != null)
            {
                // Server Send Message. Or Send List.
                receiverFidList += temp.Fid + ",";
            }
        }

        if (!receiverFidList.Equals(""))
        {
            receiverFidList = receiverFidList.Substring(0, receiverFidList.Length - 1);

            tempFid = receiverFidList;

            //Debug.Log("receiver list is " + receiverFidList);

            //Title_ServerConnection.g_instance.SendMessage(receiverFidList, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1);

            try
            {
                FaceBook.CallAppRequestAsDirectRequestMessage("Help You!", CMainData.Username + " " + Localization.Localize("3018"), receiverFidList, Callback);// + " 님이 당신에게 주사위를 선물했습니다.", receiverFidList);
                //status = "Direct Request called";
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

            /*
            if (ServerConnection.g_instance != null)
            {
                ServerConnection.g_instance.SendMessage(receiverFidList, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1);
            }
            */
        }

        /*
        this.gameObject.SetActive(false);

        if (SceneTitle.instance != null)
        {
            SceneTitle.instance.changeIsMain(true);
        }
        */
    }

    public static void Callback(FBResult result)
    {
        if (SceneGameOver.g_instance != null)
        {
            SceneGameOver.g_instance.buttonClickable = true;
        }

        if (result.Error != null)
        {
            Debug.Log("Error Response:\n" + result.Error);
        }
        else
        {
            Debug.Log("Success Response:\n" + result.Text);

            if (!Util.DeserializeCallbackResult(result.Text).Contains("true"))
            {
                if (ServerConnection.g_instance != null)
                {
                    ServerConnection.g_instance.SendMessage(tempFid, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1);
                }

                if (GameLoginHelp.g_instance != null)
                {
                    GameLoginHelp.g_instance.gameObject.SetActive(false);

                    if (SceneTitle.instance != null)
                    {
                        SceneTitle.instance.changeIsMain(true);
                    }
                }
            }
        }
    }
}
