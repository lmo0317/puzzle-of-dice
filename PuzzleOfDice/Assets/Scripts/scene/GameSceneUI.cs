using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSceneUI : MonoBehaviour
{

    public GameObject Pause_Menu;
    public UILabel label_record;
    public UILabel label_score;
    public UILabel label_time;
    public UILabel label_diceCount;
    public UILabel label_gold;
    public UILabel label_dice_time;
    public double soundTime;
    long c_score;
    long g_score;
    Queue<long> change_score = new Queue<long>();

    /* Music, Sound Button */
    public UISprite musicButton;
    public UISprite soundButton;

    /* Ready Go UI */
    public GameObject ready_back;
    public UILabel t_ready;
    public UILabel t_go;

    private const double soundTargetTime = 0.2;


    /* Dice Info */
    public GameObject diceInfo;
    private DiceSet currentDice = null;

    // Use this for initialization
    void Start()
    {
        //CMainData.setGameMode(GameData.GAME_MODE_BLITZ); //set game mode
        //CMainData.setGameState(GameData.GAME_STATE_READY); //set game state

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

        if (label_record != null)
            label_record.text = CMainData.UserScore.ToString();

        t_ready.enabled = false;
        t_go.enabled = false;

        if (CMainData.Dice_Time != -1 && CMainData.Dice_Count < 5)
        {
            if (label_dice_time != null)
                label_dice_time.text = ((int)(CDefine.DICE_RECOVERYTIME / 60 - CMainData.Dice_Time / 60)).ToString(@"00") + ":" + ((int)((CDefine.DICE_RECOVERYTIME - CMainData.Dice_Time) % 60)).ToString(@"00");
        }
        else
        {
            if (label_dice_time != null)
                label_dice_time.text = CDefine.DICE_FULL;
        }

        NGUITools.SetActive(ready_back, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (CMainData.Dice_Time != -1 && CMainData.Dice_Count < 5)
        {
            CMainData.Dice_Time += Time.deltaTime;
            if (CMainData.Dice_Time > CDefine.DICE_RECOVERYTIME)
            {
                /*
                if (Game_ServerConnection.g_instance != null)
                {
                    Game_ServerConnection.g_instance.SendDiceIncrease();
                }
                */
                if (ServerConnection.g_instance != null)
                {
                    ServerConnection.g_instance.SendDiceIncrease();
                }
            }
        }


        if (Application.loadedLevelName.Equals("GameScene"))
        {
            if (label_diceCount != null)
                label_diceCount.text = CMainData.Dice_Count.ToString();
            if (CMainData.Dice_Count < 5)
            {
                if (CMainData.Dice_Time > 0)
                {
                    if (label_dice_time != null)
                        label_dice_time.text = ((int)(CDefine.DICE_RECOVERYTIME / 60 - CMainData.Dice_Time / 60)).ToString(@"00") + ":" + ((int)((CDefine.DICE_RECOVERYTIME - CMainData.Dice_Time) % 60)).ToString(@"00");
                }
                else
                {
                    if (label_dice_time != null)
                        label_dice_time.text = "00:00";
                }
            }
            else
            {
                if (label_dice_time != null)
                    label_dice_time.text = CDefine.DICE_FULL;
            }


            if (label_gold != null)
                label_gold.text = CMainData.Gold.ToString();

            //score sound
            if (change_score.Count > 0)
            {
                g_score = change_score.Dequeue();
                //soundTime = soundTargetTime;
            }

            if (g_score - c_score > 100)
            {
                c_score += (g_score - c_score) / 50;
            }
            else if (g_score > c_score)
            {
                c_score += 1;
            }

            //if (g_score > c_score)
            //{
            //    soundTime += Time.deltaTime;
            //    if (soundTime > soundTargetTime)
            //    {
            //        soundTime = 0;
            //        SoundManager.g_Instance.PlayEffectSound("score_adding_01");
            //    }
            //}

            if (label_score != null)
                label_score.text = c_score.ToString();
        }
    }

    void OnGUI()
    {
        //label_time.text = ((int)(CTimeManager.m_TimeStampBlitz / 60)).ToString(@"00") + ":" + ((int)(CTimeManager.m_TimeStampBlitz % 60)).ToString(@"00");

        if (label_time != null)
            label_time.text = ((int)(GameData.TIMER_GAMETIME_BLITZ / 60 - CTimeManager.m_TimeStampBlitz / 60)).ToString(@"00") + ":" + ((int)((GameData.TIMER_GAMETIME_BLITZ - CTimeManager.m_TimeStampBlitz) % 60)).ToString(@"00");



        if (currentDice != null)
        {
            if (currentDice.dice != null)
            {
                Quaternion qt = currentDice.dice.transform.rotation;
                //Debug.Log("x is " + qt.x.ToString() + ", y is " + qt.y.ToString() + ", z is " + qt.z.ToString());
                //qt.x += 0.01f;
                //qt.eulerAngles += new Vector3(0.0f, 0.0f, -10.0f);
                if(diceInfo != null)
                    diceInfo.transform.rotation = qt;
            }
        }
    }

    public void Change_Score(long score)
    {
        if(change_score != null)
            change_score.Enqueue(score);
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

    public void MusicButton()
    {
        Debug.Log("Onclick Music Button.");

        CMainData.Music = !CMainData.Music;

        ButtonClickSound();

        if (CMainData.Music)
        {
            if (musicButton != null)
                musicButton.spriteName = "MS_menu_btn_music_on";
            SoundManager.g_Instance.ChangeMusicState();
        }
        else
        {
            if (musicButton != null)
                musicButton.spriteName = "MS_menu_btn_music_off";
            SoundManager.g_Instance.ChangeMusicState();
        }
    }

    public void SoundButton()
    {
        Debug.Log("Onclick Sound Button.");

        CMainData.Sound = !CMainData.Sound;

        ButtonClickSound();

        if (CMainData.Sound)
        {
            if (soundButton != null)
                soundButton.spriteName = "MS_menu_btn_sound_on";
            //soundButton.mainTexture = main_sound_on;
        }
        else
        {
            if (soundButton != null)
                soundButton.spriteName = "MS_menu_btn_sound_off";
            //soundButton.mainTexture = main_sound_off;
        }
    }

    public void ChangeState(int gameState)
    {
        if (gameState == GameData.GAME_STATE_READY)
        {
            //NGUITools.SetActive(t_ready.gameObject, true);
            Debug.Log("UI CHANGE STATE READY");
            t_ready.enabled = true;
            NGUITools.SetActive(ready_back, true);
            SoundManager.g_Instance.PlayBGM("BGM_ReadyGo_01", false);
        }
        else if (gameState == GameData.GAME_STATE_START)
        {
            //NGUITools.SetActive(t_ready.gameObject, false);
            //NGUITools.SetActive(t_go.gameObject, true);
            t_ready.enabled = false;
            t_go.enabled = true;

            //Debug.Log("UI CHANGE STATE START");
        }
        else if (gameState == GameData.GAME_STATE_GAMEING)
        {
            NGUITools.SetActive(t_go.gameObject, false);
            NGUITools.SetActive(ready_back, false);

            MainGame.g_Instance.setSoundState(SOUND_STATE.SOUND_STATE_GAME_INTRO_READY);
        }
    }

    public void Change_DiceInfo(DiceSet currentDice)
    {
        if (currentDice == null)
        {
            NGUITools.SetActive(diceInfo, false);
            diceInfo.SetActive(false);
            this.currentDice = null;
        }
        else
        {
            NGUITools.SetActive(diceInfo, true);
            diceInfo.SetActive(true);

            this.currentDice = currentDice;
        }
    }

    public static void MenuOpenSound()
    {
        if (SoundManager.g_Instance != null)
            SoundManager.g_Instance.PlayEffectSound("menu_open_01");
    }

    public static void MenuCloseSound()
    {
        if (SoundManager.g_Instance != null)
            SoundManager.g_Instance.PlayEffectSound("menu_close_01");
    }
}