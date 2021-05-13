using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CallBackMethod;
using LitJson;

public class SceneTitle : MonoBehaviour
{
    public static SceneTitle instance = null;

    /* Title MainState */
    public bool isMain = true;

    /* Panel */
    public GameObject shop_cat;
    private GameObject shop_goldbar;
    public GameObject shop_goldbar_prefab;
    public GameObject achieve_list;
    public GameObject like_us;
    public GameObject tutorial;
    public GameObject shop_dice;
    public GameObject gamelogin_help;
    //public GameObject friend_dice_request;

    private FriendDiceRequest friend_dice_request;
    public GameObject friendDiceRequestPrefab;

    public GameObject game_message_box;
    public FriendTogether friend_together;
    //public GameObject friend_invite;
    public FriendInvite friend_invite;

    //private Title_ServerConnection serverConnection;

    // Music, Sound Button
    public UISprite musicButton;
    public UISprite soundButton;

    public static bool first_loading;

    // Use this for initialization
    void Start()
    {
        // FriendDiceRequest Panel
        GameObject tempFriendDiceR = GameObject.Instantiate(friendDiceRequestPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
        tempFriendDiceR.transform.parent = this.transform;
        tempFriendDiceR.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        ((UIPanel)tempFriendDiceR.GetComponent("UIPanel")).depth = 1;
        friend_dice_request = (FriendDiceRequest)tempFriendDiceR.GetComponent("FriendDiceRequest");
        friend_dice_request.gameObject.SetActive(false);

        // ShopGoldBar Panel
        shop_goldbar = GameObject.Instantiate(shop_goldbar_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
        shop_goldbar.transform.parent = this.transform;
        shop_goldbar.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        ((UIPanel)shop_goldbar.GetComponent("UIPanel")).depth = 2;
        shop_goldbar.SetActive(false);

        instance = this;

        changeIsMain(true);
        //isMain = true;

        if (Localization.instance != null)
        {
            // Localization 객체에게 언어를 바꾸라고 합니다.

            Debug.Log("Locale is " + CMainData.Locale);

            if(CMainData.Locale.Contains("ko"))
                Localization.instance.currentLanguage = "korea";
            else if(CMainData.Locale.Contains("ja"))
                Localization.instance.currentLanguage = "japan";
            else
                Localization.instance.currentLanguage = "english";
        }

        if (musicButton != null)
        {
            if (CMainData.Music)
            {
                musicButton.spriteName = "MS_menu_btn_music_on";
                // musicButton.mainTexture = main_music_on;
            }
            else
            {
                musicButton.spriteName = "MS_menu_btn_music_off";
                //musicButton.mainTexture = main_pause;
            }
        }

        if (soundButton != null)
        {
            if (CMainData.Sound)
            {
                soundButton.spriteName = "MS_menu_btn_sound_on";
                //soundButton.mainTexture = main_sound_on;
            }
            else
            {
                soundButton.spriteName = "MS_menu_btn_sound_off";
                //soundButton.mainTexture = main_sound_off;
            }
        }

        if (!CMainData.Tutorial)
        {
            TutorialOpen();
        }
        else
        {
            if (first_loading)
            {
                Debug.Log("FirstLoading");
                changeIsMain(false);
                //isMain = false;
                gamelogin_help.SetActive(true);
                first_loading = false;
            }
        }

        FaceBook.LoadFriendImages();

        //tutorial.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (CMainData.Dice_Time != -1 && CMainData.Dice_Count < 5)
        {
            CMainData.Dice_Time += Time.deltaTime;
            if (CMainData.Dice_Time > CDefine.DICE_RECOVERYTIME)
            {
                /*if (Title_ServerConnection.g_instance != null)
                    Title_ServerConnection.g_instance.SendDiceIncrease();*/
                if (ServerConnection.g_instance != null)
                    ServerConnection.g_instance.SendDiceIncrease();
                //CMainData.Dice_Time = 0.0f;
                //CMainData.Dice_Count += 1;
            }
        }
    }

    public void MusicButton()
    {
        Debug.Log("Onclick Music Button.");

        if (isMain)
        {
            CMainData.Music = !CMainData.Music;

            if (CMainData.Music)
            {
                musicButton.spriteName = "MS_menu_btn_music_on";
                SoundManager.g_Instance.ChangeMusicState();
            }
            else
            {
                musicButton.spriteName = "MS_menu_btn_music_off";
                SoundManager.g_Instance.ChangeMusicState();
            }
        }
    }

    public void SoundButton()
    {
        Debug.Log("Onclick Sound Button.");
        if (isMain)
        {
            CMainData.Sound = !CMainData.Sound;

            if (CMainData.Sound)
            {
                soundButton.spriteName = "MS_menu_btn_sound_on";
                //soundButton.mainTexture = main_sound_on;
            }
            else
            {
                soundButton.spriteName = "MS_menu_btn_sound_off";
                //soundButton.mainTexture = main_sound_off;
            }
        }
    }

    // defer
    public void ShopButton()
    {
        Debug.Log("Scene Onclick Shop Button.");
        ButtonClickSound();
        //shop_cat.SetActive(true);

        //GameObject.FindGameObjectWithTag("Shop_Cat").SetActive(true);
        //obj_shop.SetActive(true);
    }

    // defer
    public void ShopCloseButton()
    {
        Debug.Log("ShopCloseButton");
        ButtonClickSound();
        //shop_cat.SetActive(false);
    }

    public void ShopGoldBarButton()
    {
        Debug.Log("ShopGoldBarButton");
        if (isMain)
        {
            ButtonClickSound();
            changeIsMain(false);
            //isMain = false;
            MenuOpenSound();
            shop_goldbar.SetActive(true);
        }
    }

    public void ShopGoldBarButton_Power()
    {
        Debug.Log("ShopGoldBarButtonPower");
        ButtonClickSound();
        changeIsMain(false);
        //isMain = false;
        shop_dice.SetActive(false);
        MenuOpenSound();
        shop_goldbar.SetActive(true);
    }

    public void ShopGoldBarCloseButton()
    {
        Debug.Log("ShopGoldBarCloseButton");
        ButtonClickSound();
        changeIsMain(true);
        //isMain = true;
        shop_goldbar.SetActive(false);
        MenuCloseSound();
    }

    public static void ButtonClickSound()
    {
        if (SoundManager.g_Instance != null)
            SoundManager.g_Instance.PlayEffectSound("menu_click_01");
    }

    public static void MenuOpenSound()
    {
        if(SoundManager.g_Instance != null)
            SoundManager.g_Instance.PlayEffectSound("menu_open_01");
    }

    public static void MenuCloseSound()
    {
        if (SoundManager.g_Instance != null)
            SoundManager.g_Instance.PlayEffectSound("menu_close_01");
    }

    public void StartGameButton()
    {
        Debug.Log("StartGameButton");

        if (isMain)
        {
            Debug.Log("IsMain");

            ButtonClickSound();
            
            if (DiceCheck())
            {
                /*if(Title_ServerConnection.g_instance != null)
                    Title_ServerConnection.g_instance.GameStart();*/
                if (ServerConnection.g_instance != null)
                    ServerConnection.g_instance.GameStart();
            }
            else
            {
                ShopDiceOpenButton();
                //shop_dice.SetActive(true);
            }
        }
    }

    public void TutorialOpen()
    {
        changeIsMain(false);
        //isMain = false;
        MenuOpenSound();
        tutorial.SetActive(true);
    }

    public void TutorialOpenButton()
    {
        Debug.Log("TutorialOpenButton");

        if (isMain)
        {
            changeIsMain(false);
            //isMain = false;
            ButtonClickSound();
            tutorial.SetActive(true);
            MenuOpenSound();
        }
    }

    public void TutorialCloseButton()
    {
        Debug.Log("TutorialCloseButton");

        changeIsMain(true);
        //isMain = true;
        ButtonClickSound();
        tutorial.SetActive(false);

        MenuCloseSound();

        if (first_loading)
        {
            changeIsMain(false);
            //isMain = false;
            gamelogin_help.SetActive(true);
            MenuOpenSound();
            first_loading = false;
        }
    }

    public void StartTutorialButton()
    {
        Debug.Log("StartTutorialButton");

        ButtonClickSound();

        /*if (DiceCheck())
        {*/


        CMainData.setGameMode(GameData.GAME_MODE_TUTORIAL); //set game mode
        CMainData.setGameState(GameData.GAME_STATE_READY); //set game state
        Application.LoadLevel("GameScene"); //load game scene


        /*}
        else
        {
            shop_dice.SetActive(true);
        }*/
    }

    public bool DiceCheck()
    {
        if (CMainData.Dice_Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShopDiceOpenButton()
    {
        Debug.Log("ShopDiceOpenButton");

        if (isMain)
        {
            ButtonClickSound();
            changeIsMain(false);
            //isMain = false;
            shop_dice.SetActive(true);
            MenuOpenSound();
        }
    }

    public void ShopDiceCloseButton()
    {
        Debug.Log("ShopDiceCloseButton");

        ButtonClickSound();
        changeIsMain(true);
        //isMain = true;
        shop_dice.SetActive(false);
        MenuCloseSound();
    }

    public void FriendDiceRequestOpenButton()
    {
        Debug.Log("FriendDiceRequestOpenButton");

        ButtonClickSound();
        changeIsMain(false);
        //isMain = false;
        shop_dice.SetActive(false);
        friend_dice_request.gameObject.SetActive(true);
        MenuOpenSound();
        friend_dice_request.init();
    }

    public void FriendDiceRequestCloseButton()
    {
        Debug.Log("FriendDiceRequestCloseButton");

        ButtonClickSound();
        changeIsMain(true);
        //isMain = true;
        friend_dice_request.CleanData();
        friend_dice_request.gameObject.SetActive(false);
        MenuCloseSound();
    }

    public void GameMessageBoxOpenButton()
    {
        Debug.Log("GameMessageBoxOpenButton");

        if (isMain)
        {
            ButtonClickSound();
            changeIsMain(false);
            //isMain = false;
            game_message_box.SetActive(true);
            MenuOpenSound();
        }
    }

    public void GameMessageBoxCloseButton()
    {
        Debug.Log("GameMessageBoxCloseButton");

        ButtonClickSound();
        changeIsMain(true);
        //isMain = true;
        game_message_box.SetActive(false);
        MenuCloseSound();
    }

    // defer
    public void AchieveOpenButton()
    {
        Debug.Log("AchieveOpenButton");
        ButtonClickSound();
        //achieve_list.SetActive(true);
        ShareButton();
        MenuOpenSound();
    }

    // TESTCODE!!!
    public void ShareButton()
    {
        try
        {
            CallFBFeed();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void CallFBFeed()
    {
        string FeedToId = "";
        string FeedLink = "https://apps.facebook.com/vanishdiceworld/";
        string FeedLinkName = "Facebook!";
        string FeedLinkCaption = "caption";
        string FeedLinkDescription = "description";
        string FeedPicture = "https://fbcdn-photos-b-a.akamaihd.net/hphotos-ak-prn1/t39.2081/p128x128/851567_503332359777519_1897376422_n.png";
        string FeedMediaSource = "";
        string FeedActionName = "";
        string FeedActionLink = "";
        string FeedReference = "";
        Dictionary<string, string[]> feedProperties = null;

        FB.Feed(
            toId: FeedToId,
            link: FeedLink,
            linkName: FeedLinkName,
            linkCaption: FeedLinkCaption,
            linkDescription: FeedLinkDescription,
            picture: FeedPicture,
            mediaSource: FeedMediaSource,
            actionName: FeedActionName,
            actionLink: FeedActionLink,
            reference: FeedReference,
            properties: feedProperties,
            callback: FeedCallback
        );
    }

    void FeedCallback(FBResult result)
    {
        if (result.Error != null)
            Debug.Log("Error Response:\n" + result.Error);
        else
        {
            Debug.Log("Success Response:\n");
        }
    }

    // defer
    public void AchieveCloseButton()
    {
        Debug.Log("AchieveCloseButton");
        ButtonClickSound();
        achieve_list.SetActive(false);
        MenuCloseSound();
    }

    public void LikeUsOpenButton()
    {
        Debug.Log("LikeUsOpenButton");
        if (isMain)
        {
            ButtonClickSound();
            MenuOpenSound();
            Application.ExternalEval("window.open('http://thepipercat.com/Vanish.html','Puzzle of Dice','width=590,height=480,toolbar=no,menubar=no,location=no,scrollbars=no,status=no,channelmode=no,left=100,top=100')");
            //Application.ExternalEval("window.open('http://www.facebook.com/plugins/likebox.php?href=https%3A%2F%2Fwww.facebook.com%2F1477936459088484&width&height=290&colorscheme=light&show_faces=true&header=true&stream=false&show_border=true&appId=214159675440557','Puzzle of Dice','height=300')");
            //Application.OpenURL("http://www.facebook.com/plugins/likebox.php?href=https%3A%2F%2Fwww.facebook.com%2F1477936459088484&width&height=290&colorscheme=light&show_faces=true&header=true&stream=false&show_border=true&appId=214159675440557");

            //isMain = false;
            //like_us.SetActive(true);
        }
    }

    public void LikeUsCloseButton()
    {
        Debug.Log("LikeUsCloseButton");
        ButtonClickSound();
        changeIsMain(true);
        //isMain = true;
        like_us.SetActive(false);
        MenuCloseSound();
    }

    public void FriendTogetherOpenButton()
    {
        if (isMain)
        {
            ButtonClickSound();
            changeIsMain(false);
            //isMain = false;
            friend_together.gameObject.SetActive(true);
            MenuOpenSound();
            //friend_together.init();
        }
    }

    public void FriendTogetherCloseButton()
    {
        ButtonClickSound();
        changeIsMain(true);
        //isMain = true;
        //friend_together.cleanData();
        friend_together.gameObject.SetActive(false);
        MenuCloseSound();
    }

    public void FriendInviteOpenButton()
    {
        ButtonClickSound();
        changeIsMain(false);
        //isMain = false;

        friend_together.gameObject.SetActive(false);

        friend_invite.gameObject.SetActive(true);
        MenuOpenSound();
        friend_invite.init();
    }

    public void FriendInviteCloseButton()
    {
        ButtonClickSound();
        changeIsMain(true);
        //isMain = true;
        friend_invite.CleanData();
        friend_invite.gameObject.SetActive(false);
        MenuCloseSound();
    }

    public void GameLoginHelpClose()
    {
        ButtonClickSound();
        changeIsMain(true);
        //isMain = true;
        gamelogin_help.SetActive(false);
        MenuCloseSound();
    }

    public void BuyDice()
    {
        if (CMainData.Dice_Count > 4)
        {

        }
        else if (CMainData.Gold < CDefine.DICE_PRICE)
        {
            changeIsMain(false);
            //isMain = false;
            shop_dice.SetActive(false);
            shop_goldbar.SetActive(true);
        }
        else
        {
            /*if(Title_ServerConnection.g_instance != null)
                Title_ServerConnection.g_instance.SendBuyDice(new CallBackClass.MessageCheckCallBack(BuyDiceSuccess));*/
            if (ServerConnection.g_instance != null)
                ServerConnection.g_instance.SendBuyDice(new CallBackClass.MessageCheckCallBack(BuyDiceSuccess));
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
            changeIsMain(false);
            //isMain = false;
            shop_dice.SetActive(false);
            shop_goldbar.SetActive(true);
        }
        else if (CMainData.Dice_Count > 4)
        {
            ShopDiceCloseButton();
        }

    }

    public void BuyDiceFalse()
    {
    }

    public GameObject[] mainColliderObject;

    public void changeIsMain(bool changeMain)
    {
        isMain = changeMain;

        if (isMain)
        {
            foreach(GameObject temp in mainColliderObject)
            {
                temp.collider.enabled = true;
            }
        }
        else
        {
            foreach (GameObject temp in mainColliderObject)
            {
                temp.collider.enabled = false;
            }
        }
    }
}
