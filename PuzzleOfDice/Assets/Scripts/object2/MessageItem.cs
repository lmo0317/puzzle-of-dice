using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageItem : MonoBehaviour {

    public UILabel title;
    public UILabel subtitle;
    public UILabel label_acceptbutton;
    public TextureUpdate picture;

    private bool setenable = false;
    private int type = 0;
    private string fid = null;
    //private string sendderName = null;

    public void SetMessageContext(int type, string sendderName, string fid)
    {
        setenable = true;
        this.type = type;
        //this.sendderName = sendderName;
        this.fid = fid;
        picture.fid = fid;
        if (!FaceBook.getfriendImages().ContainsKey(fid))
        {
            FacebookPictureDownloader.EnQueue(fid);
        }

        switch (type)
        {
            case 1:
                if (title != null)
                {
                    //title.text = StringData.getType1_Title(sendderName);
                    title.text = StringData.getString(StringData.Type1_Title_key).Replace("%s",sendderName);
                }
                if (subtitle != null)
                {
                    //subtitle.text = StringData.getType1_Message(sendderName);
                    subtitle.text = StringData.getString(StringData.Type1_Message_key).Replace("%s", sendderName);
                }
                if (label_acceptbutton != null)
                {
                    //label_acceptbutton.text = StringData.Type1_AcceptButton;
                    label_acceptbutton.text = StringData.getString(StringData.Type1_AcceptButton_key);
                }
                break;
            case 2:
                if (title != null)
                {
                    //title.text = StringData.getType2_Title(sendderName);
                    title.text = StringData.getString(StringData.Type2_Title_key).Replace("%s", sendderName);
                }
                if (subtitle != null)
                {
                    //subtitle.text = StringData.getType2_Message(sendderName);
                    subtitle.text = StringData.getString(StringData.Type2_Message_key).Replace("%s", sendderName);
                }
                if (label_acceptbutton != null)
                {
                    //label_acceptbutton.text = StringData.Type2_AcceptButton;
                    label_acceptbutton.text = StringData.getString(StringData.Type2_AcceptButton_key);
                }
                break;
            case 3:
                if (title != null)
                {
                    //title.text = StringData.getType3_Title(sendderName);
                    title.text = StringData.getString(StringData.Type3_Title_key).Replace("%s", sendderName);
                }
                if (subtitle != null)
                {
                    //subtitle.text = StringData.getType3_Message(sendderName);
                    subtitle.text = StringData.getString(StringData.Type3_Message_key).Replace("%s", sendderName);
                }
                if (label_acceptbutton != null)
                {
                    //label_acceptbutton.text = StringData.Type3_AcceptButton;
                    label_acceptbutton.text = StringData.getString(StringData.Type3_AcceptButton_key);
                }
                break;
            default:
                if (title != null)
                {
                    title.text = StringData.getString(StringData.Type_Unknown_key);
                    //title.text = "알수 없는 메시지";
                }
                if (subtitle != null)
                {
                    subtitle.text = sendderName;
                }
                break;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AcceptButtonClick()
    {
        if(setenable && GameMessageBox.instance != null)
        {
            GameMessageBox.instance.acceptMessage(fid, type);
        }
    }

    public void CloseButtonClick()
    {
        if (setenable && GameMessageBox.instance != null)
        {
            GameMessageBox.instance.closeMessage(fid, type);
        }
    }
}
