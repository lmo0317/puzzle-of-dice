using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//랭킹 추월에 관한 UI
//renew test 
//targetFid = "100007063804919";

public class GameOverRenewRanking : MonoBehaviour {

    public UILabel label_message;

    // down
    public UILabel label_down_name;
    public UILabel label_down_rank;
    public UILabel label_down_score;
    public TextureUpdate texture_down_texture;

    // up
    public UILabel label_up_name;
    public UILabel label_up_rank;
    public UILabel label_up_score;
    public TextureUpdate texture_up_texture;

    //string
    private string targetFid = "";
    private string targetName = "";

	void Start () 
    {
        if (SceneGameOver.ResultInstance != null)
        {
            //up - 내 기록으로 채운다.
            int myRanking = int.Parse(SceneGameOver.ResultInstance["ranking"].ToString());
            if (label_up_name != null)
                label_up_name.text = CMainData.Username;
            if (label_up_rank != null)
                label_up_rank.text = myRanking.ToString();
            if (label_up_score != null)
                label_up_score.text = CMainData.UserScore.ToString();
            texture_up_texture.fid = FB.UserId;

            //down 내가 추월한 user정보
            if (label_down_name != null)
                label_down_name.text = SceneGameOver.ResultInstance["7_name"].ToString();
            if (label_down_rank != null)
                label_down_rank.text = (myRanking + 1).ToString();
            if (label_down_score != null)
                label_down_score.text = SceneGameOver.ResultInstance["7_score"].ToString();
            if (label_message != null) 
                { label_message.text = StringData.getString(StringData.RenewRanking_key).Replace("%s", SceneGameOver.ResultInstance["7_name"].ToString()); }

            targetFid = SceneGameOver.ResultInstance["7_fid"].ToString();
            texture_down_texture.fid = targetFid;
            targetName = SceneGameOver.ResultInstance["7_name"].ToString();            
        }
    }
	
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

    //feed renew ranking
    private void CallFBFeed()
    {
        if (GameRanking.instance != null)
        {
            GameRanking.instance.ButtonPrev(true);
        }

        Debug.Log("CallFBFeed() Start ");
        string FeedToId = targetFid;
        string FeedLink = "https://apps.facebook.com/vanishdiceworld/";
        string FeedLinkName = Localization.Localize("44");
        string FeedLinkDescription = Localization.Localize("45").Replace("%s", targetName);
        string FeedLinkCaption = Localization.Localize("46");
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
