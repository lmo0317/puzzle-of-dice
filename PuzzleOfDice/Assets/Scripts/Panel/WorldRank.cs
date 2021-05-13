using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

public class WorldRank : MonoBehaviour {

    public UITexture[] face;
    public GameObject[] back;
    public UILabel[] friend_name;
    public UILabel[] rank;
    public UILabel[] score;
    private JsonData topUser;
    Dictionary<string, string> friendInfoList = new Dictionary<string, string>();
    
	// Use this for initialization
	void Start () {

        topUser = CMainData.TopUser;
        if(topUser != null)
        {
            for (int i = 0; i < topUser.Count; i++)
            {
                TextureUpdate t = face[i].GetComponent<TextureUpdate>();
                if (t != null)
                {
                    if (FaceBook.getfriendImages().ContainsKey(topUser[i]["fid"].ToString()))
                    {
                        Texture picture = null;
                        FaceBook.getfriendImages().TryGetValue(topUser[i]["fid"].ToString(), out picture);

                        if (picture != null)
                        {
                            if (face[i] != null)
                                face[i].mainTexture = picture;
                            Destroy(t);
                        }
                        else
                        {
                            t.fid = topUser[i]["fid"].ToString();
                        }
                    }
                    else
                    {
                        t.fid = topUser[i]["fid"].ToString();
                        FacebookPictureDownloader.EnQueue(topUser[i]["fid"].ToString());
                    }
                }

                /*
                string strURL = topUser[i]["fid"].ToString() + "?fields=id,name";
                Debug.Log("FRIEND NAME URL = [ " + strURL + " ]");
                FB.API(strURL, Facebook.HttpMethod.GET, result =>
                {
                    if (result.Error == null)
                    {
                        Debug.Log("FRIEND NAME RESULT = [ " + result.Text + " ]");
                        Dictionary<string, string> friendInfo = Util.DeserializeJSONFriendInfo(result.Text);
                        friendInfoList.Add(friendInfo["id"],friendInfo["name"]);
                    }
                });
                */

                FacebookName facebookName = friend_name[i].GetComponent<FacebookName>();
                facebookName.fid = topUser[i]["fid"].ToString();

                //if (friend_name[i] != null)
                //    friend_name[i].text = topUser[i]["name"].ToString();

                if (score[i] != null)
                    score[i].text = topUser[i]["score"].ToString();
            }

            for (int i = topUser.Count; i < 10; i++)
            {
                face[i].gameObject.SetActive(false);
                back[i].SetActive(false);
                friend_name[i].gameObject.SetActive(false);
                rank[i].gameObject.SetActive(false);
                score[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                face[i].gameObject.SetActive(false);
                back[i].SetActive(false);
                friend_name[i].gameObject.SetActive(false);
                rank[i].gameObject.SetActive(false);
                score[i].gameObject.SetActive(false);
            }
        }

        TextureUpdate t_my = face[10].GetComponent<TextureUpdate>();

        if (CMainData.UserTexture != null)
        {
            face[10].mainTexture = CMainData.UserTexture;
            Destroy(t_my);
        }
        else if (FaceBook.getfriendImages().ContainsKey(FB.UserId))
        {
            Texture picture = null;
            FaceBook.getfriendImages().TryGetValue(FB.UserId, out picture);

            if (picture != null)
            {
                if (face[10] != null)
                    face[10].mainTexture = picture;
                Destroy(t_my);
            }
            else
            {
                t_my.fid = FB.UserId;
            }
        }
        else
        {
            FacebookPictureDownloader.EnQueue(FB.UserId);
        }

        if (CMainData.Username != null)
        {
            friend_name[10].text = CMainData.Username;
        }

        if (score[10] != null)
            score[10].text = CMainData.UserScore.ToString();
        if (rank[10] != null)
            rank[10].text = CMainData.UserRanking.ToString();
	}

    // Update is called once per frame
    void Update()
    {
        /*
        for (int i = 0; i < 10; i++)
        {
            if (topUser[i]["fid"] != null && friendInfoList.ContainsKey(topUser[i]["fid"].ToString()))
            {
                friend_name[i].text = friendInfoList[topUser[i]["fid"].ToString()].ToString();
                //friend_name[i].text = topUser[i]["fid"].ToString();
            }
        }
        */
	}
}
