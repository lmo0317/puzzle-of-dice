using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FacebookPictureDownloader : MonoBehaviour {
    private static Queue<string> wait = null;
    private static Dictionary<string, bool> ing = null;
    private static Queue<string> complete = null;

    public void Start()
    {
        if (wait == null)
            wait = new Queue<string>();
        if (ing == null)
            ing = new Dictionary<string, bool>();
        if (complete == null)
            complete = new Queue<string>();
    }

    private static void CheckInit()
    {
        if (wait == null)
            wait = new Queue<string>();
        if (ing == null)
            ing = new Dictionary<string, bool>();
        if (complete == null)
            complete = new Queue<string>();
    }

    public static void EnQueue(string fid)
    {
        CheckInit();

        wait.Enqueue(fid);
    }

    public void Update()
    {
        if (wait != null)
        {
            if (wait.Count > 0)
            {
                string fid = wait.Dequeue();

                if (!FaceBook.getfriendImages().ContainsKey(fid) && !complete.Contains(fid) && !ing.ContainsKey(fid))
                {
                    ing.Add(fid, false);

                    FB.API(Util.GetPictureURL(fid, 128, 128), Facebook.HttpMethod.GET, pictureResult =>
                    {
                        ing.Remove(fid);

                        if (pictureResult.Error != null)
                        {
                            //Debug.LogError(pictureResult.Error);
                            wait.Enqueue(fid);
                        }
                        else
                        {
                            try
                            {
                                FaceBook.getfriendImages().Add(fid, pictureResult.Texture);
                                complete.Enqueue(fid);
                            }
                            catch (System.Exception e)
                            {
                                Debug.Log(e.Message + " : Exceiption.");
                            }
                        }
                    });
                }
            }
        }
    }
}
