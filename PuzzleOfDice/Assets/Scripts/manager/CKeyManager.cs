using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CKeyManager
{
    public static int m_nKeyState = GameData.KEY_STATE_ON;
    public static float tutorialStateChangeTime = 0;
    public static bool IsKeyEnable()
    {
        if (m_nKeyState == GameData.KEY_STATE_ON)
        {
            //Debug.Log("Key Success");
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ControlTimeCheck()
    {
        if (m_nKeyState == GameData.KEY_STATE_OFF)
        {
            tutorialStateChangeTime += Time.deltaTime;
            if (tutorialStateChangeTime > GameData.TUTORIAL_STATE_CHANGE_TIME)
            {
                m_nKeyState = GameData.KEY_STATE_ON;
                tutorialStateChangeTime = 0;
            }
        }
    }

    public static void ControlOff()
    {
        //Debug.Log("Control Off");
        m_nKeyState = GameData.KEY_STATE_OFF;
        tutorialStateChangeTime = 0;
    }

    public static void ControlOn()
    {
        m_nKeyState = GameData.KEY_STATE_ON;
        tutorialStateChangeTime = 0;
    }
}