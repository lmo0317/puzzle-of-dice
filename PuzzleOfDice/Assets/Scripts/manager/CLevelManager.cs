using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CLevelManager
{
    public static int m_nStageLevel;
    public static void CheckStage(long nScore)
    {
        m_nStageLevel = (int)(nScore / 100);
        m_nStageLevel = Mathf.Min(m_nStageLevel, 999);
        m_nStageLevel = Mathf.Max(m_nStageLevel, 0);

        if (CMainData.getGameMode() == GameData.GAME_MODE_BASIC)
        {
            CTimeManager.m_TimeCreateDice = (float)(GameData.TIMER_CREATE_DICE_BASIC - ((m_nStageLevel / 10) * 0.1));
            CTimeManager.m_TimeCreateDice = Mathf.Max(CTimeManager.m_TimeCreateDice, GameData.TIMER_CREATE_DICE_MIN);
            
            CTimeManager.m_TimeDisappearDice = (float)(GameData.OBJECT_DISAPPEAR_TIME - ((m_nStageLevel / 10) * 0.2));
            CTimeManager.m_TimeDisappearDice = Mathf.Max(CTimeManager.m_TimeDisappearDice, GameData.OBJECT_DISAPPEAR_TIME_MIN);

            CTimeManager.m_TimeDisappearPlusPercent = (float)(GameData.OBJECT_DISAPPEAR_PLUS_PERCENT - ((m_nStageLevel / 10) * 0.1));
            CTimeManager.m_TimeDisappearPlusPercent = Mathf.Max(CTimeManager.m_TimeDisappearPlusPercent, GameData.OBJECT_DISAPPEAR_PLUS_PERCENT_MIN);
        }
    }
}