using UnityEngine;
using System.Collections;

public class Progress : MonoBehaviour {

    public UITexture loading;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        loading.gameObject.transform.Rotate(0.0f, 0.0f, Time.deltaTime * -100.0f);
	}
}
