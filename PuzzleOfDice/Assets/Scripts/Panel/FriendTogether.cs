using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using CallBackMethod;

public class FriendTogether : MonoBehaviour
{
    public static FriendTogether instance = null;

    //public GameObject friendTogetherPref;

    //public UIGrid leftGrid;
    //public UIGrid rightGrid;

    public FriendTogetherItem[] leftItem;
    public FriendTogetherItem[] rightItem;

    public GameObject[] leftGridnon;
    public GameObject[] rightGridnon;

    public List<FriendTogetherItem> currentList = new List<FriendTogetherItem>();
    public static List<string> sendList;

    public static string tempSendFid;
    public static bool all = false;

    // Use this for initialization
    void Start()
    {
        instance = this;
        all = false;
        sendList = new List<string>();
        init();
    }

    // Update is called once per frame
    void Update()
    {
        //rightGrid.repositionNow = true;
        //leftGrid.repositionNow = true;
    }

    public void init()
    {
        tempSendFid = null;
        for(int i = 0 ; i < 4; i++)
        {
            leftItem[i].gameObject.SetActive(false);
            rightItem[i].gameObject.SetActive(false);
        }

        string friendlist = "";
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

        usingAppFriends = new List<Dictionary<string, string>>(CMainData.appFriends);
        sendList.Clear();

        JsonData resultData = result["result"];
        {
            int i = 0;
            foreach (Dictionary<string, string> temp in CMainData.appFriends)
            {
                Debug.Log(resultData[i].ToString());
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

        //cleanData();


        if (usingAppFriends.Count == 0)
        {
            Debug.Log("FriendTogether, usingAppFriends count is 0.");

            for (int i = 0; i < CMainData.appFriends.Count && i < 8; i++)
            {
                if (i % 2 == 0)
                {
                    leftGridnon[i / 2].SetActive(false);
                }
                else
                {
                    rightGridnon[i / 2].SetActive(false);
                }

                if (i % 2 == 0)
                {
                    leftItem[i / 2].gameObject.SetActive(true);
                    leftItem[i / 2].SetFriendTogetherContext(CMainData.appFriends[i]["name"].ToString(),
                        CMainData.appFriends[i]["fid"].ToString());
                    leftItem[i / 2].SetNonClick();

                    currentList.Add(leftItem[i / 2]);
                    sendList.Add(CMainData.appFriends[i]["fid"].ToString());
                }
                else
                {
                    rightItem[i / 2].gameObject.SetActive(true);
                    rightItem[i / 2].SetFriendTogetherContext(CMainData.appFriends[i]["name"].ToString(),
                        CMainData.appFriends[i]["fid"].ToString());
                    rightItem[i / 2].SetNonClick();

                    currentList.Add(rightItem[i / 2]);
                    sendList.Add(CMainData.appFriends[i]["fid"].ToString());
                }
            }
        }
        else
        {
            for (int i = 0; i < usingAppFriends.Count && i < 8; i++)
            {
                if (i % 2 == 0)
                {
                    leftGridnon[i / 2].SetActive(false);
                }
                else
                {
                    rightGridnon[i / 2].SetActive(false);
                }

                if (i % 2 == 0)
                {
                    /*
                    GameObject obj = Instantiate(friendTogetherPref, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                    obj.transform.parent = leftGrid.transform;
                    obj.transform.localScale = new Vector3(1f, 1f, 1f);
                    obj.name = usingAppFriends[i]["fid"].ToString();

                    currentList.Add((FriendTogetherItem)obj.GetComponent("FriendTogetherItem"));

                    ((FriendTogetherItem)obj.GetComponent("FriendTogetherItem")).SetFriendTogetherContext(usingAppFriends[i]["name"].ToString(),
                        usingAppFriends[i]["fid"].ToString());
                    */
                    leftItem[i / 2].gameObject.SetActive(true);
                    leftItem[i / 2].SetFriendTogetherContext(usingAppFriends[i]["name"].ToString(),
                        usingAppFriends[i]["fid"].ToString());

                    currentList.Add(leftItem[i / 2]);
                }
                else
                {
                    /*
                    GameObject obj = Instantiate(friendTogetherPref, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                    obj.transform.parent = rightGrid.transform;
                    obj.transform.localScale = new Vector3(1f, 1f, 1f);
                    obj.name = usingAppFriends[i]["fid"].ToString();

                    currentList.Add((FriendTogetherItem)obj.GetComponent("FriendTogetherItem"));

                    ((FriendTogetherItem)obj.GetComponent("FriendTogetherItem")).SetFriendTogetherContext(usingAppFriends[i]["name"].ToString(),
                        usingAppFriends[i]["fid"].ToString());
                    */
                    rightItem[i / 2].gameObject.SetActive(true);
                    rightItem[i / 2].SetFriendTogetherContext(usingAppFriends[i]["name"].ToString(),
                        usingAppFriends[i]["fid"].ToString());

                    currentList.Add(rightItem[i / 2]);
                }
            }

            /*
            if (usingAppFriends.Count % 2 == 0 && usingAppFriends.Count > 6)
            {
                rightGridnon[3].SetActive(false);
            }
            */
            /*
            leftGrid.repositionNow = true;
            rightGrid.repositionNow = true;
            */
        }

        return;
    }

    /*
    public void cleanData()
    {
        for (int i = 0; i < 4; i++)
        {
            rightGridnon[i].SetActive(true);
            leftGridnon[i].SetActive(true);
        }
        
        foreach (FriendTogetherItem temp in currentList)
        {
            temp.gameObject.transform.parent = this.gameObject.transform.parent;
            Destroy(temp.gameObject);
            Destroy(temp);
        }

        currentList.Clear();

        rightGrid.repositionNow = true;
        leftGrid.repositionNow = true;
    }
    */

    private static FriendTogetherItem tempHelpObject = null;

    public void HelpMessage(string fid, FriendTogetherItem helpObject)
    {
        SceneTitle.ButtonClickSound();

        tempSendFid = fid;
        tempHelpObject = helpObject;

        all = false;

        try
        {
            FaceBook.CallAppRequestAsDirectRequestMessage("Help You!", CMainData.Username + " " + Localization.Localize("3018"), fid, Callback);//" 님이 당신에게 주사위를 선물했습니다.", fid);
            //status = "Direct Request called";
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        /*if (Title_ServerConnection.g_instance != null)
        {
            sendList.Add(fid);
            Title_ServerConnection.g_instance.SendMessage(fid, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1);
        }*/
        /*
        if (ServerConnection.g_instance != null)
        {
            sendList.Add(fid);
            ServerConnection.g_instance.SendMessage(fid, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1);
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
                if (ServerConnection.g_instance != null && tempSendFid != null)
                {
                    sendList.Add(tempSendFid);
                    ServerConnection.g_instance.SendMessage(tempSendFid, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1);

                    if (all)
                    {
                        if (FriendTogether.instance != null)
                        {
                            FriendTogether.instance.gameObject.SetActive(false);

                            if (SceneTitle.instance != null)
                            {
                                SceneTitle.instance.changeIsMain(true);
                            }
                        }
                    }
                    else
                    {
                        tempHelpObject.SetNonClick();
                    }
                }
            }
        }
    }

    public void SendMessageAll()
    {
        SceneTitle.ButtonClickSound();
        // Server 전송

        string receiverFidList = "";

        foreach (FriendTogetherItem temp in currentList)
        {
            bool check = false;
            foreach (string t in sendList)
            {
                if(t.Equals(temp.Fid))
                {
                    check = true;
                    break;
                }
            }

            if(!check)
                receiverFidList += temp.Fid + ",";
        }

        if (!receiverFidList.Equals(""))
        {
            receiverFidList = receiverFidList.Substring(0, receiverFidList.Length - 1);
            //Debug.Log("receiver list is " + receiverFidList);

            /*if (Title_ServerConnection.g_instance != null)
            {
                Title_ServerConnection.g_instance.SendMessage(receiverFidList, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1);
            }*/

            tempSendFid = receiverFidList;
            all = true;

            try
            {
                FaceBook.CallAppRequestAsDirectRequestMessage("Help You!", CMainData.Username + " " + Localization.Localize("3018"), receiverFidList, Callback);//+ " 님이 당신에게 주사위를 선물했습니다.", receiverFidList);
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

        //cleanData();
    }
}
