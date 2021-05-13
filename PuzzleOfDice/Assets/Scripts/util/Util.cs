using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.MiniJSON;

public class Util : ScriptableObject
{
    public static string GetPictureURL(string facebookID, int? width = null, int? height = null, string type = null)
    {
        string url = string.Format("/{0}/picture", facebookID);
        string query = width != null ? "&width=" + width.ToString() : "";
        query += height != null ? "&height=" + height.ToString() : "";
        query += type != null ? "&type=" + type : "";
        if (query != "") url += ("?g" + query);
        //Debug.Log("GetPictureURL = [ " + url + " ] ");
        return url;
    }

    public static void FriendPictureCallback(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }
    }

    public static Dictionary<string, string> RandomFriend(List<object> friends)
    {
        var fd = ((Dictionary<string, object>)(friends[Random.Range(0, friends.Count - 1)]));
        var friend = new Dictionary<string, string>();
        friend["id"] = (string)fd["id"];
        friend["first_name"] = (string)fd["first_name"];
        return friend;
    }

    public static Dictionary<string, string> DeserializeJSONProfile(string response)
    {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object nameH;
        var profile = new Dictionary<string, string>();
        if (responseObject.TryGetValue("first_name", out nameH))
        {
            profile["first_name"] = (string)nameH;
        }

        if (responseObject.TryGetValue("last_name", out nameH))
        {
            profile["last_name"] = (string)nameH;
        }

        if (responseObject.TryGetValue("name", out nameH))
        {
            profile["name"] = (string)nameH;
        }

        if (responseObject.TryGetValue("gender", out nameH))
        {
            profile["gender"] = (string)nameH;
        }

        if (responseObject.TryGetValue("locale", out nameH))
        {
            profile["locale"] = (string)nameH;
        }

        if (responseObject.TryGetValue("currency", out nameH))
        {
            profile["exchange"] = System.Convert.ToSingle(((Dictionary<string, object>)nameH)["usd_exchange_inverse"]).ToString();
            profile["currency"] = (string)(((Dictionary<string, object>)nameH)["user_currency"]);
        }

        return profile;
    }
	
	public static List<object> DeserializeScores(string response) 
	{
		var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
		object scoresh;
		var scores = new List<object>();
		if (responseObject.TryGetValue ("data", out scoresh)) 
		{
			scores = (List<object>) scoresh;
		}

		return scores;
	}

    public static string DeserializeCallbackResult(string response)
    {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object resulth;
        string returnValue = "";

        if (responseObject.TryGetValue("cancelled", out resulth))
        {
            returnValue = (string)resulth;
        }

        return returnValue;
    }

    public static List<object> DeserializeJSONFriends(string response)
    {
        //Debug.Log("DeserializeJSONFriends " + " , " + response);
 
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object friendsH;
        var friends = new List<object>();
        if (responseObject.TryGetValue("friends", out friendsH))
        {
            friends = (List<object>)(((Dictionary<string, object>)friendsH)["data"]);
        }
        return friends;
    }

    public static string DeserializeLikeCounts(string response)
    {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;

        Debug.Log("TEST Count : " + responseObject);

        //string count = (string)responseObject["total_likes_sentence"];
        string count = System.Convert.ToString(responseObject["likes"]);
        return count;
    }

    public static List<object> DeserializeLikeFriends(string response)
    {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object likesh;
        var likes = new List<object>();
        if (responseObject.TryGetValue("data", out likesh))
        {
            likes = (List<object>)likesh;
        }

        return likes;
    }

    public static Dictionary<string, string> DeserializeJSONFriendInfo(string response)
    {
        var responseObject = Json.Deserialize(response) as Dictionary<string, object>;
        object nameH;
        var profile = new Dictionary<string, string>();

        if (responseObject.TryGetValue("id", out nameH))
        {
            profile["id"] = (string)nameH;
        }

        if (responseObject.TryGetValue("name", out nameH))
        {
            profile["name"] = (string)nameH;
        }
        
        return profile;
    }
}