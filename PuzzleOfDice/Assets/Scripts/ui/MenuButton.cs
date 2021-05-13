// button 

// add button
// 1. add - enum
// 2. add - string
// 3. add - type in checkButton()
// 4. add - type in buttonProcessing()

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;


public class MenuButton : MonoBehaviour {
	
	enum BUTTON {
		ENDLESS = 0,
		STAGE,
		BLITZ,
		RANKING,
		OPTION,
		BUYSTARTGAME,
		BUYBACK,
		G_PAUSE,
		G_RESUME,
		G_RESTART,
		G_MAINMENU,
		G_SOUND_ON,
		G_SOUND_OFF,
		G_RANKINGBACK,
		FACEBOOK_INIT,
	};	
	
	public string text;
	public Texture2D img = null;
	public Rect position;
	private Rect rect;
	private int buttonType = -1;
	private string strEndless = "Endless";
	private string strStage = "Stage";
	private string strBlitz = "Blitz";
	private string strRanking = "Ranking";
	private string strOptions = "Options";
	private string strBuyBack = "BuyBack";
	private string strG_Pause = "PAUSE";
	private string strG_Resume = "RESUME";
	private string strG_ReStart = "RESTART";
	private string strG_MainMenu = "MAINMENU";
	private string strG_Sound_On = "SOUND_ON";
	private string strG_Sound_Off = "SOUND_OFF";
	private string strFaceBookInit = "FACEBOOK_INIT";	
	private string strRankingBack = "RankingBack";
	private string strBuyStartGame = "BuyStartGame";

	void Start () {
		buttonType = checkButton( text );
		setPosition( buttonType );		
	}
	
	void Update () {
	
	}
	
	void OnGUI() {
	
		//pause
		if( CMainData.getPause() == true ){
			if( buttonType == (int)BUTTON.G_PAUSE ) {
				return;
			}
		}
		else if(CMainData.getGameState() == GameData.GAME_STATE_TIMEUP){
			if( buttonType == (int)BUTTON.G_PAUSE || 
				buttonType == (int)BUTTON.G_RESUME ) { 
					return;
			}
		}
		else{
			if( buttonType == (int)BUTTON.G_RESUME ||
				buttonType == (int)BUTTON.G_RESTART || 
				buttonType == (int)BUTTON.G_MAINMENU ) {
					return;
			}
		}
		//sound
		if( CMainData.getSound() == true ){
			if( buttonType == (int)BUTTON.G_SOUND_OFF ){
				return;
			}
		}
		else{
			if( buttonType == (int)BUTTON.G_SOUND_ON ) {
				return;
			}
		}	
		
		//check button 
		if( img == null ){
			//set button background color
			GUI.backgroundColor = Color.white;
			
			if( GUI.Button( rect , text ) ){
				//button processing
				buttonProcessing( buttonType );
			}
		}
		else{
			//none show button background window
			GUI.backgroundColor = Color.clear;
			
			if( GUI.Button( rect , img ) ){
				//button processing
				buttonProcessing( buttonType );
			}
		}
	}
	
	/*
	 * @brief check button type
	 * @param string button : button type string
	 * @return int : button type index
	 * */
	int checkButton( string button)
	{
		//main menu scene
		if( button.Equals( strEndless ) ) 			return (int)BUTTON.ENDLESS;
		else if( button.Equals( strStage ) ) 		return (int)BUTTON.STAGE;
		else if( button.Equals( strBlitz ) ) 		return (int)BUTTON.BLITZ;
		else if( button.Equals( strRanking ) ) 		return (int)BUTTON.RANKING;
		else if( button.Equals( strOptions ) ) 		return (int)BUTTON.OPTION;
		//buy menu scene
		else if( button.Equals( strBuyStartGame ) ) return (int)BUTTON.BUYSTARTGAME;
		else if( button.Equals( strBuyBack ) ) 		return (int)BUTTON.BUYBACK;
		//game scene
		else if( button.Equals( strG_Pause ) ) 		return (int)BUTTON.G_PAUSE; 
		else if( button.Equals( strG_Resume ) ) 	return (int)BUTTON.G_RESUME;
		else if( button.Equals( strG_ReStart ) ) 	return (int)BUTTON.G_RESTART;
		else if( button.Equals( strG_MainMenu ) ) 	return (int)BUTTON.G_MAINMENU;
		else if( button.Equals( strG_Sound_On ) ) 	return (int)BUTTON.G_SOUND_ON;
		else if( button.Equals( strG_Sound_Off ) ) 	return (int)BUTTON.G_SOUND_OFF;
		//ranking scene
		else if( button.Equals( strRankingBack ) ) 	return (int)BUTTON.G_RANKINGBACK;
		else if( button.Equals( strFaceBookInit ) ) return  (int)BUTTON.FACEBOOK_INIT;
		
		return -1;
	}
	
