// lib

using UnityEngine;
using System.Collections;

public class CLib : MonoBehaviour 
{
	//time after the start of app
	public static float getRealTime()
	{
		return Time.realtimeSinceStartup;
	}
	
	//time after the start of new scene
	public static float getSceneStartTime()
	{
		return Time.timeSinceLevelLoad;
	}
	
	//time after the start of app (only play time)
	public static float getAppPlayTime()
	{
		return Time.fixedTime;
	}
	
	//get time speed value
	public static float getTimeSpeed()
	{
		return Time.timeScale;
	}
	
	//set time speed
	public static void setTimeSpeed( float speed = 1.0F )
	{
		Time.timeScale = speed;
	}
	
	public static float getTime()
	{
		return Time.time;
	}
}

