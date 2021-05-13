//main game
using UnityEngine;
using System.Collections;

public class MainGame : MonoBehaviour
{
    public static MainGame g_Instance;

    private GameObject m_oBoard = null;
    private GameObject m_oCursor = null;
    private GameObject m_oScore = null;
    private Board m_sBoard = null;
    private Cursor m_sCursor = null;
    private GameEvent m_Event = null;
    private GameEvent m_CurEvent = null;
    private GameObject m_Star = null;
    private GameObject m_Star2 = null;
    private GameObject m_Arrow = null;
    private GameObject m_Arrow2 = null;
    public GameSceneUI gameSceneUI;

    public GameObject GameModeUI;
    public GameObject TutorialModeUI;

    private float startTime;
    private long m_lScore = 0;
    private int m_nKeyState = GameData.KEY_STATE_CURSOR;
    private int m_nDiceState = GameData.GAME_DICE_READY;
    private TUTORIAL_STEP m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP_NONE;
    private TUTORIAL_SUB_STEP m_nTutorailSubStep = TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP_NONE;
    private SOUND_STATE m_nSoundState = SOUND_STATE.SOUND_STATE_NONE;

    TutorialSceneUI tutorialUI;
    public Board getBoard() { return m_sBoard; }
    public Cursor GetCursor() { return m_sCursor; }
    public TUTORIAL_STEP getTutorialState() {return m_nTutorialState; }

    public GameEvent GetCursorEvent() { return m_CurEvent; }

    void Start()
    {
        g_Instance = this;
        Application.runInBackground = true;
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

        // 기존 Input
        /*
		Queue queue = new Queue();
        m_EventQueue = Queue.Synchronized(queue);
		m_EventQueue.Clear();
        */

        m_oBoard = GameObject.Find("Board");
        m_oCursor = GameObject.Find("Cursor");
        m_sBoard = m_oBoard.GetComponent("Board") as Board;
        m_sCursor = m_oCursor.GetComponent("Cursor") as Cursor;
        m_oScore = GameObject.Find("objScore");
        m_Star = GameObject.Find("Star");
        m_Star2 = GameObject.Find("Star2");
        m_Arrow = GameObject.Find("Arrow");
        m_Arrow2 = GameObject.Find("Arrow2");

        CTimeManager.SetTimer();
        if (CMainData.getGameMode() == GameData.GAME_MODE_BLITZ)
        {
            //Processing GameUI
            //CMainData.setGameState(GameData.GAME_STATE_PRE);           
        }

        m_Star.SetActive(false);
        m_Arrow.SetActive(false);
        m_Star2.SetActive(false);
        m_Arrow2.SetActive(false);

       //TEST CODE : tutorial test 
      //CMainData.setGameMode(GameData.GAME_MODE_TUTORIAL);
       /* ******************************************************* */

        if(CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL)
        {
            TutorialModeUI.SetActive(true);
            GameModeUI.SetActive(false);
        }
        else {
            TutorialModeUI.SetActive(false);
            GameModeUI.SetActive(true);
        }

        tutorialUI = TutorialModeUI.GetComponent("TutorialSceneUI") as TutorialSceneUI;
    }

    public void setSoundState(SOUND_STATE soundState)
    {
        m_nSoundState = soundState;
    }

    void GameProcess()
    {
        if (CMainData.getGameState() == GameData.GAME_STATE_READY)
        {
            startTime = 0.0f;
            m_lScore = 0;
            m_sBoard.initBoards();
            m_sBoard.makeStartDice();
            CTimeManager.SetTimer();

            if (gameSceneUI)
            {
                gameSceneUI.ChangeState(CMainData.getGameState());
            }

            CMainData.setGameState(GameData.GAME_STATE_START);
            CKeyManager.ControlOn();

            m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP_NONE;
        }
        else if (CMainData.getGameState() == GameData.GAME_STATE_START)
        {
            processReadyUI();
        }
        else if (CMainData.getGameState() == GameData.GAME_STATE_GAMEING)
        {
            CheckControl();
            CheckEvent();
            CheckTimer();
            if (CheckBoard())
            {
                CMainData.setGameState(GameData.GAME_STATE_GAMEOVER);
            }
        }
        else if (CMainData.getGameState() == GameData.GAME_STATE_PAUSE)
        {

        }
        else if (CMainData.getGameState() == GameData.GAME_STATE_GAMEOVER)
        {
            GameOverProcess();
        }
        else if (CMainData.getGameState() == GameData.GAME_STATE_TIMEUP)
        {
            GameOverProcess();
        }

        PlayBGM();
    }

    void TutorialProcess()
    {
        if (CMainData.getGameState() == GameData.GAME_STATE_READY)
        {
            startTime = 0.0f;
            m_lScore = 0;
            m_sBoard.initBoards();
            CTimeManager.SetTimer();
            MainGame.g_Instance.setSoundState(SOUND_STATE.SOUND_STATE_GAME_INTRO_READY);

            CMainData.setGameState(GameData.GAME_STATE_GAMEING);
            m_nTutorialState = GameData.startTutorialStep;
        }
        else if (CMainData.getGameState() == GameData.GAME_STATE_GAMEING)
        {
            CheckControl();
            CheckEvent();
            TutorialMainProcess();
        }
        else if (CMainData.getGameState() == GameData.GAME_STATE_PAUSE)
        {

        }
        else if (CMainData.getGameState() == GameData.GAME_STATE_GAMEOVER)
        {

        }
        else if (CMainData.getGameState() == GameData.GAME_STATE_TIMEUP)
        {
            GameOverProcess();
        }

        PlayBGM();
    }