	/*
	 * @brief init set button position
	 * @param int type : button type
	 * */
	void setPosition( int type )
	{
		if( type == -1 ) return;
		
		//set draw button box position 
		if( img == null ){
			if( position.width == 0 ) position.width = 150;
			if( position.height == 0) position.height = 40;
			rect = position;
		}
		else{
			rect.Set( position.x , position.y, img.width, img.height );
		}
		
		//set position -> center
		if(rect.x == 0 && rect.y == 0){
			rect.x = ( Screen.width >> 1 ) - ( rect.width / 2 );
		}
		
		switch ( type )
		{
		//main menu scene
		case (int)BUTTON.ENDLESS:
			rect.y = (Screen.height>>1)-(((rect.height*5)+(10*4))/2);
			break;
		case (int)BUTTON.STAGE:
			rect.y = (Screen.height>>1)-(((rect.height*5)+(10*4))/2)+((rect.height*1)+10);
			break;
		case (int)BUTTON.BLITZ:
			rect.y = (Screen.height>>1)-(((rect.height*5)+(10*4))/2)+((rect.height*2)+20);
			break;
		case (int)BUTTON.RANKING:
			rect.y = (Screen.height>>1)-(((rect.height*5)+(10*4))/2)+((rect.height*3)+30);
			break;
		case (int)BUTTON.OPTION:
			rect.y = (Screen.height>>1)-(((rect.height*5)+(10*4))/2)+((rect.height*4)+40);
			break;
			
		// facebook
		case (int)BUTTON.FACEBOOK_INIT:
			rect.y = (Screen.height>>1)-(((rect.height*5)+(10*4))/2)+((rect.height*5)+50);
			break;
			
		//buy item menu scene
		case (int)BUTTON.BUYSTARTGAME:
			rect.y = (Screen.height>>1)-(30/2);
			break;
		case (int)BUTTON.BUYBACK:
			rect.x = (Screen.width-rect.width-20);
			rect.y = (Screen.height-rect.height-20);
			break;
			
		//game scene
		case (int)BUTTON.G_PAUSE: 
			rect.x = (Screen.width-rect.width-20);
			rect.y = (5);
			break;
		case (int)BUTTON.G_RESUME:
			rect.y = (Screen.height>>1)-(((rect.height*3)+(10*2))/2);
			break;
		case (int)BUTTON.G_RESTART:
			rect.y = (Screen.height>>1)-(((rect.height*3)+(10*2))/2)+((rect.height*1)+10);
			break;
		case (int)BUTTON.G_MAINMENU:
			rect.y = (Screen.height>>1)-(((rect.height*3)+(10*2))/2)+((rect.height*2)+20);
			break;
		case (int)BUTTON.G_SOUND_ON:
		case (int)BUTTON.G_SOUND_OFF:
			rect.x = (Screen.width-rect.width-20);
			rect.y = (5+rect.height+10);
			break;
			
		//ranking scene
		case (int)BUTTON.G_RANKINGBACK:
			rect.x = (Screen.width-rect.width-20);
			rect.y = (Screen.height-rect.height-20);
			break;
		}
	}
	
	/*
	 * @brief button processing
	 * @param int type : button type
	 * */
	void buttonProcessing( int type )
	{
		if( type == -1 ) return;
		/*
		switch ( type )
		{
		//main menu scene
		case (int)BUTTON.ENDLESS:
			CMainData.setGameMode( GameData.GAME_MODE_ENDLESS ); //set game mode
			CMainData.setGameState( GameData.GAME_STATE_GAMEING ); //set game state
  			Application.LoadLevel( "GameScene" ); //load game scene		
			break;
		case (int)BUTTON.STAGE:
			//set game mode
			CMainData.setGameMode( GameData.GAME_MODE_STAGE );			
			//set game state
			CMainData.setGameState( GameData.GAME_STATE_GAMEING );
			Application.LoadLevel( "GameScene" );
			break;
		case (int)BUTTON.BLITZ:
			//set game mode
			CMainData.setGameMode( GameData.GAME_MODE_BLITZ );
 			Application.LoadLevel( "SceneBuyMenu" );
			break;
		case (int)BUTTON.RANKING:
			Application.LoadLevel( "SceneRanking" );
			break;
		case (int)BUTTON.OPTION:
			rect.y = (Screen.height>>1)-(((rect.height*5)+(10*4))/2)+((rect.height*4)+40);
			break;			
	
		//buy item menu scene
		case (int)BUTTON.BUYSTARTGAME:
			CMainData.setPause( false );
			//HanK
			CMainData.setGameState(GameData.GAME_STATE_READY);
			Application.LoadLevel( "GameScene" );
			break;
		case (int)BUTTON.BUYBACK:
			Application.LoadLevel( "SceneMenu" );
			break;
			
		//game scene
		case (int)BUTTON.G_PAUSE: 
			CMainData.setGameState( GameData.GAME_STATE_PAUSE );
			CMainData.setPause( true );
			break;
		case (int)BUTTON.G_RESUME:
			CMainData.setGameState( GameData.GAME_STATE_GAMEING );
			CMainData.setPause( false );
			break;
		case (int)BUTTON.G_RESTART:
			CMainData.setGameState( GameData.GAME_STATE_READY );
			CMainData.setPause( false );
			break;
		case (int)BUTTON.G_MAINMENU:
			Application.LoadLevel( "SceneMenu" );
			break;
		case (int)BUTTON.G_SOUND_ON:
			CMainData.setSound( false );
			break;
		case (int)BUTTON.G_SOUND_OFF:
			CMainData.setSound( true );
			break;
			
		//ranking scene
		case (int)BUTTON.G_RANKINGBACK:
			Application.LoadLevel( "SceneMenu" );
			break;
		}
        */
	}	
}
