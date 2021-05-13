using UnityEngine;
using System.Collections;

public class FriendTogetherItem : MonoBehaviour {

    public UILabel label_name;
    public TextureUpdate picture;
    public GameObject check;

    private bool checkable = true;
    private bool setenable = false;
    private string fid = null;
    //private string friendname = null;

    public string Fid{
        get {return fid; }
        set {fid = value; }
    }

    public void SetFriendTogetherContext(string friendname, string fid)
    {
        setenable = true;
        //this.friendname = friendname;
        this.fid = fid;
        picture.fid = fid;

        if(!FaceBook.getfriendImages().ContainsKey(fid))
        {
            FacebookPictureDownloader.EnQueue(fid);
        }

        if (label_name != null)
            label_name.text = friendname;
    }

    public void SetNonClick()
    {
        check.SetActive(true);
        checkable = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HelpButtonClick()
    {
        if (setenable && FriendTogether.instance != null && checkable)
        {
            /*
            check.SetActive(true);
            checkable = false;
            */
            FriendTogether.instance.HelpMessage(fid, this);
        }
    }
}
