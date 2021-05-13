using UnityEngine;
using System.Collections;

public class GameOverNormal : MonoBehaviour {

    public UILabel label_score;
    public UILabel label_maxchain;
    public UILabel label_maxchainscore;
    public UILabel[] label_dice;
    public UILabel label_average;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.loadedLevelName.Equals("SceneGameOver"))
        {
            if (label_score != null)
                label_score.text = SceneGameOver.score.ToString();
            if (label_maxchain != null)
                label_maxchain.text = SceneGameOver.result_maxChain.ToString();
            if (label_maxchainscore != null)
                label_maxchainscore.text = SceneGameOver.result_maxChainScore.ToString();

            for (int i = 0; i < label_dice.Length; i++)
            {
                if (label_dice[i] != null && SceneGameOver.result_DiceN[i] != null)
                    label_dice[i].text = SceneGameOver.result_DiceN[i].ToString();
            }

            if (label_average != null)
            {
                label_average.text = (
                    ((float)SceneGameOver.result_DiceN[2] / SceneGameOver.result_DiceN[0] * 2.0f)
                    + ((float)SceneGameOver.result_DiceN[3] / SceneGameOver.result_DiceN[0] * 3.0f)
                    + ((float)SceneGameOver.result_DiceN[4] / SceneGameOver.result_DiceN[0] * 4.0f)
                    + ((float)SceneGameOver.result_DiceN[5] / SceneGameOver.result_DiceN[0] * 5.0f)
                    + ((float)SceneGameOver.result_DiceN[6] / SceneGameOver.result_DiceN[0] * 6.0f)
                    ).ToString();

                //label_average.text = (SceneGameOver.result_DiceN[0]/6).ToString();
            }
        }
	}
}
