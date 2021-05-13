using UnityEngine;
using System.Collections;

public class ToolTipObj : MonoBehaviour {

    public string key = null;

    public UILabel name = null;
    public UISprite isDisableCheck = null;

    void OnTooltip(bool show)
    {
        Debug.Log("Tool Tip " + show);

        if (show && key != null)
        {
            string t = StringData.getString(key);

            if (name != null)
            {
                t = t.Replace("%s", name.text);
            }

            if (isDisableCheck != null && isDisableCheck.gameObject.activeSelf)
            {
                UITooltip.ShowText(null);
                return;
            }

            UITooltip.ShowText(t);
            return;
        }
        else
            UITooltip.ShowText(null);
    }
}
