using UnityEngine;
using System.Collections;

public class ScoreLabel : MonoBehaviour {
    
    private float duringTime;
 
    private TextMesh text;

	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent("TextMesh") as TextMesh;
        duringTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        duringTime += Time.deltaTime;

        gameObject.transform.position += new Vector3(0.0f, GameData.SCORE_LABEL_upY * Time.deltaTime, 0.0f);
        gameObject.transform.localScale -= new Vector3(GameData.SCORE_LABEL_scale * Time.deltaTime, GameData.SCORE_LABEL_scale * Time.deltaTime, GameData.SCORE_LABEL_scale * Time.deltaTime);

        text.color -= new Color(0, 0, 0, GameData.SCORE_LABEL_alpha * Time.deltaTime);

        if (duringTime > GameData.SCORE_LABEL_endTime)
        {
            Destroy(gameObject);
            Destroy(this);
        }
	}
}
