using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CallBackMethod;
using LitJson;

public class FriendInvite : MonoBehaviour
{
    public GameObject frienddicePref;
    public GameObject loading;

    public UIGrid leftGrid;
    public UIGrid rightGrid;

    List<object> usingFriends = null;
    public List<RequestFriendItem> requestlist = new List<RequestFriendItem>();

    public UIToggle allselect;

    public static FriendInvite g_instance;

    // Use this for initialization
    void Start()
    {
        g_instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        rightGrid.repositionNow = true;
        leftGrid.repositionNow = true;
    }

    public void init()
    {
        loading.SetActive(true);

        string friendlist = "";

        if (FaceBook.friends != null)
        {
            foreach (Dictionary<string, object> temp in FaceBook.friends)
            {
                friendlist += (string)temp["id"] + ",";
            }

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

    public void toggleSet()
    {
        foreach (RequestFriendItem temp in requestlist)
        {
            temp.btn_Toggle.value = allselect.value;
        }
    }

    public void success(JsonData result)
    {
        if (usingFriends != null)
        {
            usingFriends.Clear();
        }

        usingFriends = new List<object>(FaceBook.friends);

        JsonData resultData = result["result"];
        int z = 0;
        foreach (Dictionary<string, object> temp in FaceBook.friends)
        {
            //Debug.Log(resultData[z].ToString());
            if (resultData[z].ToString().Equals("0"))
            {
                usingFriends.Remove(temp);
            }
            z++;
        }

        CleanData();

        var i = 0;

        foreach (Dictionary<string, object> temp in usingFriends)
        {
            bool check = false;
            foreach (Dictionary<string, string> apptemp in CMainData.appFriends)
            {
                if (((string)temp["id"]).Equals(apptemp["fid"]))
                {
                    check = true;
                    break;
                }
            }

            if (check)
                continue;

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
            obj.name = (string)temp["id"].ToString();

            ((RequestFriendItem)obj.GetComponent("RequestFriendItem")).SetFriendContext((string)temp["last_name"] + (string)temp["first_name"],
                    (string)temp["id"]);

            requestlist.Add(((RequestFriendItem)obj.GetComponent("RequestFriendItem")));
            i++;
        }

        leftGrid.repositionNow = true;
        rightGrid.repositionNow = true;
        loading.SetActive(false);
        return;
    }


    public void CleanData()
    {
        foreach (RequestFriendItem temp in requestlist)
        {
            temp.gameObject.transform.parent = this.gameObject.transform.parent;
            Destroy(temp.gameObject);
            Destroy(temp);
        }

        allselect.value = true;
        requestlist.Clear();
    }

    public void RequestButton()
    {
        SceneTitle.ButtonClickSound();
        string receiverFidList = "";
        // Server 전송

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

            try
            {
                string str = Localization.Localize("3017");
                FaceBook.CallAppRequestAsDirectRequestMessage("Invite You", CMainData.Username + str, receiverFidList, Callback);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
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
                if (FriendInvite.g_instance != null)
                {
                    FriendInvite.g_instance.gameObject.SetActive(false);
                    if (SceneTitle.instance != null)
                    {
                        SceneTitle.instance.changeIsMain(true);
                    }
                }
            }
        }
    }
}
