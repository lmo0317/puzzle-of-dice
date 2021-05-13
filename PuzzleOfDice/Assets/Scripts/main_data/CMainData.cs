using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public enum TUTORIAL_STEP
{
    TUTORIAL_STEP_NONE,

    //포인터를 이동시킴
    TUTORIAL_STEP1_BEGIN,
    TUTORIAL_STEP1_ING,
    TUTORIAL_STEP1_END,

    //주사위를 이동시킴
    TUTORIAL_STEP2_BEGIN,
    TUTORIAL_STEP2_ING,
    TUTORIAL_STEP2_END,
     
    //주사위 2 클리어 
    TUTORIAL_STEP6_BEGIN,
    TUTORIAL_STEP6_ING,
    TUTORIAL_STEP6_END,

    //주사위 3 클리어 
    TUTORIAL_STEP7_BEGIN,
    TUTORIAL_STEP7_ING,
    TUTORIAL_STEP7_END,

    //주사위 4 클리어 
    TUTORIAL_STEP8_BEGIN,
    TUTORIAL_STEP8_ING,
    TUTORIAL_STEP8_END,

    //주사위 5 클리어 
    TUTORIAL_STEP9_BEGIN,
    TUTORIAL_STEP9_ING,
    TUTORIAL_STEP9_END,

    //주사위 6 클리어 
    TUTORIAL_STEP10_BEGIN,
    TUTORIAL_STEP10_ING,
    TUTORIAL_STEP10_END,

    //체인 설명
    TUTORIAL_STEP11_BEGIN,
    TUTORIAL_STEP11_ING,
    TUTORIAL_STEP11_END,

    //주사위 1 설명
    TUTORIAL_STEP12_BEGIN,
    TUTORIAL_STEP12_ING,
    TUTORIAL_STEP12_END,

    //주사위 팁 1.
    TUTORIAL_STEP13_BEGIN,
    TUTORIAL_STEP13_ING,
    TUTORIAL_STEP13_END,

    //주사위 팁 2.
    TUTORIAL_STEP14_BEGIN,
    TUTORIAL_STEP14_ING,
    TUTORIAL_STEP14_END,

    //주사위 팁 3.
    TUTORIAL_STEP15_BEGIN,
    TUTORIAL_STEP15_ING,
    TUTORIAL_STEP15_END,
}

public enum TUTORIAL_SUB_STEP
{
    TUTORIAL_SUB_STEP_NONE,
    TUTORIAL_SUB_STEP1,
    TUTORIAL_SUB_STEP2,
    TUTORIAL_SUB_STEP3,
}

public enum SOUND_STATE
{
    SOUND_STATE_NONE = 0,
    SOUND_STATE_MAIN_READY,
    SOUND_STATE_MAIN,
    SOUND_STATE_GAME_INTRO_READY,
    SOUND_STATE_GAME_INTRO,
    SOUND_STATE_GAME_ING_READY,
    SOUND_STATE_GAME_ING,
    SOUND_STATE_GAME_ING_HURRY_UP,
    SOUND_STATE_GAME_END,
}

public enum CURSOR_STATE
{
    CURSOR_STATE_NONE = 0,
    CURSOR_STATE_START = 1,
    CURSOR_STATE_IDLE = 2,
    CURSOR_STATE_RUN = 8,
    CURSOR_STATE_JUMP_DOWN = 9,
    CURSOR_STATE_JUMP_UP = 10,
};

public class CMainData
{
    //variable
	private static int m_GameMode = GameData.GAME_MODE_BASIC; //game mode (0-endless , 1-stage , 2-blitz)
	private static int m_GameState = GameData.GAME_STATE_READY; //ingame game state
	private static bool m_isSound = true; //sound
	private static bool m_isPause = false; //in game pause situation

    private static int m_nControl = GameData.GAME_CONTROL_KEY;
    private static bool m_music = true;
    private static bool m_sound = true;
    
    //private static string username = "TestUser"; // 테스트용 임시 데이터
    private static string username = ""; // 실제 데이터

    private static string locale = "en_US";
    private static string currency = "USD";
    private static float exchange = 1.0f;
    private static long userscore = 0;
    public static Texture UserTexture;
    private static int userRanking = 0;

    private static int gold = 0;
    private static int dice_count = 1;
    private static float dice_time = 0.0f;
    private static bool tutorial = false;

    public static List<Message> message = new List<Message>();
    private static JsonData topUser = null;

    public static Dictionary<string, string> friendNameList = new Dictionary<string, string>();


    public static List<Dictionary<string, string>> appFriends = new List<Dictionary<string, string>>(); // App Play중인 친구 정보
	//appFriends[i]["fid"], appFriends[i]["name"]


