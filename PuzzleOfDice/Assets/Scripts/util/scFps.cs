// FPS information

using UnityEngine;
using System.Collections;

public class scFps : MonoBehaviour {
	
	public int xPos = 10;
	public int yPos = 10;
	public float updateInterval = 0.5f;
	private float lastInterval;
	private int frames = 0;
	private float fps;
	
	// Use this for initialization
	void Start () {
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}
	
	// Update is called once per frame
	void Update () {
		++frames;
		float timeNow = Time.realtimeSinceStartup;
		if( timeNow > lastInterval + updateInterval ){
			fps = frames / ( timeNow - lastInterval );
			frames = 0;
			lastInterval = timeNow;
		}
	}
	
	void OnGUI () {
		//GUILayout.Label( "FPS : " + fps.ToString("f2") );
		GUI.Label( new Rect( xPos , yPos , 200 , 30 ) , "FPS : " + fps.ToString("f2") );
	}
}
