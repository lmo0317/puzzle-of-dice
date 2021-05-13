using UnityEngine;
using System.Collections;

public class ScoreLabelObj : MonoBehaviour {

    public static ScoreLabelObj instance = null;

    public GameObject cursorObj;
    public GameObject labelPrefab;

	// Use this for initialization
	void Start () {
	    instance = this;
	}
	
    public void createScoreLabel(long score) {

        if (labelPrefab == null)
            return;

        GameObject temp = GameObject.Instantiate(labelPrefab, cursorObj.transform.position, Quaternion.identity) as GameObject;
        temp.transform.parent = gameObject.transform;
        temp.transform.localRotation = new Quaternion(0, 0, 0, 0);
        TextMesh text = temp.GetComponent("TextMesh") as TextMesh;
        text.text = score.ToString();
    }
}
