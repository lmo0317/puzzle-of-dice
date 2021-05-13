// lib

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

public class FaceBook : MonoBehaviour {	
	public static FaceBook g_Instance;
    public static List<object> friends = null;
    private static Dictionary<string, string> profile 	= null;
    private static Dictionary<string, Texture> friendImages = new Dictionary<string, Texture>();
    private Vector2 scrollPosition = Vector2.zero;
    public GUISkin MenuSkin;
    public Texture LogoTexture;
    private static bool isInit = false;

    public static bool IsInit
    {
        get { return isInit; }
        set { isInit = value; }
    }
	
	void OnStart ()
	{
		g_Instance = this;
	}
	
	#region FB.Init() example

    public static void CallFBInit()
    {
        FbDebug.Log("Facebook Init Call");
        FB.Init(OnInitComplete, OnHideUnity);
    }

    private static void OnInitComplete()
    {
        Debug.Log("FB.Init completed");
        FbDebug.Log("Facebook Init Complete");
        isInit = true;
    }

    private static void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }
    #endregion

    #region FB.Login() example
    public static void APICallback(FBResult result)
    {
        if (result.Error != null)
        {
            FbDebug.Error(result.Error);
            Debug.LogError(result.Error);
            FB.API("/me?fields=id,name,first_name,last_name,gender,locale,currency,friends.limit(1000).fileds(first_name,last_name,id)", Facebook.HttpMethod.GET, APICallback);
            return;
        }

        Debug.Log("PROFILE RESULT = [ " + result.Text + " ]");
        Dictionary<string, string> profile = Util.DeserializeJSONProfile(result.Text);
        FaceBook.setProfile(profile);
        CMainData.Username = profile["name"];
        CMainData.Locale = profile["locale"];
        CMainData.Currency = profile["currency"];
        CMainData.Exchange = System.Convert.ToSingle(profile["exchange"]);
        FaceBook.setFriends(Util.DeserializeJSONFriends(result.Text));
    }

    public static void LoginCallback(FBResult result)
    {
        FbDebug.Log("call login: " + FB.UserId);
        FbDebug.Log("login result: " + result.Text);

        FB.API("/me?fields=id,name,first_name,last_name,gender,locale,currency,friends.limit(1000).fields(first_name,last_name,id)", Facebook.HttpMethod.GET, FaceBook.APICallback);
        FacebookPictureDownloader.EnQueue(FB.UserId);
        FB.API("/app/scores?fields=score,user.limit(1000)", Facebook.HttpMethod.GET, ScoresCallback);
    }

    public static void MyPictureCallback(FBResult result)
    {
        FbDebug.Log("MyPictureCallback");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, MyPictureCallback);
            return;
        }
        CMainData.UserTexture = result.Texture;
    }
	
	public void UpdateFriendScores()
	{
		FB.API("/app/scores?fields=score,user.limit(1000)", Facebook.HttpMethod.GET, ScoresCallback);
	}

    public static void CallFBLogin()
    {
        FbDebug.Log("Facebook Login Call");
        FB.Login("email,publish_actions", FaceBook.LoginCallback);
    }

    public static void ScoresCallback(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }

        List<object> scoresList = Util.DeserializeScores(result.Text);
        var x = 0;
        CMainData.appFriends.Clear();

        foreach (object score in scoresList)
        {
            x++;
            Dictionary<string, object> entry = (Dictionary<string, object>)score;
            Dictionary<string, object> user = (Dictionary<string, object>)entry["user"];

            string userId = (string)user["id"];
            string name = ((string)user["name"]);

            if (string.Equals(userId, FB.UserId))
            {
                FbDebug.Log("Start Ranking is " + x.ToString());
            }
            else
            {
                // App Play 중인 친구 정보 추가
                Dictionary<string, string> friend = new Dictionary<string, string>();
                friend.Add("fid", userId);
                friend.Add("name", name);
                CMainData.appFriends.Add(friend);
            }

            FacebookPictureDownloader.EnQueue(userId);
        }
    }
	
	public static Dictionary<string, string> get_Profile()
	{
		return profile;
	}
	
	public static Dictionary<string, UnityEngine.Texture> getfriendImages()
	{
		return friendImages;
	}

    #endregion

    #region FB.GetAuthResponse() example
    void CallGetAuthResponse()
    {
        //FB.GetAuthResponse(LoginCallback);
    }

    #endregion

    #region FB.PublishInstall() example

    private void CallFBPublishInstall()
    {
        FB.PublishInstall(PublishComplete);
    }

    private void PublishComplete(FBResult result)
    {
        Debug.Log("publish response: " + result.Text);
    }

    #endregion

    #region FB.AppRequest() Friend Selector

    public string FriendSelectorTitle = "";
    public string FriendSelectorMessage = "Derp";
    public string FriendSelectorFilters = "[\"all\",\"app_users\",\"app_non_users\"]";
    public string FriendSelectorData = "{}";
    public string FriendSelectorExcludeIds = "";
    public string FriendSelectorMax = "";

    private void CallAppRequestAsFriendSelector()
    {
        // If there's a Max Recipients specified, include it
        int? maxRecipients = null;
        if (FriendSelectorMax != "")
        {
            try
            {
                maxRecipients = Int32.Parse(FriendSelectorMax);
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }

        // include the exclude ids
        string[] excludeIds = (FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');

        FB.AppRequest(
            FriendSelectorMessage, // message
            null, // to
            FriendSelectorFilters, // filters
            excludeIds, // excludeIds
            maxRecipients, // maxRecipients
            FriendSelectorData, // data
            FriendSelectorTitle, // title
            Callback // callback
        );
    }
    #endregion

    #region FB.AppRequest() Direct Request

    public string DirectRequestTitle = "";
    public string DirectRequestMessage = "Herp";
    private string DirectRequestTo = "";

    private void CallAppRequestAsDirectRequest()
    {
        if (DirectRequestTo == "")
        {
            throw new ArgumentException("\"To Comma Ids\" must be specificed", "to");
        }

        FB.AppRequest(
            DirectRequestMessage, // message
            DirectRequestTo.Split(','), // to
            "", // filters
            null, // excludeIds
            null, // maxRecipients
            "", // data
            DirectRequestTitle, // title
            Callback // callback
        );
    }

    #endregion

    #region FB.Feed() example

    public string FeedToId = "";
    public string FeedLink = "";
    public string FeedLinkName = "";
    public string FeedLinkCaption = "";
    public string FeedLinkDescription = "";
    public string FeedPicture = "";
    public string FeedMediaSource = "";
    public string FeedActionName = "";
    public string FeedActionLink = "";
    public string FeedReference = "";
    public bool IncludeFeedProperties = false;
    private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();

    private void CallFBFeed()
    {
        Dictionary<string, string[]> feedProperties = null;
        if (IncludeFeedProperties)
        {
            feedProperties = FeedProperties;
        }
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
            callback: Callback
        );
    }

    #endregion

    #region FB.Canvas.Pay() example

    public static void CallFBPay(
        string product = "",
        Facebook.FacebookDelegate callback = null)
    {
        FB.Canvas.Pay(product, "purchaseitem", 1, null, null, null, null, null, callback);
        //FB.Canvas.Pay(PayProduct, "purchaseitem", 1, 1, 1, null, "10", "testCurrency", null);
    }
    
    #endregion

    #region FB.API() example

    public string ApiQuery = "";

    private void CallFBAPI()
    {
        FB.API(ApiQuery, Facebook.HttpMethod.GET, Callback);
    }

    #endregion

    #region GUI

    private string status = "Ready";
    private string lastResponse = "";
    public GUIStyle textStyle = new GUIStyle();
    private Texture2D lastResponseTexture;

#if UNITY_IOS || UNITY_ANDROID
    int buttonHeight = 60;
    int mainWindowWidth = 610;
    int mainWindowFullWidth = 640;
#else
    int buttonHeight = 30;
    int mainWindowWidth = 200;
    int mainWindowFullWidth = 230;
#endif

    private int TextWindowHeight
    {
        get
        {
#if UNITY_IOS || UNITY_ANDROID
            return IsHorizontalLayout() ? Screen.height : 85;
#else
        return Screen.height;
#endif

        }
    }

    void Awake()
    {
        textStyle.alignment = TextAnchor.UpperLeft;
        textStyle.wordWrap = true;
        textStyle.padding = new RectOffset(10, 10, 10, 10);
        textStyle.stretchHeight = true;
        textStyle.stretchWidth = false;

        FeedProperties.Add("key1", new[] { "valueString1" });
        FeedProperties.Add("key2", new[] { "valueString2", "http://www.facebook.com" });
    }

    void OnGUI()
    {
        if (IsHorizontalLayout())
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
        }
		
        GUILayout.Box("Status: " + status, GUILayout.MinWidth(mainWindowWidth));

#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
        }
