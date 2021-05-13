using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverRenewScore : MonoBehaviour {

    public UILabel label_share_button;
    public UILabel label_message;
    public UILabel label_score;
    public UILabel label_title;

	// Use this for initialization
	void Start () {
        label_score.text = SceneGameOver.score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void ButtonClickSound()
    {
        SoundManager.g_Instance.PlayEffectSound("menu_click_01");
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

    //feed renew score
    private void CallFBFeed()
    {
        if (GameRanking.instance != null)
        {
            GameRanking.instance.ButtonPrev(true);
        }

        string FeedToId = "";
        string FeedLink = "https://apps.facebook.com/vanishdiceworld/";
        string FeedLinkName = Localization.Localize("47");
        string FeedLinkDescription = Localization.Localize("48");
        string FeedLinkCaption = Localization.Localize("49");
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

        if (GameRanking.instance != null)
        {
            GameRanking.instance.ButtonPrev(false);
        }

        if (SceneGameOver.g_instance != null)
        {
            SceneGameOver.g_instance.RenewScoreCloseButton();
        }
    }
}
