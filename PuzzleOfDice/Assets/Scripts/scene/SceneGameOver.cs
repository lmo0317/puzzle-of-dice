using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

using CallBackMethod;

public class SceneGameOver : MonoBehaviour
{
    public static SceneGameOver g_instance = null;
    public bool buttonClickable = true;
    public static long score = 0;
    public static int[] result_DiceN = {0, 0, 0, 0, 0, 0, 0};
    public static int result_maxChain = 0;
    public static long result_maxChainScore = 0;
    private long prev_score = 0;
    public static JsonData ResultInstance = null;
    //public GameOver_ServerConnection serverConnection;
    public ServerConnection serverConnection;
    private GameObject shopgoldbar;
    public GameObject shopgoldbarprefab;
    private FriendDiceRequest frienddicerequest;
    public GameObject frienddicerequestPrefab;
    public GameObject gameovernormal;
    public GameObject gameoverrenewranking;
    public GameObject gamerenewscore;
    public GameObject[] gameovertypePanel; // 스코어, 랭킹, 노말
    public GameObject gameoverrestartdicenohave;
    public GameObject gameranking;
    public int currentGameOverType = (int)GAMEOVERTYPE.RENEWSCORE;
    public enum GAMEOVERTYPE { RENEWSCORE, RENEWRANKING, NORMAL };
    //public GameObject gameoverachievementunlock;
 
