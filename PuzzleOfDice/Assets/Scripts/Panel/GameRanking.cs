using UnityEngine;
using System.Collections;

public class GameRanking : MonoBehaviour
{
    public static GameRanking instance = null;

    public Ranking_MsgSend[] ranking_msg;

    public GameObject allBtn;

    public static string tempFid = null;

    // Use this for initialization
    void Start()
    {
        tempFid = null;
        instance = this;

        Debug.Log("GameRanking");

        if (SceneGameOver.ResultInstance != null)
        {
            int myRanking = int.Parse(SceneGameOver.ResultInstance["ranking"].ToString());
            
            //1등에서 3등 까지 top 3 표시
            for (int i = 0; i < 3; i++)
            {
                if (!SceneGameOver.ResultInstance[(i) + "_fid"].ToString().Equals("-1"))
                {
                    ranking_msg[i].SetInit(i + 1, SceneGameOver.ResultInstance[(i) + "_fid"].ToString(), SceneGameOver.ResultInstance[(i) + "_name"].ToString(),
                       SceneGameOver.ResultInstance[(i) + "_score"].ToString(), SceneGameOver.ResultInstance[(i) + "_can_send"].ToString(), myRanking == (i+1));
                }
                else
                {
                    ranking_msg[i].gameObject.SetActive(false);
                }
            }

            //내 등수가 4등보다 클경우
            if (myRanking > 4)
            {
                if (myRanking < CMainData.appFriends.Count + 1 && CMainData.appFriends.Count > 1) // 일반적인 경우
                {
                    if (!SceneGameOver.ResultInstance["6_fid"].ToString().Equals("-1"))
                    {
                        ranking_msg[3].SetInit(myRanking - 1, SceneGameOver.ResultInstance["6_fid"].ToString(), SceneGameOver.ResultInstance["6_name"].ToString(),
                            SceneGameOver.ResultInstance["6_score"].ToString(), SceneGameOver.ResultInstance["6_can_send"].ToString(), false);
                    }
                    else
                    {
                        ranking_msg[3].gameObject.SetActive(false);
                    }

                    ranking_msg[4].SetInit(myRanking, FB.UserId, CMainData.Username,
                        CMainData.UserScore.ToString(), "1", true);

                    if (!SceneGameOver.ResultInstance["7_fid"].ToString().Equals("-1"))
                    {
                        ranking_msg[5].SetInit(myRanking + 1, SceneGameOver.ResultInstance["7_fid"].ToString(), SceneGameOver.ResultInstance["7_name"].ToString(),
                            SceneGameOver.ResultInstance["7_score"].ToString(), SceneGameOver.ResultInstance["7_can_send"].ToString(), false);
                    }
                    else
                    {
                        ranking_msg[5].gameObject.SetActive(false);
                    }
                }
                else // 내가 꼴등인 경우
                {
                    if (!SceneGameOver.ResultInstance["7_fid"].ToString().Equals("-1"))
                    {
                        ranking_msg[3].SetInit(myRanking - 2, SceneGameOver.ResultInstance["7_fid"].ToString(), SceneGameOver.ResultInstance["7_name"].ToString(),
                            SceneGameOver.ResultInstance["7_score"].ToString(), SceneGameOver.ResultInstance["7_can_send"].ToString(), false);
                    }
                    else
                    {
                        ranking_msg[3].gameObject.SetActive(false);
                    }

                    if (!SceneGameOver.ResultInstance["6_fid"].ToString().Equals("-1"))
                    {
                        ranking_msg[4].SetInit(myRanking - 1, SceneGameOver.ResultInstance["6_fid"].ToString(), SceneGameOver.ResultInstance["6_name"].ToString(),
                            SceneGameOver.ResultInstance["6__score"].ToString(), SceneGameOver.ResultInstance["6__can_send"].ToString(), false);
                    }
                    else
                    {
                        ranking_msg[4].gameObject.SetActive(false);
                    }

                    ranking_msg[5].SetInit(myRanking, FB.UserId, CMainData.Username,
                        CMainData.UserScore.ToString(), "1", true);
                }
            }
            else // 내가 4등 이하인 경우 ( 1~3 등)
            {
                //하위 UI 4 ~ 6등으로 구성
                for (int i = 3; i < 6; i++)
                {
                    Debug.Log("rank = [ " + SceneGameOver.ResultInstance[(i) + "_fid"].ToString() + " ]"); 
                    if (!SceneGameOver.ResultInstance[(i) + "_fid"].ToString().Equals("-1"))
                    {
                        ranking_msg[i].SetInit(i + 1, SceneGameOver.ResultInstance[(i) + "_fid"].ToString(), SceneGameOver.ResultInstance[(i) + "_name"].ToString(),
                           SceneGameOver.ResultInstance[(i) + "_score"].ToString(), SceneGameOver.ResultInstance[(i) + "_can_send"].ToString(), myRanking == (i + 1));
                    }
                    else
                    {
                        ranking_msg[i].gameObject.SetActive(false);
                    }
                }
            }

            allCheck();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ButtonClickSound()
    {
        SoundManager.g_Instance.PlayEffectSound("menu_click_01");
    }

    public void allMessage()
    {
        ButtonClickSound();

        if (SceneGameOver.g_instance != null)
        {
            if (SceneGameOver.g_instance.buttonClickable)
            {

                if (SceneGameOver.ResultInstance != null)
                {
                    string receiverFidList = "";
                    foreach (Ranking_MsgSend temp in ranking_msg)
                    {
                        if (temp.Sendable && !receiverFidList.Contains(temp.Fid))
                        {
                            receiverFidList += temp.Fid + ",";
                        }
                    }

                    if (!receiverFidList.Equals(""))
                    {
                        receiverFidList = receiverFidList.Substring(0, receiverFidList.Length - 1);
                        //Debug.Log("receiver list is " + receiverFidList);

                        tempFid = receiverFidList;

                        allBtn.SetActive(false);
                        //GameOver_ServerConnection.g_instance.SendMessage(receiverFidList, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1, checkSendMsg);

                        try
                        {
                            FaceBook.CallAppRequestAsDirectRequestMessage("Help You!", CMainData.Username + " " + Localization.Localize("3018"), receiverFidList, Callback);// " 님이 당신에게 주사위를 선물했습니다.", receiverFidList);
                            //status = "Direct Request called";
                        }
                        catch (System.Exception e)
                        {
                            Debug.Log(e.Message);
                        }

                        /*
                        if (ServerConnection.g_instance != null)
                        {
                            ServerConnection.g_instance.SendMessage(receiverFidList, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1, checkSendMsg);
                        }
                        */
                    }
                }
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
                if (ServerConnection.g_instance != null && GameRanking.instance != null)
                {
                    ServerConnection.g_instance.SendMessage(tempFid, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1, GameRanking.instance.checkSendMsg);
                }
            }
        }
    }

    public void sendButton(string fid)
    {
        ButtonClickSound();

        if (SceneGameOver.g_instance != null)
        {
            if (SceneGameOver.g_instance.buttonClickable)
            {
                /*if (GameOver_ServerConnection.g_instance != null)
                {
                    GameOver_ServerConnection.g_instance.SendMessage(fid, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1, checkSendMsg);
                }
                */

                tempFid = fid;

                try
                {
                    FaceBook.CallAppRequestAsDirectRequestMessage("Help You!", CMainData.Username + " " + Localization.Localize("3018"), fid, Callback);// " 님이 당신에게 주사위를 선물했습니다.", fid);
                    //status = "Direct Request called";
                }
                catch (System.Exception e)
                {
                    Debug.Log(e.Message);
                }

                /*
                if (ServerConnection.g_instance != null)
                {
                    ServerConnection.g_instance.SendMessage(fid, (int)SENDMESSAGEACTIONTYPE.NORMAL + 1, checkSendMsg);
                }
                */
            }
        }
    }

    public void checkSendMsg(string fidList)
    {
        string[] fidArray = fidList.Split(',');

        foreach (string fid in fidArray)
        {
            foreach (Ranking_MsgSend temp in ranking_msg)
            {
                if (temp.Fid.Equals(fid))
                    temp.setuneable();
            }
        }

        allCheck();
    }

    public void allCheck()
    {
        bool enable = false;

        foreach (Ranking_MsgSend temp in ranking_msg)
        {
            if (temp.Sendable)
            {
                enable = true;
                break;
            }
        }

        if (enable)
        {
            allBtn.SetActive(true);
        }
        else
        {
            allBtn.SetActive(false);
        }
    }

    public GameObject[] rankingButton;

    public void ButtonPrev(bool prev)
    {
        if (prev)
        {
            foreach (GameObject temp in rankingButton)
            {
                temp.collider.enabled = false;
            }
        }
        else
        {
            foreach (GameObject temp in rankingButton)
            {
                temp.collider.enabled = true;
            }
        }
    }
}