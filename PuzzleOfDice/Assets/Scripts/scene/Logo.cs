// logo view

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Logo : MonoBehaviour
{
	public const int LOADING_STATE_LOGO = 0;
    public const int LOADING_STATE_NONE = 1;
    public const int LOADING_STATE_FACEBOOK_INIT = 2;
    public const int LOADING_STATE_FACEBOOK_LOGIN = 3;
    public const int LOADING_STATE_SERVER = 4;
    public const int LOADING_STATE_COMPLETE = 5;
    private const float FIRST = 25.0f;
    private const float SECOND = 50.0f;
    private const float THIRD = 75.0f;
    private const float FOURTH = 100.0f;
    public UISprite loading_bar;
    private const string LOADING = "LOADING";
    private const string LOADING_FACEBOOK = "FACEBOOK LOGIN...";
    private const string LOADING_SERVER = "SERVER LOGIN...";
    private const string LOADING_COMPLETE = "LOADING SUCCESS";
    public static int loadingState = LOADING_STATE_NONE;
    //public GUIText progress = null;
    private float nextTime = 0;
    private float currentBar;
    //public Texture2D texLogo = null;
    public Logo_ServerConnection serverConnection;
    // Use this for initialization

    void Start()
    {
        Application.runInBackground = true;
        //load company logo
        //texLogo = (Texture2D)Resources.Load(  "logo/logo_company" );
        //get framecount
		loading_bar.fillAmount = 0;
        currentBar = 0.0f;
        FbDebug.Log("TEST FBDEBUG, App Id is " + FB.AppId);
        //progress.text = LOADING;
        //loadingState = LOADING_STATE_NONE;
		loadingState = LOADING_STATE_LOGO;
        nextTime = Time.frameCount;
    }

    // Update is called once per frame
    void Update()
    {
        switch (loadingState)
        {
			case LOADING_STATE_LOGO:
				if ((Time.frameCount > nextTime + CDefine.LOGO_SHOWFRAME) || (Input.GetKeyDown(KeyCode.Return)))
				{
					loadingState = LOADING_STATE_NONE;
					GameObject.FindGameObjectWithTag("Logo").SetActive(false);
					nextTime = Time.frameCount;
				}
			break;
            case LOADING_STATE_NONE:
                FaceBook.CallFBInit();
                loadingState = LOADING_STATE_FACEBOOK_INIT;
                break;
            case LOADING_STATE_FACEBOOK_INIT:
                if (FaceBook.IsInit)
                {
                    loadingState = LOADING_STATE_FACEBOOK_LOGIN;
                    FaceBook.CallFBLogin();
                }
                break;
            case LOADING_STATE_FACEBOOK_LOGIN:
                if (FB.IsLoggedIn)
                {
                    //·Î±×ÀÎ
                    FbDebug.Log("State To Loading Server");
                    loadingState = LOADING_STATE_SERVER;
                }
                break;
            case LOADING_STATE_SERVER:
                //FbDebug.Log("State To Complete");
                if (currentBar >= FOURTH)
                {
                    serverConnection.SendLogin();
                    loadingState = LOADING_STATE_COMPLETE;
                }                	
                break;
            case LOADING_STATE_COMPLETE:
                //if ((Time.frameCount > nextTime + CDefine.LOGO_SHOWFRAME) || (Input.GetKeyDown(KeyCode.Return)))
                //{
                    //load title scene

                    //FaceBook.sendScore(0);
                
                if (serverConnection.isComplete())
                {
                    SceneTitle.first_loading = true;
                    Application.LoadLevel("SceneTitle");
                }
                else if (!MainData.ServerConnection)
                {
                    loadingState += 1;
                }

                //}
                break;
        }

        //print( Time.frameCount );
        //next scene
        /*
        if( (Time.frameCount > nextTime + CDefine.LOGO_SHOWFRAME) || ( Input.GetKeyDown (KeyCode.Return) ) ){
            //load title scene
            Application.LoadLevel( "SceneTitle" );
        }
        */
    }

    //draw GUI
    void OnGUI()
    {
        //draw cpmpany logo
        //GUI.DrawTexture( new Rect( (Screen.width>>1)-(texLogo.width>>1),(Screen.height>>1)-(texLogo.height>>1),texLogo.width,texLogo.height) , texLogo );O
        switch (loadingState)
        {
            case LOADING_STATE_NONE:
                if (currentBar < FIRST)
                    currentBar += Time.deltaTime * 10;
                //progress.text = LOADING;
                break;
            case LOADING_STATE_FACEBOOK_INIT:
                if(currentBar < FIRST)
                    currentBar += Time.deltaTime * 30;
                else if (currentBar < SECOND)
                    currentBar += Time.deltaTime * 10;
                break;
            case LOADING_STATE_FACEBOOK_LOGIN:
                if (currentBar < SECOND)
                    currentBar += Time.deltaTime * 30;
                else if (currentBar < THIRD)
                    currentBar += Time.deltaTime * 10;
                //progress.text = LOADING_FACEBOOK;
                break;
            case LOADING_STATE_SERVER:
                if (currentBar < THIRD)
                    currentBar += Time.deltaTime * 30;
                else if (currentBar < FOURTH)
                    currentBar += Time.deltaTime * 10;
                //progress.text = LOADING_SERVER;
                break;
			/*
            case LOADING_STATE_COMPLETE:
                if (currentBar < FOURTH)
                    currentBar += Time.deltaTime * 30;
                else if (currentBar <= FINAL)
                    currentBar += Time.deltaTime * 10;
                //progress.text = LOADING_COMPLETE;
                break;
                */
        }

        if (currentBar >= 100.0f)
            loading_bar.fillAmount = currentBar/100.0f;
        else
            loading_bar.fillAmount = currentBar/100.0f;
    }

    /*
    void CallGetAuthResponse()
    {
        FB.GetAuthResponse(LoginCallback);
    }
    */
}