    // Use this for initialization
    void Start()
    {
        //FriendDiceRequest Panel
        GameObject tempFriendDiceR = GameObject.Instantiate(frienddicerequestPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
        tempFriendDiceR.transform.parent = this.transform;
        tempFriendDiceR.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        ((UIPanel)tempFriendDiceR.GetComponent("UIPanel")).depth = 1;
        frienddicerequest = (FriendDiceRequest)tempFriendDiceR.GetComponent("FriendDiceRequest");
        frienddicerequest.gameObject.SetActive(false);

        // ShopGoldBar Panel
        shopgoldbar = GameObject.Instantiate(shopgoldbarprefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
        shopgoldbar.transform.parent = this.transform;
        shopgoldbar.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        ((UIPanel)shopgoldbar.GetComponent("UIPanel")).depth = 1;
        shopgoldbar.SetActive(false);

        g_instance = this;
        buttonClickable = true;
        if (Localization.instance != null)
        {
            // Localization 객체에게 언어를 바꾸라고 합니다.

            Debug.Log("Locale is " + CMainData.Locale);

            if (CMainData.Locale.Contains("ko"))
                Localization.instance.currentLanguage = "korea";
            else if (CMainData.Locale.Contains("ja"))
                Localization.instance.currentLanguage = "japan";
            else
                Localization.instance.currentLanguage = "english";
        }
        
        prev_score = CMainData.UserScore;
        // Check Current State - 점수 갱신, 랭킹 갱신, 일반
        string friendlist = "";
        /*
        foreach (Dictionary<string, object> temp in FaceBook.friends)
        {
            friendlist += (string)temp["id"] + ",";
        }*/

        foreach (Dictionary<string, string> temp in CMainData.appFriends)
        {
            friendlist += (string)temp["fid"] + ",";
        }


        if (friendlist.Length != 0)
        {
            friendlist = friendlist.Substring(0, friendlist.Length - 1);
        }

        if (serverConnection != null)
            serverConnection.SendGameEnd(score.ToString(), friendlist, new CallBackClass.MessageCheckCallBack(success));
    }

    public void success(JsonData result)
    {
        ResultInstance = result;
        Debug.Log("score is " + score.ToString());
        Debug.Log("prev score is " + CMainData.UserScore.ToString());
        if (score > prev_score)
        {
            CMainData.UserScore = score;
        }
        else
        {
            currentGameOverType = (int)GAMEOVERTYPE.NORMAL;
        }
    }

    public void gameEndUIStart(bool serverstate) // From GameEnd_ServerConnection -> Receive Complete.
    {
        if(serverstate)
        {
            // Connection Success
            gameranking.SetActive(true);
            UpdateView();
        }
        else
        {
            // Connection Fail...
            Application.LoadLevel("SceneTitle");
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

    public static void MenuOpenSound()
    {
        SoundManager.g_Instance.PlayEffectSound("menu_open_01");
    }

    public static void MenuCloseSound()
    {
        SoundManager.g_Instance.PlayEffectSound("menu_close_01");
    }

    public bool DiceCheck()
    {
        if (CMainData.Dice_Count > 0)
        {
            //CMainData.Dice_Count--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateView()
    {
        //랭킹 변동 UI 표시 
        if (currentGameOverType == (int)GAMEOVERTYPE.RENEWRANKING)
        {
            //서버에서 랭킹 변동이 없다고 할경우 무시하고 NORMAL로 넘어간다.
            //내아래 유저가 없을경우 넘어간다.
			string temp = ResultInstance["change"].ToString();
            if (!temp.Equals("1") || SceneGameOver.ResultInstance["7_fid"].ToString().Equals("-1"))
            {
                //GAMEOVERTYPE.RENEWRANKING = 1 -> GAMEOVERTYPE.NORMAL = 2
                currentGameOverType += 1;
            }
        }

        Debug.Log("UpdateView : " + currentGameOverType);
        for (int i = 0; i < gameovertypePanel.Length; i++)
        {
            if(i == currentGameOverType)
            {
                gameovertypePanel[i].SetActive(true);

                if (i == (int)GAMEOVERTYPE.RENEWSCORE)
                {
                    SoundManager.g_Instance.PlayBGM("BGM_NewRecord_01", true);
                }
                else if (i == (int)GAMEOVERTYPE.RENEWRANKING)
                {
                    SoundManager.g_Instance.PlayBGM("BGM_Overtake_01", true);
                }
                else if (i == (int)GAMEOVERTYPE.NORMAL)
                {
                    SoundManager.g_Instance.PlayBGM("BGM_main_01", true);
                }

                MenuOpenSound();
            }
            else
            {
                gameovertypePanel[i].SetActive(false);
            }
        }
    }

    public void ShopGoldBarOpenButton()
    {
        ButtonClickSound();
        gameoverrestartdicenohave.SetActive(false);
        shopgoldbar.SetActive(true);
        MenuOpenSound();
    }

    public void ShopGoldBarCloseButton()
    {
        ButtonClickSound();
        gameoverrestartdicenohave.SetActive(true);
        shopgoldbar.SetActive(false);
        MenuCloseSound();
    }

    public void FriendDiceRequestOpenButton()
    {
        ButtonClickSound();
        gameoverrestartdicenohave.SetActive(false);
        frienddicerequest.gameObject.SetActive(true);
        MenuOpenSound();
        frienddicerequest.init();
    }

    public void FriendDiceRequestCloseButton()
    {
        ButtonClickSound();
        frienddicerequest.gameObject.SetActive(false);
        MenuCloseSound();
        gameoverrestartdicenohave.SetActive(true);
    }

    public void RenewScoreCloseButton()
    {
        // GAMEOVERTYPE.RENEWSCORE == 0 -> GAMEOVERTYPE.RENEWRANKING == 1
        currentGameOverType += 1;
        ButtonClickSound();
        UpdateView();
    }

    public void RenewRankingCloseButton()
    {
        // GAMEOVERTYPE.RENEWRANKING == 1 -> GAMEOVERTYPE.NORMAL == 2       
        currentGameOverType += 1;
        ButtonClickSound();
        UpdateView();
    }

    public void NormalCloseButton()
    {
        ButtonClickSound();
        MenuCloseSound();
        Application.LoadLevel("SceneTitle");
    }

    public void NormalRestartButton()
    {
        ButtonClickSound();

        if (DiceCheck())
        {
            /*
            if(GameOver_ServerConnection.g_instance != null)
                GameOver_ServerConnection.g_instance.GameStart();
            */

            if (serverConnection != null)
                serverConnection.GameStart();

            /*
            CMainData.setGameMode(GameData.GAME_MODE_BLITZ); //set game mode
            CMainData.setGameState(GameData.GAME_STATE_READY); //set game state
            Application.LoadLevel("GameScene"); //load game scene
            */
        }
        else
        {
            MenuCloseSound();
            gameovernormal.SetActive(false);
            gameranking.SetActive(false);
            gameoverrestartdicenohave.SetActive(true);
            MenuOpenSound();
        }
    }

    public void BuyDice()
    {
        ButtonClickSound();

        if (CMainData.Gold < CDefine.DICE_PRICE)
        {
            gameovernormal.SetActive(false);
            gameranking.SetActive(false);
            shopgoldbar.SetActive(true);
            MenuOpenSound();
        }
        else
        {
            /*
            if(GameOver_ServerConnection.g_instance != null)
                GameOver_ServerConnection.g_instance.SendBuyDice(new CallBackClass.MessageCheckCallBack(BuyDiceSuccess));
            */

            if (serverConnection != null)
                serverConnection.SendBuyDice(new CallBackClass.MessageCheckCallBack(BuyDiceSuccess));

            Debug.Log("Buy Dice");
        }
    }

    public void BuyDiceSuccess(JsonData result)
    {
        CMainData.Gold = int.Parse(result["gold"].ToString());
        CMainData.Dice_Count = int.Parse(result["dice"].ToString());
        CMainData.Dice_Time = int.Parse(result["dicetime"].ToString());
        Debug.Log("Buy Dice Success " + CMainData.Gold + "," + CMainData.Dice_Count + "," + CMainData.Dice_Time);
        if (CMainData.Dice_Count < 1)
        {
            shopgoldbar.SetActive(true);
            MenuOpenSound();
        }
    }

    public void GameOverRestartDicenohaveCloseButton()
    {
        ButtonClickSound();
        Application.LoadLevel("SceneTitle");
    }
}
