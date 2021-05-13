using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CTimeManager
{
    public static float m_TimeCreateDice = GameData.TIMER_CREATE_DICE_BASIC; //create dice
    public static float m_TimeStampCreateDice = 0.0f;	//create dice stamp

    public static float m_TimeBlitz = GameData.TIMER_GAMETIME_BLITZ; // blitz time
    public static float m_TimeStampBlitz = 0.0f; //blitz time stamp

    public static float m_TimeStampBasic = 0.0f; //Basic time stamp

    public static float m_TimeDisappearDice = GameData.OBJECT_DISAPPEAR_TIME;
    public static float m_TimeDisappearPlusPercent = GameData.OBJECT_DISAPPEAR_PLUS_PERCENT;

    public static void Init()
    {

    }

    public static void SetTimer()
    {
        if (CMainData.getGameMode() == GameData.GAME_MODE_BLITZ)
        {
            CTimeManager.m_TimeCreateDice = GameData.TIMER_CREATE_DICE_BLITZ;
            m_TimeStampBlitz = 0.0f;
        }
        else if (CMainData.getGameMode() == GameData.GAME_MODE_BASIC)
        {
            CTimeManager.m_TimeCreateDice = GameData.TIMER_CREATE_DICE_BASIC;
            m_TimeStampBasic = 0.0f;
        }
        else if (CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL)
        {
            CTimeManager.m_TimeCreateDice = GameData.TIMER_CREATE_DICE_TUTORIAL;
        }
    }
}