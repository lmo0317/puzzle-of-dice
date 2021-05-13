using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    public static GameControl g_Instance;

    public UITexture ui_keyboard;
    public UITexture ui_mouse;

    private void stateChange()
    {
        /*
        if (CMainData.Control_Method == GameData.GAME_CONTROL_KEY)
        {
            NGUITools.SetActive(ui_keyboard.gameObject, true);
            NGUITools.SetActive(ui_mouse.gameObject, false);
        }
        else
        {
            NGUITools.SetActive(ui_keyboard.gameObject, false);
            NGUITools.SetActive(ui_mouse.gameObject, true);
        }
        */


    }

	// Use this for initialization
	void Start () {
        g_Instance = this;
        stateChange();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL)
        {
            NGUITools.SetActive(this.gameObject, false);
        }
	}
}