    //method
    public static void setGameMode( int gameMode ) { m_GameMode = gameMode; } 	//set,get game mode
	public static int  getGameMode() { return m_GameMode; } 	//set,get ingame game state
	public static void setGameState( int gameState ) { m_GameState = gameState; }
	public static int  getGameState() { return m_GameState; } //set,get sound situation
	public static void setSound( bool sound ) { m_isSound = sound; }
	public static bool getSound() { return m_isSound; }
	public static void setPause( bool pause ) { m_isPause = pause; } //set,get in game pause situation
	public static bool getPause() { return m_isPause; }


    public static int Control_Method
    {
        get { return m_nControl; }
        set { m_nControl = value; }
    }

    public static bool Music
    {
        get { return m_music; }
        set { m_music = value; }
    }

    public static bool Sound
    {
        get { return m_sound; }
        set { m_sound = value; }
    }

    public static string Username
    {
        get { return username; }
        set { username = value; }
    }

    public static int Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    public static int Dice_Count
    {
        get { return dice_count; }
        set { dice_count = value; }
    }

    public static float Dice_Time
    {
        get { return dice_time; }
        set { dice_time = value; }
    }

    public static string Locale
    {
        get { return locale; }
        set { locale = value; }
    }

    public static string Currency
    {
        get { return currency; }
        set { currency = value; }
    }

    public static float Exchange
    {
        get { return exchange; }
        set { exchange = value; }
    }

    public static long UserScore
    {
        get { return userscore; }
        set { userscore = value; }
    }

    public static int UserRanking
    {
        get { return userRanking; }
        set { userRanking = value; }
    }

    public static JsonData TopUser
    {
        get { return topUser; }
        set { topUser = value; }
    }

    public static bool Tutorial
    {
        get { return tutorial; }
        set { tutorial = value; }
    }
}

public class CDefine
{

    //public const string serverURL = "http://localhost:8080/puzzleofdice/servlet";
    public const string serverURL = "http://thepipercat.com/servlet";
   
    public const string strVersion = "1.0.0";
    public const string DICE_FULL = "FULL";    
    public const int LOGO_SHOWFRAME = 300;
    public const int TITLE_SHOWFRAME = 1000;
    public const float DICE_RECOVERYTIME = 1800.0f;
    public const int TUTORIAL_STEP_LENGTH = 13;

    public const int DICE_PRICE = 12;
}

public class GameData
{
    //board width, height count
    public const int BOARD_SIZE_WIDTH = 9;
    public const int BOARD_SIZE_HEIGHT = 9;
    public const int MAX_DIRECTION = 6;

    //dice object max count
    public const int DICE_TOTAL_COUNT = BOARD_SIZE_WIDTH * BOARD_SIZE_HEIGHT;//49;

    //key state
    public const int KEY_STATE_CURSOR = 0;
    public const int KEY_STATE_DICE = 1;

    //main state	
    public const int EVENT_TYPE_APPEAR = 0;
    public const int EVENT_TYPE_MOVE = 1;
    public const int EVENT_TYPE_DISAPPEAR = 2;
    public const int EVENT_TARGET_CURSOR = 0;
    public const int EVENT_TARGET_DICE = 1;
    public const int EVENT_DIRECTION_RIGHT = 1;
    public const int EVENT_DIRECTION_LEFT = 2;
    public const int EVENT_DIRECTION_UP = 3;
    public const int EVENT_DIRECTION_DOWN = 4;
	
	//game mode
    public const int GAME_MODE_BASIC = 0;
    public const int GAME_MODE_TUTORIAL = 1;
    public const int GAME_MODE_BLITZ = 2;

	//control 
    public const int GAME_CONTROL_KEY = 0;
    public const int GAME_CONTROL_MOUSE = 1;
    public const int GAME_CONTROL_KEYPAD = 2;

    //contorl ON/OFF
	
    public const int KEY_STATE_ON = 0;
    public const int KEY_STATE_OFF = 1;

    //game state
    public const int GAME_STATE_PRE = -1;
    public const int GAME_STATE_READY = 0;
    public const int GAME_STATE_START = 1;
    public const int GAME_STATE_GAMEING = 2;
    public const int GAME_STATE_PAUSE = 3;
    public const int GAME_STATE_TIMEUP = 4;
    public const int GAME_STATE_GAMEOVER = 5;
	
    //dice state
    public const int GAME_DICE_READY = 0;
    public const int GAME_DICE_EVENT = 1;
    public const int GAME_DECE_END = 2;
	
