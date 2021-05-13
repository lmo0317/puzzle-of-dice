using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

public class FacebookName : MonoBehaviour {

    public string fid = null;
    private UILabel label;

	// Use this for initialization
	void Start () {
        label = GetComponent<UILabel>(); 
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (fid != null)
        {
            string strURL = fid + "?fields=id,name";
            Debug.Log("FRIEND NAME URL = [ " + strURL + " ]");
            FB.API(strURL, Facebook.HttpMethod.GET, result =>
            {
                if (result.Error == null)
                {
                    Debug.Log("FRIEND NAME RESULT = [ " + result.Text + " ]");
                    Dictionary<string, string> friendInfo = Util.DeserializeJSONFriendInfo(result.Text);
                    label.text = friendInfo["name"];
                }
            });

            Debug.Log("Name Update, Name Correct!");
            Destroy(this);
        }
	}
}
