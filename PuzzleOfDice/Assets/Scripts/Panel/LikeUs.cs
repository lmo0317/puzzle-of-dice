using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LikeUs : MonoBehaviour
{
    public TextureUpdate[] friends;
    public static LikeUs instance;

    public UILabel label_likebutton;
    public UILabel label_likecount;
    private bool isLike = false;

    // Use this for initialization
    void Start()
    {
        instance = this;

        //FB.API("1477936459088484?fields=total_likes_sentence", Facebook.HttpMethod.GET, likeCount);
        FB.API("1477936459088484?fields=likes", Facebook.HttpMethod.GET, likeCount);

        FB.API("fql?q=SELECT+uid+FROM+page_fan+WHERE+page_id=1477936459088484+AND+uid+IN+(SELECT+uid1+FROM+friend+WHERE+uid2="+FB.UserId+")", Facebook.HttpMethod.GET, likeFriends);
        FB.API("fql?q=SELECT+uid+FROM+page_fan+WHERE+page_id=1477936459088484+AND+uid=" + FB.UserId, Facebook.HttpMethod.GET, likeMe);
    }

    public static void likeCount(FBResult result)
    {
        if (result.Error != null)
        {
            FbDebug.Error(result.Error);
            Debug.LogError(result.Error);

            FB.API("1477936459088484?fields=likes", Facebook.HttpMethod.GET, likeCount);
            return;
        }

        Debug.Log("TEST COUNT1 : " + result.Text);

        string count = Util.DeserializeLikeCounts(result.Text);

        if (LikeUs.instance != null)
        {
            LikeUs.instance.setLikeCount(count);
        }
    }

    public static void likeMe(FBResult result)
    {
        if (result.Error != null)
        {
            FbDebug.Error(result.Error);
            Debug.LogError(result.Error);

            FB.API("fql?q=SELECT+uid+FROM+page_fan+WHERE+page_id=1477936459088484+AND+uid=" + FB.UserId + ")", Facebook.HttpMethod.GET, likeMe);
            return;
        }

        List<object> checkMy = Util.DeserializeLikeFriends(result.Text);

        foreach (object temp in checkMy)
        {
            Dictionary<string, object> t = (Dictionary<string, object>)temp;
            string uid = System.Convert.ToString(t["uid"]);

            if(uid.Equals(FB.UserId))
            {
                if (LikeUs.instance != null)
                {
                    LikeUs.instance.setLikeButtonSet(true);
                }
                return;
            }
        }

        if (LikeUs.instance != null)
        {
            LikeUs.instance.setLikeButtonSet(false);
        }
    }

    public static void likeFriends(FBResult result)
    {
        if (result.Error != null)
        {
            FbDebug.Error(result.Error);
            Debug.LogError(result.Error);

            FB.API("fql?q=SELECT+uid+FROM+page_fan+WHERE+page_id=1477936459088484+AND+uid+IN+(SELECT+uid1+FROM+friend+WHERE+uid2=" + FB.UserId + ")", Facebook.HttpMethod.GET, likeFriends);
            return;
        }

        List<object> likesFriends = Util.DeserializeLikeFriends(result.Text);

        var x = 0;
        foreach (object temp in likesFriends)
        {
            Dictionary<string, object> t = (Dictionary<string, object>)temp;
            string uid = System.Convert.ToString(t["uid"]);

            if(LikeUs.instance != null)
            {
                LikeUs.instance.setFriendsFid(uid, x);
            }

            x++;

            if(x > 10)
                break;
        }
    }

    public void setFriendsFid(string fid, int num)
    {
        friends[num].fid = fid;
    }

    public void setLikeButtonSet(bool isLike)
    {
        this.isLike = isLike;

        if(isLike)
        {
            label_likebutton.text = "liked";
        }
        else
        {
            label_likebutton.text = "like";
        }
    }

    public void LikeButton()
    {
        Debug.Log("Click Like Button");

        // Send Like!!!
        if(!isLike)
        {
            Debug.Log("Call Like");
            Application.OpenURL("fb://profile/1477936459088484");
            //FB.API("1477936459088484?fields=total_likes_sentence", Facebook.HttpMethod.GET, likeCount);
        }
    }

    public void setLikeCount(string count)
    {
        label_likecount.text = "People like : " + count;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
