using UnityEngine;
using System.Collections;

public class ButtonHover : MonoBehaviour {

    private bool isOver = false;
    public string hoverSound = "";

    void Start()
    {
        isOver = false;
    }

    void OnPress(bool isPressed)
    {
        if (enabled)
        {
            Debug.Log("Press " + isPressed);
        }
    }

    void OnHover(bool isOver)
    {
        if (enabled)
        {
            //Debug.Log("Hover " + isOver);
            if (this.isOver != isOver)
            {
                if (isOver && !hoverSound.Equals(""))
                {
                    SoundManager.g_Instance.PlayEffectSound(hoverSound);
                }

                if (isOver)
                {
                    CustomMouseCursor.isButton = true;
                }
                else
                {
                    CustomMouseCursor.isButton = false;
                }

                this.isOver = isOver;
            }
        }
    }
}