#endif

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(mainWindowFullWidth));
        GUILayout.BeginVertical();
        GUI.enabled = !isInit;
        if (Button("FB.Init"))
        {
            CallFBInit();
            status = "FB.Init() called with " + FB.AppId;
        }

        GUI.enabled = isInit;
        if (Button("Login"))
        {
            CallFBLogin();
            status = "Login called";
        }

#if UNITY_IOS || UNITY_ANDROID
        if (Button("Publish Install"))
        {
            CallFBPublishInstall();
            status = "Install Published";
        }
#endif

        GUI.enabled = FB.IsLoggedIn;
        GUILayout.Space(10);
        
		//LabelAndTextField("Title (optional): ", ref FriendSelectorTitle);
        //LabelAndTextField("Message: ", ref FriendSelectorMessage);
        //LabelAndTextField("Exclude Ids (optional): ", ref FriendSelectorExcludeIds);
        //LabelAndTextField("Filters (optional): ", ref FriendSelectorFilters);
        //LabelAndTextField("Max Recipients (optional): ", ref FriendSelectorMax);
        //LabelAndTextField("Data (optional): ", ref FriendSelectorData);
        
		if (Button("Open Friend Selector"))
        {
            try
            {
                CallAppRequestAsFriendSelector();
                status = "Friend Selector called";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        
		GUILayout.Space(10);
        
		if(Button ("Send Score"))
		{
			String temp = "";
			
			var query = new Dictionary<string, string>();
			query["score"] = "32500";
			FB.API("/me/scores", Facebook.HttpMethod.POST, 
  			delegate(FBResult r) { Debug.Log("Result: " + r.Text); temp = r.Text; }, query);
			
			status = "Send Score(32500) : " + temp;
		}
		
		GUILayout.Space(10);
		
		if(Button ("Send Profile"))
		{
			String temp = "";
			
			var query = new Dictionary<string, string>();
			query["profile"] = "My_Test";
			FB.API("/me/testunityapp_byhan:profile", Facebook.HttpMethod.POST, 
  			delegate(FBResult r) { Debug.Log("Result: " + r.Text); temp = r.Text; }, query);
			
			status = "Send Profile : " + temp;
		}
		
		//LabelAndTextField("Title (optional): ", ref DirectRequestTitle);
        //LabelAndTextField("Message: ", ref DirectRequestMessage);
        //LabelAndTextField("To Comma Ids: ", ref DirectRequestTo);
        
		/*
		if (Button("Open Direct Request"))
        {
            try
            {
                CallAppRequestAsDirectRequest();
                status = "Direct Request called";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        GUILayout.Space(10);
        LabelAndTextField("To Id (optional): ", ref FeedToId);
        LabelAndTextField("Link (optional): ", ref FeedLink);
        LabelAndTextField("Link Name (optional): ", ref FeedLinkName);
        LabelAndTextField("Link Desc (optional): ", ref FeedLinkDescription);
        LabelAndTextField("Link Caption (optional): ", ref FeedLinkCaption);
        LabelAndTextField("Picture (optional): ", ref FeedPicture);
        LabelAndTextField("Media Source (optional): ", ref FeedMediaSource);
        LabelAndTextField("Action Name (optional): ", ref FeedActionName);
        LabelAndTextField("Action Link (optional): ", ref FeedActionLink);
        LabelAndTextField("Reference (optional): ", ref FeedReference);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Properties (optional)", GUILayout.Width(150));
        IncludeFeedProperties = GUILayout.Toggle(IncludeFeedProperties, "Include");
        GUILayout.EndHorizontal();
        if (Button("Open Feed Dialog"))
        {
            try
            {
                CallFBFeed();
                status = "Feed dialog called";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        GUILayout.Space(10);

#if UNITY_WEBPLAYER
        LabelAndTextField("Product: ", ref PayProduct);
        if (Button("Call Pay"))
        {
            CallFBPay();
        }
        GUILayout.Space(10);
#endif

        LabelAndTextField("API: ", ref ApiQuery);
        if (Button("Call API"))
        {
            status = "API called";
            CallFBAPI();
        }
        */

        GUILayout.Space(10);

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        if (IsHorizontalLayout())
        {
            GUILayout.EndVertical();
        }
        GUI.enabled = true;
		
		/*
        var textAreaSize = GUILayoutUtility.GetRect(640, TextWindowHeight);

        GUI.TextArea(
            textAreaSize,
            string.Format(
                " AppId: {0} \n Facebook Dll: {1} \n UserId: {2}\n IsLoggedIn: {3}\n AccessToken: {4}\n\n {5}",
                FB.AppId,
                (isInit) ? "Loaded Successfully" : "Not Loaded",
                FB.UserId,
                FB.IsLoggedIn,
                FB.AccessToken,
                lastResponse
            ), textStyle);

        if (lastResponseTexture != null)
        {
            GUI.Label(new Rect(textAreaSize.x + 5, textAreaSize.y + 200, lastResponseTexture.width, lastResponseTexture.height), lastResponseTexture);
        }
		*/
		
        if (IsHorizontalLayout())
        {
            GUILayout.EndHorizontal();
        }
    }

    void Callback(FBResult result)
    {
        lastResponseTexture = null;
        if (result.Error != null)
            lastResponse = "Error Response:\n" + result.Error;
        else if (!ApiQuery.Contains("/picture"))
            lastResponse = "Success Response:\n" + result.Text;
        else
        {
            lastResponseTexture = result.Texture;
            lastResponse = "Success Response:\n";
        }
    }

    private bool Button(string label)
    {
        return GUILayout.Button(label, GUILayout.MinHeight(buttonHeight));
    }

    private void LabelAndTextField(string label, ref string text)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(150));
        text = GUILayout.TextField(text, GUILayout.MinWidth(300));
        GUILayout.EndHorizontal();
    }

    private bool IsHorizontalLayout()
    {
#if UNITY_IOS || UNITY_ANDROID
        return Screen.orientation == ScreenOrientation.Landscape;
#else
        return true;
#endif
    }

    #endregion
	
	public static void setProfile(Dictionary<string, string> profile){
		FaceBook.profile = profile;
	}

    public static void setFriends(List<object> friends)
    {
		FaceBook.friends = friends;

        string result = "FRIENDS = ";
        foreach (Dictionary<string, object> temp in FaceBook.friends)
        {
            result += (string)temp["id"] + ",";
        }
        Debug.Log(result);
	}

    public static void CallAppRequestAsDirectRequestMessage(string title, string message, string tofid, Facebook.FacebookDelegate Callback = null)
    {
        if (SceneGameOver.g_instance != null)
        {
            SceneGameOver.g_instance.buttonClickable = false;
        }

        if (tofid == "")
        {
            throw new ArgumentException("\"To Comma Ids\" must be specificed", "to");
        }

        if (Callback == null)
        {
            Callback = Callback_;
        }

        FB.AppRequest(
            message, // message
            tofid.Split(','), // to
            "", // filter
            null, // excludeIds
            null, // maxRecipients
            "", // data
            title, // title
            Callback // callback
        );
    }

    public static void Callback_(FBResult result)
    {
        if (SceneGameOver.g_instance != null)
        {
            SceneGameOver.g_instance.buttonClickable = true;
        }

        Debug.Log("FaceBook CallBack");

        if (result.Error != null)
        {
            Debug.Log("Error Response:\n" + result.Error);
        }
        else
        {
            Debug.Log("Success Response:\n" + result.Text);
        }
    }

    public static void LoadFriendImages()
    {
        if (FaceBook.friends != null)
        {
            foreach (Dictionary<string, object> temp in FaceBook.friends)
            {
                FacebookPictureDownloader.EnQueue((string)temp["id"]);
                /*
                if (!friendImages.ContainsKey((string)temp["id"]))
                {
                    // We don't have this players image yet, request it now
                    FB.API(Util.GetPictureURL((string)temp["id"], 128, 128), Facebook.HttpMethod.GET, pictureResult =>
                    {
                        if (pictureResult.Error != null)
                        {
                            FbDebug.Error(pictureResult.Error);
                        }
                        else
                        {
                            try
                            {
                                friendImages.Add((string)temp["id"], pictureResult.Texture);
                            }
                            catch (System.Exception e)
                            {
                                Debug.Log(e.Message + " : Exceiption.");
                            }
                        }
                    });
                }
                 */
            }
        }
    }
}

