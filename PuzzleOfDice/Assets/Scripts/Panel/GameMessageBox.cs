using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMessageBox : MonoBehaviour
{
    public static GameMessageBox instance = null;

    public UILabel label_message_num;
    public UILabel label_title;

    public GameObject messagePref;

    public UIGrid messageGrid;

    // Use this for initialization
    void Start()
    {
        tempMessageObj = null;
        instance = this;

        if (label_message_num != null)
        {
            label_message_num.text = StringData.getString(StringData.label_message_num_key).Replace("%s", CMainData.message.Count.ToString());
        }

        foreach (Message temp in CMainData.message)
        {
            if (temp.Obj == null)
            {
                GameObject obj = Instantiate(messagePref, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                obj.transform.parent = messageGrid.transform;
                obj.transform.localScale = new Vector3(1f, 1f, 1f);
                ((MessageItem)obj.GetComponent("MessageItem")).SetMessageContext(temp.Type, temp.Sendder, temp.Fid);

                temp.Obj = obj;
            }
        }

        messageGrid.repositionNow = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void acceptMessage(string fid, int type)
    {
        SceneTitle.ButtonClickSound();

        foreach (Message temp in CMainData.message)
        {
            if (temp.Fid.Equals(fid) && temp.Type == type)
            {

                //Server로 데이터 전송

                /*if (Title_ServerConnection.g_instance != null)
                {
                    Title_ServerConnection.g_instance.ReceiveMessage(temp, (int)RECEIVEMESSAGEACTIONTYPE.ACCEPT);

                    CMainData.message.Remove(temp);
                    temp.Obj.transform.parent = this.gameObject.transform.parent;
                    Destroy(temp.Obj);
                }*/

                if (type != 3)
                {
                    if (ServerConnection.g_instance != null)
                    {
                        ServerConnection.g_instance.ReceiveMessage(temp, (int)RECEIVEMESSAGEACTIONTYPE.ACCEPT);

                        CMainData.message.Remove(temp);
                        temp.Obj.transform.parent = this.gameObject.transform.parent;
                        Destroy(temp.Obj);
                    }
                }
                else
                {
                    tempMessageObj = temp;
                    try
                    {
                        FaceBook.CallAppRequestAsDirectRequestMessage("Help You!", CMainData.Username + " " + Localization.Localize("3018"), temp.Fid, Callback);// + " 님이 당신에게 주사위를 선물했습니다.", receiverFidList);
                        //status = "Direct Request called";
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }

                break;
            }
        }
        if (label_message_num != null)
        {
            label_message_num.text = StringData.getString(StringData.label_message_num_key).Replace("%s", CMainData.message.Count.ToString());
        }
        messageGrid.repositionNow = true;
    }

    private static Message tempMessageObj = null;

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
                if (ServerConnection.g_instance != null && GameMessageBox.instance != null)
                {
                    ServerConnection.g_instance.ReceiveMessage(tempMessageObj, (int)RECEIVEMESSAGEACTIONTYPE.ACCEPT);

                    CMainData.message.Remove(tempMessageObj);
                    tempMessageObj.Obj.transform.parent = GameMessageBox.instance.gameObject.transform.parent;
                    Destroy(tempMessageObj.Obj);

                    if (GameMessageBox.instance.label_message_num != null)
                    {
                        GameMessageBox.instance.label_message_num.text = StringData.getString(StringData.label_message_num_key).Replace("%s", CMainData.message.Count.ToString());
                    }
                    GameMessageBox.instance.messageGrid.repositionNow = true;
                }
            }
        }
    }


    public void closeMessage(string fid, int type)
    {
        SceneTitle.ButtonClickSound();

        foreach (Message temp in CMainData.message)
        {
            if (temp.Fid.Equals(fid) && temp.Type == type)
            {
                //Server로 데이터 전송
                /*if (Title_ServerConnection.g_instance != null)
                {
                    Title_ServerConnection.g_instance.ReceiveMessage(temp, (int)RECEIVEMESSAGEACTIONTYPE.CANCLE);

                    CMainData.message.Remove(temp);
                    temp.Obj.transform.parent = this.gameObject.transform.parent;
                    Destroy(temp.Obj);
                }*/
                if (ServerConnection.g_instance != null)
                {
                    ServerConnection.g_instance.ReceiveMessage(temp, (int)RECEIVEMESSAGEACTIONTYPE.CANCLE);

                    CMainData.message.Remove(temp);
                    temp.Obj.transform.parent = this.gameObject.transform.parent;
                    Destroy(temp.Obj);
                }
                break;
            }
        }

        if (label_message_num != null)
        {
            label_message_num.text = StringData.getString(StringData.label_message_num_key).Replace("%s", CMainData.message.Count.ToString());
        }
        messageGrid.repositionNow = true;
    }
}
