using UnityEngine;
using System.Collections;

public class RequestFriendItem : MonoBehaviour
{

    public UILabel label_name;
    public TextureUpdate picture;

    private string fid = null;
    //private string friendname = null;

    private bool loading = false;

    public UIToggle btn_Toggle;

    public void SetFriendContext(string friendname, string fid)
    {
        //this.friendname = friendname;
        this.fid = fid;
        btn_Toggle.value = true;

        loading = true;
        if (label_name != null)
            label_name.text = friendname;
    }

    public string Fid
    {
        set { fid = value; }
        get { return fid; }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //btn_Toggle.isChecked

        if (loading)
        {
            picture.fid = fid;
            if (!FaceBook.getfriendImages().ContainsKey(fid))
            {                
                FacebookPictureDownloader.EnQueue(fid);

                /*
                FB.API(Util.GetPictureURL(fid, 128, 128), Facebook.HttpMethod.GET, pictureResult =>
                {
                    if (pictureResult.Error != null)
                    {
                        Debug.LogError(pictureResult.Error);
                    }
                    else
                    {
                        FaceBook.getfriendImages().Add(fid.ToString(), pictureResult.Texture);
                    }
                });
                 */
            }

            loading = false;
        }
    }
}
