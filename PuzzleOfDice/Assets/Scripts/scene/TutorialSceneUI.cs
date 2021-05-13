using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CallBackMethod;
using LitJson;

public class TutorialSceneUI : MonoBehaviour {

    int m_nStep = (int)GameData.startTutorialStep/3;
    public UILabel tutorialString;
    public List<GameObject> m_listObject = new List<GameObject>();
    public GameObject m_tutorialCompleteUI;
    public GameObject back;
    public GameObject label;
    public GameObject Pause_Menu;

	// Use this for initialization
	void Start () {
        m_tutorialCompleteUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NextStep()
    {
        if (tutorialString != null)
        {
            tutorialString.text = StringData.getString(StringData.Tutorial_key[m_nStep]);
            //tutorialString.text = UIString.TUTORIAL_STRING[m_nStep];
        }

        for(int i=0;i<m_listObject.Count;i++)
        {
            if (m_nStep == i)
            {
                m_listObject[i].SetActive(true);
            }
            else
            {
                m_listObject[i].SetActive(false);
            }
        }

        m_nStep++;
    }

    public void TutorialComplete()
    {
        for (int i = 0; i < m_listObject.Count; i++)
        {
            m_listObject[i].SetActive(false);
        }

        back.SetActive(false);
        label.SetActive(false);

        SoundManager.g_Instance.PlayEffectSound("menu_open_01");

        m_tutorialCompleteUI.SetActive(true);
    }

    public void OnEnd()
    {
        //server tutorial end message 전송
        //option : tutorial_end
        //fid : FB.UserId

        ButtonClickSound();

        /*
        if (Game_ServerConnection.g_instance != null)
            Game_ServerConnection.g_instance.SendTutorialEnd(new CallBackClass.MessageCheckCallBack(TutorialEndSuccess));
         */
        if (ServerConnection.g_instance != null)
            ServerConnection.g_instance.SendTutorialEnd(new CallBackClass.MessageCheckCallBack(TutorialEndSuccess));
    }

    public void TutorialEndSuccess(JsonData result)
    {
        CMainData.Tutorial = true;
        Application.LoadLevel("SceneTitle");
    }

    public void ShareButton()
    {
        ButtonClickSound();

        try
        {
            CallFBFeed();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    //feed tutorial
    private void CallFBFeed()
    {
        Debug.Log("CallFBFeed() Start ");
        string FeedToId = "";
        string FeedLink = "https://apps.facebook.com/vanishdiceworld/";
        string FeedLinkName = Localization.Localize("41");
        string FeedLinkDescription = Localization.Localize("42");
        string FeedLinkCaption = Localization.Localize("43");
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
        Debug.Log("CallFBFeed() End ");

        if (result.Error != null)
            Debug.Log("Error Response:\n" + result.Error);
        else
        {
            Debug.Log("Success Response:\n");
        }
    }

    public static void ButtonClickSound()
    {
        SoundManager.g_Instance.PlayEffectSound("menu_click_01");
    }

    public void PauseButton()
    {
        Debug.Log("Pause");

        ButtonClickSound();

        if (CMainData.getGameState() == GameData.GAME_STATE_GAMEING && !CMainData.getPause())
        {
            CMainData.setPause(true);
            CMainData.setGameState(GameData.GAME_STATE_PAUSE);
            NGUITools.SetActive(Pause_Menu, true);
        }
    }

    public void ExitButton()
    {
        Debug.Log("Resume");

        ButtonClickSound();

        CMainData.setPause(false);
        NGUITools.SetActive(Pause_Menu, false);
        Application.LoadLevel("SceneTitle");
    }

    public void ResumeButton()
    {
        Debug.Log("Resume");

        ButtonClickSound();

        CMainData.setPause(false);
        CMainData.setGameState(GameData.GAME_STATE_GAMEING);
        NGUITools.SetActive(Pause_Menu, false);
    }
}
