// Device information

using UnityEngine;
using System.Collections;

public class scDevice : MonoBehaviour {
	
	public int xPos = 10;
	public int yPos = 10;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		//screen size
		GUI.Label( new Rect( xPos , yPos, 200, 30 ) , "Screen Size : " + Screen.width + " X " + Screen.height ); 
		//current resolution
		GUI.Label( new Rect( xPos , yPos+20, 400, 200 ) , "resolution : " + Screen.resolutions ); 
		//dpi
		GUI.Label( new Rect( xPos , yPos+40, 200, 200 ) , "dpi : " + Screen.dpi.ToString( "f2" ) ); 
	}
	
	
	//fullscreen
	//Screen.fullScreen = !Screen.fullScreen;
	
	//auto rotate to portrait
	//bool Screen.autorotateToPortrait = true;
	
	//auto rotate ro portrait Upside Down
	//bool Screen.autorotateToPortraitUpsideDown = true;
	
	//auto rotate ro portrait landscape left
	//bool Screen.autorotateToLandscapeLeft = true;
	
	//auto rotate ro portrait landscape right
	//bool Screen.autorotateToLandscapeRight = true;
	
	//set screen orientation
	//Screen.orientation = ScreenOrientation.LandscapeLeft;
}