    //object state
	public const int OBJECT_STATE_NONE = 0;
	public const int OBJECT_STATE_MOVE_RIGHT = 1;
    public const int OBJECT_STATE_MOVE_LEFT = 2;
    public const int OBJECT_STATE_MOVE_UP = 3;
    public const int OBJECT_STATE_MOVE_DOWN = 4;
    public const int OBJECT_STATE_APPEAR = 5;
    public const int OBJECT_STATE_DISAPPEAR = 6;

    //object effect state
    public const int OBJECT_EFFECT_STATE_NONE = 0;
    public const int OBJECT_EFFECT_STATE_CLEAR_ON = 1;
    public const int OBJECT_EFFECT_STATE_CLEAR_OFF = 2;
    public const int OBJECT_EFFECT_STATE_MAKE_ON = 3;
    public const int OBJECT_EFFECT_STATE_MAKE_OFF = 4;
	
    //cursor 이동 속도
	public const int OBJECT_CURSOR_MOVE_SPEED = 10;

    //주사위 이벤트 시간
    public const float OBJECT_MOVE_TIME = 0.2f;
    public const float OBJECT_APPEAR_TIME = 3;
    public const float OBJECT_DISAPPEAR_TIME = 8;
    public const float OBJECT_DISAPPEAR_TIME_IN_TUTORIAL = 60;
    public const float OBJECT_DISAPPEAR_TIME_MIN = 1;
    public const float OBJECT_DISAPPEAR_PLUS_PERCENT = 20;
    public const float OBJECT_DISAPPEAR_PLUS_PERCENT_MIN = 1;


    //create dice 주사위 생성 기본 5초
    public const float TIMER_CREATE_DICE_TUTORIAL = 5.0f;
    public const float TIMER_CREATE_DICE_BASIC = 5.0f;
    public const float TIMER_CREATE_DICE_BLITZ = 5.0f;
    public const float TIMER_CREATE_DICE_MIN = 1.0f;
    public const int CREATE_DICE_COUNT = 24;

    //Ready, Go 지속 시간 기본 1초
    public const float TIMER_READY = 1.0f;
    public const float TIMER_GO = 1.0f;

    //level up stage down
    public const float TIMER_CREATE_DICE_STAGE_DOWN = 2.0f;
	
    //blitz game time
#if UNITY_EDITOR
    public const float TIMER_GAMETIME_BLITZ = 30.0f;
#else    
    public const float TIMER_GAMETIME_BLITZ = 180.0f;
#endif    
    ////////////////////////////////////////////////////////////////////////// 
    

    public static string[] STRING_GAME_MODE = new string[3] { "GAME_MODE_BASIC", "GAME_MODE_TUTORIAL", "GAME_MODE_BLITZ" };

    public const float FLASH_TIME = 0.2f;
    public const float WAVE_EFFECT_TIME = 0.2f;


    //tutorial 
    public const float TUTORIAL_STATE_CHANGE_TIME = 0.5f;
   

    //TEST CODE : tutorial test
    //public const TUTORIAL_STEP startTutorialStep = TUTORIAL_STEP.TUTORIAL_STEP10_BEGIN;
    //else 
    public const TUTORIAL_STEP startTutorialStep = TUTORIAL_STEP.TUTORIAL_STEP1_BEGIN;
    /* ******************************************************* */


    //Chain UI
    public const int CHAIN_READY = 0; // ready
    public const int CHAIN_PLAY_LABEL = 1; // play1
    public const int CHAIN_PLAY_NUMBER = 2; // play2
    public const int CHAIN_PLAY_FLIKER = 3; // play3
    public const int CHAIN_PLAY_LOCK = 4; // play4
    public const int CHAIN_FAST = 5; // fast~

    public const float CHAIN_DurationTime = 0.2f;
    public const float CHAIN_FastDurationTime = 0.1f;
    public const float CHAIN_ChainRemainTime = 4.5f;

    //Game Score Label Time
    public const float SCORE_LABEL_endTime = 0.5f; // 생성 후 지속 시간
    public const float SCORE_LABEL_upY = 3.0f; // 초당 증가하는 Y값
    public const float SCORE_LABEL_scale = 1.0f; // 초당 감소하는 Scale 값
    public const float SCORE_LABEL_alpha = 1.0f; // 초당 감소하는 alpha 값
}

public class GameEvent
{
	public int type;
	public int target;
	public int direction;
}

enum RECEIVEMESSAGEACTIONTYPE
{
    ACCEPT,
    CANCLE
}

enum SENDMESSAGEACTIONTYPE
{
    NORMAL,
    HELPTO,
    HELP
}

namespace CallBackMethod
{
    public class CallBackClass
    {
        public delegate void MessageCheckCallBack(JsonData result);
        public delegate void SendMessageCallBack(string fidList);
        public delegate void PriceCheckCallBack(string htmlstring);
    }
}