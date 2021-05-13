using UnityEngine;
using System.Collections;

public class GameOverRestartDicenohave : MonoBehaviour {

    public UILabel label_gold;
    public UILabel label_dice_time;

	// Use this for initialization
	void Start () {
        if(label_gold != null)
            label_gold.text = CMainData.Gold.ToString();
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
	}
	
	// Update is called once per frame
	void Update () {
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
    }
}
