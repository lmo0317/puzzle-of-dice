using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

using CallBackMethod;

public class FriendDiceRequest : MonoBehaviour
{
    public GameObject frienddicePref;
    //public GameObject loading;

    public UIGrid leftGrid;
    public UIGrid rightGrid;

    public UISprite allFriendBtnActiveBack;
    public UISprite allFriendBtnBack;
    public UISprite vanishFriendBtnActiveBack;
    public UISprite vanishFriendBtnBack;

    public List<RequestFriendItem> requestlist = new List<RequestFriendItem>();

    public UIToggle allselect;

    List<Dictionary<string, string>> usingAppFriends = null;
    List<object> usingFriends = null;

    public static FriendDiceRequest g_instance;
    public static string tempFid = null;

    // Use this for initialization
    void Start()
    {
        tempFid = null;
        g_instance = this;
        init();
    }

    public void success(JsonData result)
    {
        if (usingAppFriends != null)
        {
            usingAppFriends.Clear();
        }
        if (usingFriends != null)
        {
            usingFriends.Clear();
        }

        usingAppFriends = new List<Dictionary<string, string>>(CMainData.appFriends);
        usingFriends = new List<object>(FaceBook.friends);

        JsonData resultData = result["result"];

        int i = 0;
        foreach (Dictionary<string, object> temp in FaceBook.friends)
        {
            if (resultData[i].ToString().Equals("0"))
            {
                /*
                foreach (Dictionary<string, string> apptemp in usingAppFriends)
                {
                    if (((string)temp["id"]).Equals(apptemp["fid"]))
                    {
                        usingAppFriends.Remove(apptemp);
                        break;
                    }
                }
                */

                usingFriends.Remove(temp);
            }
            i++;
        }

        foreach (Dictionary<string, string> temp in CMainData.appFriends)
        {
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

        CleanData();
        AllFriendInit();
        return;
    }

    // Update is called once per frame
    void Update()
    {
        leftGrid.repositionNow = true;
        rightGrid.repositionNow = true;
    }

    public void init()
    {
        string friendlist = "";

        if (FaceBook.friends != null)
        {
            foreach (Dictionary<string, object> temp in FaceBook.friends)
            {
                friendlist += (string)temp["id"] + ",";
            }
        }

        if (CMainData.appFriends != null)
        {
            foreach (Dictionary<string, string> temp in CMainData.appFriends)
            {
                friendlist += (string)temp["fid"] + ",";
            }
        }

        if (friendlist.Length >= 1)
        {
            friendlist = friendlist.Substring(0, friendlist.Length - 1);
            /*
            if (Title_ServerConnection.g_instance != null)
            {
                Title_ServerConnection.g_instance.MessageCheck(friendlist, new CallBackClass.MessageCheckCallBack(success));
            }
            */
            if (ServerConnection.g_instance != null)
            {
                ServerConnection.g_instance.MessageCheck(friendlist, new CallBackClass.MessageCheckCallBack(success));
            }
        }
    }

    public void CleanData()
    {
        Debug.Log("call CleanData");
        foreach (RequestFriendItem temp in requestlist)
        {
            temp.gameObject.transform.parent = this.gameObject.transform.parent;
            Destroy(temp.gameObject);
            Destroy(temp);
        }


        leftGrid.repositionNow = true;
        rightGrid.repositionNow = true;

        allselect.value = true;
        requestlist.Clear();
    }

    private void AllFriendInit()
    {
        if(ServerConnection.g_instance != null)
        {
            ServerConnection.g_instance.loading.SetActive(true);
        }
        //loading.SetActive(true);

        allFriendBtnActiveBack.gameObject.SetActive(true);
        vanishFriendBtnActiveBack.gameObject.SetActive(false);

        allFriendBtnBack.gameObject.SetActive(false);
        vanishFriendBtnBack.gameObject.SetActive(true);

        var i = 0;
        foreach (Dictionary<string, object> temp in usingFriends)
        {
            GameObject obj = Instantiate(frienddicePref, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

            if (i % 2 == 0)
            {
                obj.transform.parent = leftGrid.transform;
            }
            else
            {
                obj.transform.parent = rightGrid.transform;
            }

            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            obj.name = (string)temp["id"];
            ((RequestFriendItem)obj.GetComponent("RequestFriendItem")).SetFriendContext((string)temp["last_name"] + (string)temp["first_name"],
                    (string)temp["id"]);

            requestlist.Add(((RequestFriendItem)obj.GetComponent("RequestFriendItem")));
            i++;
        }

        leftGrid.repositionNow = true;
        rightGrid.repositionNow = true;

        if (ServerConnection.g_instance != null)
        {
            ServerConnection.g_instance.loading.SetActive(false);
        }
        //loading.SetActive(false);
    }

    private void VanishFriendInit()
    {
        if (ServerConnection.g_instance != null)
        {
            ServerConnection.g_instance.loading.SetActive(true);
        }
        //loading.SetActive(true);

        vanishFriendBtnActiveBack.gameObject.SetActive(true);
        allFriendBtnActiveBack.gameObject.SetActive(false);

        vanishFriendBtnBack.gameObject.SetActive(false);
        allFriendBtnBack.gameObject.SetActive(true);

        var i = 0;
        foreach (Dictionary<string, string> temp in usingAppFriends)
        {
            GameObject obj = Instantiate(frienddicePref, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

            if (i % 2 == 0)
            {
                obj.transform.parent = leftGrid.transform;
            }
            else
            {
                obj.transform.parent = rightGrid.transform;
            }

            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            obj.name = temp["fid"].ToString();
            ((RequestFriendItem)obj.GetComponent("RequestFriendItem")).SetFriendContext(temp["name"].ToString(),
                    temp["fid"].ToString());

            requestlist.Add(((RequestFriendItem)obj.GetComponent("RequestFriendItem")));
            i++;
        }

        leftGrid.repositionNow = true;
        rightGrid.repositionNow = true;


        if (ServerConnection.g_instance != null)
        {
            ServerConnection.g_instance.loading.SetActive(false);
        }
        //loading.SetActive(false);
    }

    public void VanishButton()
    {
        SceneTitle.ButtonClickSound();
        CleanData();
        VanishFriendInit();
    }

    public void AllButton()
    {
        SceneTitle.ButtonClickSound();
        CleanData();
        AllFriendInit();
    }

    public void toggleSet()
    {
        foreach (RequestFriendItem temp in requestlist)
        {
            temp.btn_Toggle.value = allselect.value;
        }
    }

    public void RequestButton()
    {
        SceneTitle.ButtonClickSound();
        // Server 전송

        string receiverFidList = "";

        foreach (RequestFriendItem temp in requestlist)
        {
            if (temp.btn_Toggle.value)
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

            /*
            if (Title_ServerConnection.g_instance != null)
            {
                Title_ServerConnection.g_instance.SendMessage(receiverFidList, (int)SENDMESSAGEACTIONTYPE.HELP + 1);
            }
            */
            
            try
            {
                string str = Localization.Localize("3016");
                Debug.Log(CMainData.Username + str);
                FaceBook.CallAppRequestAsDirectRequestMessage("Dice Request", CMainData.Username + str, receiverFidList, Callback);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

            /*
            if (ServerConnection.g_instance != null)
            {
                ServerConnection.g_instance.SendMessage(receiverFidList, (int)SENDMESSAGEACTIONTYPE.HELP + 1);
            }
            */
        }
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
                    ServerConnection.g_instance.SendMessage(tempFid, (int)SENDMESSAGEACTIONTYPE.HELP + 1);
                }

                if (FriendDiceRequest.g_instance != null)
                {
                    FriendDiceRequest.g_instance.CleanData();
                    FriendDiceRequest.g_instance.gameObject.SetActive(false);
                    if (SceneTitle.instance != null)
                    {
                        SceneTitle.instance.changeIsMain(true);
                    }
                }
            }
        }
    }


    public void FriendDiceRequestCloseButton()
    {
        if(SceneTitle.instance != null)
        {
            SceneTitle.instance.FriendDiceRequestCloseButton();
        }
        else if(SceneGameOver.g_instance != null)
        {
            SceneGameOver.g_instance.FriendDiceRequestCloseButton();
        }
    }
}