    void Update()
    {
        if (CMainData.getGameMode() != GameData.GAME_MODE_TUTORIAL)
        {
            GameProcess();
        }
        else if (CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL)
        {
            TutorialProcess();
        }
    }

    void processReadyUI()
    {
        startTime += Time.deltaTime;
        if (startTime > GameData.TIMER_READY + GameData.TIMER_GO)
        {
            CMainData.setGameState(GameData.GAME_STATE_GAMEING);

            if (gameSceneUI)
            {
                gameSceneUI.ChangeState(CMainData.getGameState());
            }
        }
        else if (startTime > GameData.TIMER_READY)
        {
            if (gameSceneUI)
            {
                gameSceneUI.ChangeState(CMainData.getGameState());
            }
        }
    }

    void PlayBGM()
    {
        switch (m_nSoundState)
        {
            case SOUND_STATE.SOUND_STATE_GAME_INTRO_READY:
                Debug.Log("Game Mode = " + m_nSoundState);

                if (CMainData.getGameMode() == GameData.GAME_MODE_BASIC)
                {
                    SoundManager.g_Instance.PlayBGM("BGM_basic_Intro_01", false);
                    m_nSoundState = SOUND_STATE.SOUND_STATE_GAME_INTRO;
                }
                else if (CMainData.getGameMode() == GameData.GAME_MODE_BLITZ)
                {
                    SoundManager.g_Instance.PlayBGM("BGM_blitz_Intro_01", false);
                    m_nSoundState = SOUND_STATE.SOUND_STATE_GAME_INTRO;
                }
                else if (CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL)
                {
                    m_nSoundState = SOUND_STATE.SOUND_STATE_GAME_ING_READY;
                }

                break;

            case SOUND_STATE.SOUND_STATE_GAME_INTRO:
                if (SoundManager.g_Instance.IsPlayingBGM() == false)
                {
                    m_nSoundState = SOUND_STATE.SOUND_STATE_GAME_ING_READY;
                }
                break;

            case SOUND_STATE.SOUND_STATE_GAME_ING_READY:

                Debug.Log("Game Mode = " + m_nSoundState);
                if (CMainData.getGameMode() == GameData.GAME_MODE_BASIC)
                {
                    Debug.Log("basic Sound");
                    SoundManager.g_Instance.PlayBGM("BGM_basic_Looping_01", true);
                }
                else if (CMainData.getGameMode() == GameData.GAME_MODE_BLITZ)
                {
                    Debug.Log("blitz Sound");
                    SoundManager.g_Instance.PlayBGM("BGM_blitz_Looping_01", true);
                }
                else if (CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL)
                {
                    Debug.Log("Tutorial Sound");
                    SoundManager.g_Instance.PlayBGM("BGM_tutorial_01", true);
                }

                m_nSoundState = SOUND_STATE.SOUND_STATE_GAME_ING;
                break;
            case SOUND_STATE.SOUND_STATE_GAME_ING:

                if (CMainData.getGameMode() == GameData.GAME_MODE_BLITZ)
                {
                    if (CTimeManager.m_TimeBlitz - CTimeManager.m_TimeStampBlitz < 30)
                    {
                        SoundManager.g_Instance.PlayBGM("BGM_blitz_HurryUp_Looping_01", true);
                        m_nSoundState = SOUND_STATE.SOUND_STATE_GAME_ING_HURRY_UP;
                    }
                }
                break;
            case SOUND_STATE.SOUND_STATE_GAME_ING_HURRY_UP:
                break;

            case SOUND_STATE.SOUND_STATE_GAME_END:
                SoundManager.g_Instance.PlayBGM("BGM_main_01", true);
                break;
        }
    }

    public void increaseCreateDiceCount()
    {
        m_sBoard.result_DiceN[0] += 1;
    }

    //HanK
    void GameOverProcess()
    {
        //GameOver.mode = CMainData.getGameMode();
        //GameOver.score = m_lScore;

        // 수정필수!!

        SceneGameOver.result_DiceN = m_sBoard.result_DiceN;
        SceneGameOver.result_maxChain = m_sBoard.result_maxChain;
        SceneGameOver.result_maxChainScore = m_sBoard.result_maxChainScore;

        SceneGameOver.score = m_lScore;
        Application.LoadLevel("SceneGameOver");
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(5, 5, 300, 20), GameData.STRING_GAME_MODE[CMainData.getGameMode()].ToString());
        //GUI.Label(new Rect(5, 20, 300, 20),"LEVEL = " + CLevelManager.m_nStageLevel);
        //GUI.Label(new Rect(5, 35, 300, 20), "TIME CD = " + CTimeManager.m_TimeCreateDice);
    }

    void CheckControl()
    {
        if (CKeyManager.IsKeyEnable())
        {
            CheckKeyState();
        }
    }

    void CheckKeyState()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_nKeyState = GameData.KEY_STATE_DICE;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_nKeyState = GameData.KEY_STATE_CURSOR;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            GameEvent gameEvent = new GameEvent();
            gameEvent.type = GameData.EVENT_TYPE_MOVE;
            gameEvent.direction = GameData.EVENT_DIRECTION_RIGHT;

            if (m_nKeyState == GameData.KEY_STATE_CURSOR)
            {
                gameEvent.target = GameData.EVENT_TARGET_CURSOR;
            }
            else
            {
                if(CheckKeyEnable(KeyCode.RightArrow) == false)
                {
                    return;
                }

                gameEvent.target = GameData.EVENT_TARGET_DICE;
            }

            m_Event = gameEvent;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            GameEvent gameEvent = new GameEvent();
            gameEvent.type = GameData.EVENT_TYPE_MOVE;
            gameEvent.direction = GameData.EVENT_DIRECTION_LEFT;

            if (m_nKeyState == GameData.KEY_STATE_CURSOR)
            {
                gameEvent.target = GameData.EVENT_TARGET_CURSOR;
            }
            else
            {
                if (CheckKeyEnable(KeyCode.LeftArrow) == false)
                {
                    return;
                }

                gameEvent.target = GameData.EVENT_TARGET_DICE;
            }

            m_Event = gameEvent;

        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            GameEvent gameEvent = new GameEvent();
            gameEvent.type = GameData.EVENT_TYPE_MOVE;
            gameEvent.direction = GameData.EVENT_DIRECTION_UP;

            if (m_nKeyState == GameData.KEY_STATE_CURSOR)
            {
                gameEvent.target = GameData.EVENT_TARGET_CURSOR;
            }
            else
            {
                if (CheckKeyEnable(KeyCode.UpArrow) == false)
                {
                    return;
                }

                gameEvent.target = GameData.EVENT_TARGET_DICE;
            }

            m_Event = gameEvent;

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            GameEvent gameEvent = new GameEvent();
            gameEvent.type = GameData.EVENT_TYPE_MOVE;
            gameEvent.direction = GameData.EVENT_DIRECTION_DOWN;
            if (m_nKeyState == GameData.KEY_STATE_CURSOR)
            {
                gameEvent.target = GameData.EVENT_TARGET_CURSOR;
            }
            else
            {
                if (CheckKeyEnable(KeyCode.DownArrow) == false)
                {
                    return;
                }

                gameEvent.target = GameData.EVENT_TARGET_DICE;
            }
            m_Event = gameEvent;
        }
        else
        {
            if (m_Event != null)
            {
                lock (m_Event)
                {
                    m_Event = null;
                }
            }
        }
    }

    bool CheckKeyEnable(UnityEngine.KeyCode key)
    {
        //움직일수 있는 주사위 인지 검사
        DiceSet diceSet = m_sBoard.GetDice(m_oCursor.transform.position);
        if (diceSet != null)
        {
            if (diceSet.script.getMoveAble() == false) return false;
        }

        //특정 방향으로 못움직이는 주사위 검사
        switch(m_nTutorialState)
        {
            case TUTORIAL_STEP.TUTORIAL_STEP6_BEGIN:
            case TUTORIAL_STEP.TUTORIAL_STEP6_ING:
            case TUTORIAL_STEP.TUTORIAL_STEP6_END:
            case TUTORIAL_STEP.TUTORIAL_STEP7_BEGIN:
            case TUTORIAL_STEP.TUTORIAL_STEP7_ING:
            case TUTORIAL_STEP.TUTORIAL_STEP7_END:
            case TUTORIAL_STEP.TUTORIAL_STEP8_BEGIN:
            case TUTORIAL_STEP.TUTORIAL_STEP8_ING:
            case TUTORIAL_STEP.TUTORIAL_STEP8_END:
            case TUTORIAL_STEP.TUTORIAL_STEP9_BEGIN:
            case TUTORIAL_STEP.TUTORIAL_STEP9_ING:
            case TUTORIAL_STEP.TUTORIAL_STEP9_END:
            case TUTORIAL_STEP.TUTORIAL_STEP10_BEGIN:
            case TUTORIAL_STEP.TUTORIAL_STEP10_ING:
            case TUTORIAL_STEP.TUTORIAL_STEP10_END:
            case TUTORIAL_STEP.TUTORIAL_STEP11_BEGIN:
            case TUTORIAL_STEP.TUTORIAL_STEP11_ING:
            case TUTORIAL_STEP.TUTORIAL_STEP11_END:
            case TUTORIAL_STEP.TUTORIAL_STEP12_BEGIN:
            case TUTORIAL_STEP.TUTORIAL_STEP12_ING:
            case TUTORIAL_STEP.TUTORIAL_STEP12_END:
                //왼쪽으로만 이동 가능
                {
                    if (key == KeyCode.LeftArrow)
                        return true;
                    else
                        return false;
                }
            case TUTORIAL_STEP.TUTORIAL_STEP13_BEGIN:
            case TUTORIAL_STEP.TUTORIAL_STEP13_ING:
            case TUTORIAL_STEP.TUTORIAL_STEP13_END:
            case TUTORIAL_STEP.TUTORIAL_STEP14_BEGIN:
            case TUTORIAL_STEP.TUTORIAL_STEP14_ING:
            case TUTORIAL_STEP.TUTORIAL_STEP14_END:
                //아래 쪽으로만 이동 가능
                {
                    if (key == KeyCode.DownArrow)
                        return true;
                    else
                        return false;
                }
        }

        return true;
    }

    void CheckEvent()
    {
        if (m_nDiceState == GameData.GAME_DICE_READY)
        {
            /* 기존 Input Check
			if( m_EventQueue.Count > 0 )
            {
                if (m_sCursor.getState() == GameData.OBJECT_STATE_NONE)
                {
                    GameEvent curEvent = (GameEvent)m_EventQueue.Dequeue();
                    StartEvent(curEvent);
                }
			}*/


            if (m_Event != null)
            {
                lock (m_Event)
                {
                    if (m_sCursor.getState() == GameData.OBJECT_STATE_NONE)
                    {
                        StartEvent(m_Event);
                    }
                }
            }
        }
    }

    bool CheckBoard()
    {
        if (CMainData.getGameMode() == GameData.GAME_MODE_BASIC)
        {
            return m_sBoard.IsFull();
        }

        return false;
    }

    void CheckTimer()
    {
        //주사위 생성 시간 검사
        CheckCreateDiceTime();

        //블리츠 종료 시간 검사
        if (CMainData.getGameMode() == GameData.GAME_MODE_BLITZ)
        {
            CheckBlitzTime();
        }
        else if (CMainData.getGameMode() == GameData.GAME_MODE_BASIC)
        {
            CTimeManager.m_TimeStampBasic += Time.deltaTime;
        }
    }

    void CheckCreateDiceTime()
    {
        CTimeManager.m_TimeStampCreateDice += Time.deltaTime;
        if (CTimeManager.m_TimeStampCreateDice >= CTimeManager.m_TimeCreateDice)
        {
            CTimeManager.m_TimeStampCreateDice = 0.0f;
            m_sBoard.CreateDice(false);
        }
    }

    void CheckBlitzTime()
    {
        CTimeManager.m_TimeStampBlitz += Time.deltaTime;
        if (CTimeManager.m_TimeStampBlitz >= CTimeManager.m_TimeBlitz)
        {
            CTimeManager.m_TimeStampBlitz = 0.0f;
            //time up
            CMainData.setGameState(GameData.GAME_STATE_TIMEUP);
        }
    }

    void StartEvent(GameEvent curEvent)
    {
        m_CurEvent = curEvent;

        if (curEvent.target == GameData.EVENT_TARGET_CURSOR)
        {
            if (curEvent.type == GameData.EVENT_TYPE_MOVE)
            {
                if (CursorMoveOk(m_oCursor.transform.position, curEvent.direction) == true)
                {
                    m_nDiceState = GameData.GAME_DICE_EVENT;
                    m_sCursor.MoveCursor(curEvent.direction, false);
                }
            }
        }
        else if (curEvent.target == GameData.EVENT_TARGET_DICE)
        {
            DiceSet diceSet = m_sBoard.GetDice(m_oCursor.transform.position);
            if (diceSet == null)
            {
                if (CursorMoveOk(m_oCursor.transform.position, curEvent.direction) == true)
                {
                    m_CurEvent.target = GameData.EVENT_TARGET_CURSOR;
                    m_nDiceState = GameData.GAME_DICE_EVENT;
                    m_sCursor.MoveCursor(curEvent.direction, true);
                }
            }
            else
            {
                if (curEvent.type == GameData.EVENT_TYPE_MOVE)
                {
                    if (diceSet.script.GetState() == GameData.OBJECT_STATE_NONE)
                    {
                        if (DiceMoveOk(m_oCursor.transform.position, curEvent.direction) == true)
                        {
                            m_nDiceState = GameData.GAME_DICE_EVENT;
                            m_sCursor.MoveCursorNoEvent(curEvent.direction, true);
                            diceSet.script.MoveDice(curEvent.direction);
                            m_sBoard.MoveDice(m_oCursor.transform.position, curEvent.direction, diceSet);
                        }
                    }
                }
            }
        }
    }

    public void OnMoveEnd(Vector3 pos)
    {
        //HanK
        if (gameSceneUI)
        {
            gameSceneUI.Change_DiceInfo(m_sBoard.GetDice(m_oCursor.transform.position));
        }

        m_nDiceState = GameData.GAME_DICE_READY;
        if (m_CurEvent.target == GameData.EVENT_TARGET_DICE)
        {
            m_lScore += m_sBoard.CheckDices(pos);
            if (gameSceneUI)
            {
                gameSceneUI.Change_Score(m_lScore);
            }

            if (CMainData.getGameMode() == GameData.GAME_MODE_BASIC)
            {
                CLevelManager.CheckStage(m_lScore);
            }
        }

        if (CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL)
        {
            TutorialMoveEnd(pos);
        }
    }

    public void OnAppearEnd(Vector3 pos)
    {

    }

    public void OnDisappearEnd(Vector3 pos)
    {
        m_sBoard.DestroyDice(pos);

        if (gameSceneUI)
        {
            if (pos.x == m_oCursor.transform.position.x && pos.z == m_oCursor.transform.position.z)
                gameSceneUI.Change_DiceInfo(m_sBoard.GetDice(m_oCursor.transform.position));
        }
    }

    bool CursorMoveOk(Vector3 pos, int direction)
    {
        if (direction == GameData.EVENT_DIRECTION_RIGHT)
        {
            if (pos.x + 1 < GameData.BOARD_SIZE_WIDTH)
            {
                return true;
            }
        }
        else if (direction == GameData.EVENT_DIRECTION_LEFT)
        {
            if (pos.x - 1 >= 0)
            {
                return true;
            }
        }
        else if (direction == GameData.EVENT_DIRECTION_UP)
        {
            if (pos.z + 1 < GameData.BOARD_SIZE_HEIGHT)
            {
                return true;
            }
        }
        else if (direction == GameData.EVENT_DIRECTION_DOWN)
        {
            if (pos.z - 1 >= 0)
            {
                return true;
            }
        }

        return false;
    }

    bool DiceMoveOk(Vector3 pos, int direction)
    {
        if (direction == GameData.EVENT_DIRECTION_RIGHT)
        {
            if (pos.x + 1 < GameData.BOARD_SIZE_WIDTH && m_sBoard.GetDice(pos.x + 1, pos.z) == null)
            {
                return true;
            }
        }
        else if (direction == GameData.EVENT_DIRECTION_LEFT)
        {
            if (pos.x - 1 >= 0 && m_sBoard.GetDice(pos.x - 1, pos.z) == null)
            {
                return true;
            }
        }
        else if (direction == GameData.EVENT_DIRECTION_UP)
        {
            if (pos.z + 1 < GameData.BOARD_SIZE_HEIGHT && m_sBoard.GetDice(pos.x, pos.z + 1) == null)
            {
                return true;
            }
        }
        else if (direction == GameData.EVENT_DIRECTION_DOWN)
        {
            if (pos.z - 1 >= 0 && m_sBoard.GetDice(pos.x, pos.z - 1) == null)
            {
                return true;
            }
        }

        return false;
    }

    bool CheckControlState()
    {
        if (CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL)
        {

        }

        return true;
    }

    void ClearState()
    {
        m_nKeyState = GameData.KEY_STATE_CURSOR;
        m_nDiceState = GameData.GAME_DICE_READY;

        m_Event = null;
        m_sCursor.Clear();
        CScoreManager.m_nMaxChain = 0;
        m_sBoard.initBoards();
        // 기존 Input
        //m_EventQueue.Clear();
        // 새로운 Input
    }

    void TutorialMoveEnd(Vector3 pos)
    {
        //Debug.Log("TUTORIAL MOVE END " + "X = " + pos.x + "," + "Z = " + pos.z + "," + "STATE = " + m_nTutorialState);
        switch (m_nTutorialState)
        {
            case TUTORIAL_STEP.TUTORIAL_STEP13_ING:
                {
                    if (pos.x == 5 && pos.z == 1)
                    {
                        //Debug.Log("TUTORIAL MOVE END CLEAR " + "X = " + pos.x + "," + "Z = " + pos.z);
                        DiceSet _diceSet = m_sBoard.GetDice(5, 1);
                        if (_diceSet != null)
                        {
                            CKeyManager.ControlOff();
                            _diceSet.script.DisappearDice();
                            m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP13_END;  
                        }
                    }                    
                }
                break;

            case TUTORIAL_STEP.TUTORIAL_STEP14_ING:
                {
                    if (pos.x == 5 && pos.z == 1)
                    {
                        //Debug.Log("TUTORIAL MOVE END CLEAR " + "X = " + pos.x + "," + "Z = " + pos.z);
                        DiceSet _diceSet = m_sBoard.GetDice(5, 1);
                        if (_diceSet != null)
                        {
                            CKeyManager.ControlOff();
                            tutorialUI.NextStep();
                            _diceSet.script.DisappearDice();
                            m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP14_END;
                        }
                    }
                }
                break;
        }
    }

    void TutorialMainProcess()
    {
        //if (TutorialUI.g_instance != null)
        //{
        //    TutorialUI.g_instance.ChangeTutorialUI_ForStep((int)m_nTutorialState);
        //}

        switch (m_nTutorialState)
        {
            case TUTORIAL_STEP.TUTORIAL_STEP1_BEGIN:
                CKeyManager.ControlOff();
				m_sCursor.transform.position = new Vector3(2.0f, 0.0f, 3.0f);
                m_Star.SetActive(true);
                m_Star.transform.position = new Vector3(5.0f, -0.45f, 1.0f);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP1_ING;
                tutorialUI.NextStep();

                break;
            case TUTORIAL_STEP.TUTORIAL_STEP1_ING:
                CKeyManager.ControlTimeCheck();
                if (m_oCursor.transform.position == new Vector3(5.0f, 0.0f, 1.0f))
                {
                    m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP1_END;
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP1_END:
                CKeyManager.ControlOff();
                ClearState();
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP2_BEGIN;
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP2_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();
                m_Star.SetActive(true);
                m_sCursor.transform.position = new Vector3(2.0f, 0.0f, 3.0f);
                m_Star.transform.position = new Vector3(5.0f, -0.45f, 1.0f);
                m_sBoard.CreateDice(3, 3, 1,true);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP2_ING;
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP2_ING:
                {
                    CKeyManager.ControlTimeCheck();
                    DiceSet _diceSet = m_sBoard.GetDice(5, 1);
                    if (_diceSet != null)
                    {
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP2_END;
                    }
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP2_END:
                {
                    CKeyManager.ControlOff();
                    ClearState();
                    m_sBoard.RemoveDice(5, 1);
                    m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP6_BEGIN;
                    CKeyManager.ControlOff();
                }
                break;

            //주사위 2 클리어 
            case TUTORIAL_STEP.TUTORIAL_STEP6_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();
                m_Star.SetActive(true);
                m_Arrow.SetActive(true);
                m_sCursor.transform.position = new Vector3(7.0f, 0.0f, 3.0f);
                m_Star.transform.position = new Vector3(2.0f, -0.45f, 3.0f);
                m_Arrow.transform.position = new Vector3(4.0f, -0.45f, 3.0f);

                m_sBoard.CreateDice(2, 4, 2, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 3, 2, true).script.setMoveable(true);

                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP6_ING;
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP6_ING:
                {
                    CKeyManager.ControlTimeCheck();
                    DiceSet _diceSet = m_sBoard.GetDice(2, 3);
                    if (_diceSet != null)
                    {
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP6_END;
                    }
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP6_END:
                {
                    if (m_sBoard.IsAlmostEmpty())
                    {
                        CKeyManager.ControlOff();
                        Debug.Log("TUTORIAL_STEP6_END");
                        //모든 주사위 내려가면 상태 변경
                        ClearState();
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP7_BEGIN;
                    }
                }
                break;
            //주사위 3 클리어 
            case TUTORIAL_STEP.TUTORIAL_STEP7_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();
                m_Star.SetActive(true);
                m_Arrow.SetActive(true);
                m_sCursor.transform.position = new Vector3(7.0f, 0.0f, 3.0f);
                m_Star.transform.position = new Vector3(2.0f, -0.45f, 3.0f);
                m_Arrow.transform.position = new Vector3(4.0f, -0.45f, 3.0f);
                m_sBoard.CreateDice(2, 5, 3, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 4, 3, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 3, 3, true).script.setMoveable(true);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP7_ING;

                break;
            case TUTORIAL_STEP.TUTORIAL_STEP7_ING:
                {
                    CKeyManager.ControlTimeCheck();
                    DiceSet _diceSet = m_sBoard.GetDice(2, 3);
                    if (_diceSet != null)
                    {
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP7_END;
                    }
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP7_END:
                    if (m_sBoard.IsAlmostEmpty())
                    {
                        CKeyManager.ControlOff();
                        //모든 주사위 내려가면 상태 변경
                        ClearState();
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP8_BEGIN;
                    }
                break;

            //주사위 4 클리어 
            case TUTORIAL_STEP.TUTORIAL_STEP8_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();
                m_Star.SetActive(true);
                m_Arrow.SetActive(true);
                m_sCursor.transform.position = new Vector3(7.0f, 0.0f, 3.0f);
                m_Star.transform.position = new Vector3(2.0f, -0.45f, 3.0f);
                m_Arrow.transform.position = new Vector3(4.0f, -0.45f, 3.0f);
                m_sBoard.CreateDice(2, 6, 4, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 5, 4, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 4, 4, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 3, 4, true).script.setMoveable(true);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP8_ING;

                break;
            case TUTORIAL_STEP.TUTORIAL_STEP8_ING:
                {
                    CKeyManager.ControlTimeCheck();
                    DiceSet _diceSet = m_sBoard.GetDice(2, 3);
                    if (_diceSet != null)
                    {
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP8_END;
                    }
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP8_END:
                {
                    if (m_sBoard.IsAlmostEmpty())
                    {
                        CKeyManager.ControlOff();
                        //모든 주사위 내려가면 상태 변경
                        ClearState();
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP9_BEGIN;
                    }
                }
                break;

            //주사위 5 클리어 
            case TUTORIAL_STEP.TUTORIAL_STEP9_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();            
                m_Star.SetActive(true);
                m_Arrow.SetActive(true);
                m_sCursor.transform.position = new Vector3(7.0f, 0.0f, 3.0f);
                m_Star.transform.position = new Vector3(2.0f, -0.45f, 3.0f);
                m_Arrow.transform.position = new Vector3(4.0f, -0.45f, 3.0f);
                m_sBoard.CreateDice(2, 7, 5, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 6, 5, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 5, 5, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 4, 5, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 3, 5, true).script.setMoveable(true);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP9_ING;

                break;
            case TUTORIAL_STEP.TUTORIAL_STEP9_ING:
                {
                    CKeyManager.ControlTimeCheck();

                    DiceSet _diceSet = m_sBoard.GetDice(2, 3);
                    if (_diceSet != null)
                    {
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP9_END;
                    }
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP9_END:
                {
                    if (m_sBoard.IsAlmostEmpty())
                    {
                        //모든 주사위 내려가면 상태 변경
                        CKeyManager.ControlOff();
                        ClearState();
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP10_BEGIN;
                    }
                }
                break;

            //주사위 6 클리어
            case TUTORIAL_STEP.TUTORIAL_STEP10_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();
                m_Star.SetActive(true);
                m_Arrow.SetActive(true);
                m_sCursor.transform.position = new Vector3(7.0f, 0.0f, 3.0f);
                m_Star.transform.position = new Vector3(2.0f, -0.45f, 3.0f);
                m_Arrow.transform.position = new Vector3(4.0f, -0.45f, 3.0f);
                m_sBoard.CreateDice(2, 8, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 7, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 6, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 5, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 4, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 3, 6, true).script.setMoveable(true);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP10_ING;
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP10_ING:
                {
                    CKeyManager.ControlTimeCheck();
                    DiceSet _diceSet = m_sBoard.GetDice(2, 3);
                    if (_diceSet != null)
                    {
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP10_END;
                    }
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP10_END:
                {
                    if (m_sBoard.IsAlmostEmpty())
                    {
                        //모든 주사위 내려가면 상태 변경
                        CKeyManager.ControlOff();
                        ClearState();
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP11_BEGIN;
                    }
                }
                break;

            //체인 설명
            case TUTORIAL_STEP.TUTORIAL_STEP11_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();
                m_Star.SetActive(true);
                m_Arrow.SetActive(true);
                m_Star2.SetActive(true);
                m_Arrow2.SetActive(true);

                m_sCursor.transform.position = new Vector3(7.0f, 0.0f, 3.0f);
                m_Star.transform.position = new Vector3(2.0f, -0.45f, 3.0f);
                m_Arrow.transform.position = new Vector3(4.0f, -0.45f, 3.0f);
                m_Star2.transform.position = new Vector3(2.0f, -0.45f, 2.0f);
                m_Arrow2.transform.position = new Vector3(4.0f, -0.45f, 2.0f);

                m_sBoard.CreateDice(2, 8, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 7, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 6, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 5, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(2, 4, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 3, 6, true).script.setMoveable(true);
                m_sBoard.CreateDice(6, 2, 6, true).script.setMoveable(false);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP11_ING;
                m_nTutorailSubStep = TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP1;

                break;
            case TUTORIAL_STEP.TUTORIAL_STEP11_ING:
                {
                    CKeyManager.ControlTimeCheck();

                    switch (m_nTutorailSubStep)
                    {
                        case TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP1:
                            DiceSet _diceSet = m_sBoard.GetDice(2, 3);
                            if (_diceSet != null)
                            {
                                //DiceSet _diceSet = m_sBoard.GetDice(2, 3);
                                m_nTutorailSubStep = TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP2;
                                m_sBoard.GetDice(6, 2).script.setMoveable(true);
                            }
                            break;

                        case TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP2:

                            if (CScoreManager.m_nMaxChain >= 1)
                            {
                                m_nTutorailSubStep = TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP_NONE;
                                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP11_END;
                            }
                            break;
                    }
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP11_END:
                {
                    if (m_sBoard.IsAlmostEmpty())
                    {
                        //모든 주사위 내려가면 상태 변경
                        CKeyManager.ControlOff();
                        ClearState();
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP12_BEGIN;
                    }
                }
                break;

            //주사위 1 설명
            case TUTORIAL_STEP.TUTORIAL_STEP12_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();
                m_Star.SetActive(false);
                m_Star2.SetActive(false);
                m_Arrow.SetActive(false);
                m_Arrow2.SetActive(false);

                m_Star.SetActive(true);
                m_Arrow.SetActive(true);
                
                m_Star.transform.position = new Vector3(0.0f, -0.45f, 1.0f);
                m_Arrow.transform.position = new Vector3(2.0f, -0.45f, 1.0f);
                

                m_sCursor.transform.position = new Vector3(7.0f, 0.0f, 3.0f);
                //m_sBoard.CreateDice(0, 0, 6, true).script.setMoveable(false);
                //m_sBoard.CreateDice(1, 0, 6, true).script.setMoveable(false);
                //m_sBoard.CreateDice(2, 0, 6, true).script.setMoveable(false);
                //m_sBoard.CreateDice(3, 0, 6, true).script.setMoveable(false);

                m_sBoard.CreateDice(0, 2, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(0, 3, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(0, 4, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(0, 5, 6, true).script.setMoveable(false);
                m_sBoard.CreateDice(0, 6, 6, true).script.setMoveable(false);

                m_sBoard.CreateDice(3, 2, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(3, 3, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(3, 4, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(3, 5, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(3, 6, 1, true).script.setMoveable(false);

                m_sBoard.CreateDice(4, 2, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(4, 3, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(4, 4, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(4, 5, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(4, 6, 1, true).script.setMoveable(false);

                m_sBoard.CreateDice(5, 2, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(5, 3, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(5, 4, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(5, 5, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(5, 6, 1, true).script.setMoveable(false);

                m_sBoard.CreateDice(6, 2, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 3, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 4, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 5, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(6, 6, 1, true).script.setMoveable(false);
                m_sBoard.CreateDice(4, 1, 6, true).script.setMoveable(true);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP12_ING;
                m_nTutorailSubStep = TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP1;

                break;
            case TUTORIAL_STEP.TUTORIAL_STEP12_ING:
                {
                    CKeyManager.ControlTimeCheck();

                    switch (m_nTutorailSubStep)
                    {
                        case TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP1:
                            DiceSet _diceSet = m_sBoard.GetDice(0, 1);
                            if (_diceSet != null)
                            {
                                m_nTutorailSubStep = TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP2;

                                m_sBoard.GetDice(3, 2).script.setMoveable(true);
                                m_sBoard.GetDice(3, 3).script.setMoveable(true);
                                m_sBoard.GetDice(3, 4).script.setMoveable(true);
                                m_sBoard.GetDice(3, 5).script.setMoveable(true);
                                m_sBoard.GetDice(3, 6).script.setMoveable(true);

                                m_sBoard.GetDice(4, 2).script.setMoveable(true);
                                m_sBoard.GetDice(4, 3).script.setMoveable(true);
                                m_sBoard.GetDice(4, 4).script.setMoveable(true);
                                m_sBoard.GetDice(4, 5).script.setMoveable(true);
                                m_sBoard.GetDice(4, 6).script.setMoveable(true);

                                m_sBoard.GetDice(5, 2).script.setMoveable(true);
                                m_sBoard.GetDice(5, 3).script.setMoveable(true);
                                m_sBoard.GetDice(5, 4).script.setMoveable(true);
                                m_sBoard.GetDice(5, 5).script.setMoveable(true);
                                m_sBoard.GetDice(5, 6).script.setMoveable(true);

                                m_sBoard.GetDice(6, 2).script.setMoveable(true);
                                m_sBoard.GetDice(6, 3).script.setMoveable(true);
                                m_sBoard.GetDice(6, 4).script.setMoveable(true);
                                m_sBoard.GetDice(6, 5).script.setMoveable(true);
                                m_sBoard.GetDice(6, 6).script.setMoveable(true);
                            }
                            break;

                        case TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP2:

                            if (CScoreManager.m_nMaxChain >= 10)
                            {
                                m_nTutorailSubStep = TUTORIAL_SUB_STEP.TUTORIAL_SUB_STEP_NONE;
                                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP12_END;
                            }
                            break;
                    }
                }

                break;
            case TUTORIAL_STEP.TUTORIAL_STEP12_END:
                {
                    //if (m_sBoard.IsEmpty())
                    {
                        //모든 주사위 내려가면 상태 변경
                        CKeyManager.ControlOff();
                        ClearState();
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP13_BEGIN;
                    }
                }
                break;

            //주사위 팁
            case TUTORIAL_STEP.TUTORIAL_STEP13_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();

                m_Star.SetActive(false);
                m_Star2.SetActive(false);
                m_Arrow.SetActive(false);
                m_Arrow2.SetActive(false);

                m_Star.SetActive(true);
                m_Star.transform.position = new Vector3(5.0f, -0.45f, 1.0f);
                m_sCursor.transform.position = new Vector3(7.0f, 0.0f, 3.0f);
                m_sBoard.CreateDice(5, 3, 1, true);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP13_ING;
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP13_ING:
                {
                    CKeyManager.ControlTimeCheck();
                    //DiceSet _diceSet = m_sBoard.GetDice(5, 1);
                    //if (_diceSet != null)
                    //{
                    //    if (_diceSet.script.GetState() == GameData.OBJECT_STATE_NONE)
                    //    {
                    //        _diceSet.script.DisappearDice();
                    //        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP13_END;
                    //    }
                    //}                
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP13_END:
                {
                    if (m_sBoard.IsAlmostEmpty())
                    {
                        //모든 주사위 내려가면 상태 변경
                        CKeyManager.ControlOff();
                        ClearState();
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP14_BEGIN;
                    }
                }
                break;

            case TUTORIAL_STEP.TUTORIAL_STEP14_BEGIN:
                tutorialUI.NextStep();
                CKeyManager.ControlOff();
                m_Star.SetActive(true);
                m_Star.transform.position = new Vector3(5.0f, -0.45f, 1.0f);
                m_sCursor.transform.position = new Vector3(7.0f, 0.0f, 3.0f);
                m_sBoard.CreateDice(5, 3, 2, true);
                m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP14_ING;

                break;
            case TUTORIAL_STEP.TUTORIAL_STEP14_ING:
                {
                    CKeyManager.ControlTimeCheck();
                    
                    //DiceSet _diceSet = m_sBoard.GetDice(5, 1);
                    //if (_diceSet != null)
                    //{
                    //    CKeyManager.ControlOff();
                    //    if (_diceSet.script.GetState() == GameData.OBJECT_STATE_NONE)
                    //    {
                    //        tutorialUI.NextStep();
                    //        _diceSet.script.DisappearDice();
                    //        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP14_END;
                    //    }
                    //}     
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP14_END:
                {
                    if (m_sBoard.IsAlmostEmpty())
                    {
                        tutorialUI.TutorialComplete();
                        m_nTutorialState = TUTORIAL_STEP.TUTORIAL_STEP15_BEGIN;

                    }
                }
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP15_BEGIN:
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP15_ING:
                break;
            case TUTORIAL_STEP.TUTORIAL_STEP15_END:
                break;

            default:
                break;
        }
    }
}
