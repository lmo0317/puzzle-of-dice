using UnityEngine;
using System.Collections;

using CallBackMethod;

public class Ranking_MsgSend : MonoBehaviour
{
    private string fid = null;

    public UILabel label_name;
    public UILabel label_score;
    public UILabel label_rank;

    private bool sendable = false;
    private bool init = false;
    private bool me = false;

    public TextureUpdate picture;

    public GameObject sendBtn;
    public GameObject sendunable;

    public void SetInit(int rank, string fid, string name, string score, string sendable, bool me)
    {
        this.fid = fid;

        if (this.label_rank != null)
            this.label_rank.text = rank.ToString();
        if (this.label_name != null)
            this.label_name.text = name;
        if (this.label_score != null)
            this.label_score.text = score;

        if (sendable.Equals("1"))
        {
            this.sendable = true;
        }
        else
        {
            this.sendable = false;
        }

        this.me = me;

        if (me)
        {
            label_rank.color = new Color(13.0f / 255.0f, 150.0f / 255.0f, 150.0f / 255.0f);
            this.sendable = false;
            sendBtn.SetActive(false);
        }
        else if (this.sendable)
        {
            sendBtn.SetActive(true);
            sendunable.SetActive(false);
        }
        else
        {
            sendBtn.SetActive(true);
            sendunable.SetActive(true);
        }

        if(!me && rank > 3)
        {
            label_rank.color = new Color(8.0f / 255.0f, 8.0f / 255.0f, 8.0f / 255.0f);
        }

        picture.fid = fid;

        if (!FaceBook.getfriendImages().ContainsKey(fid))
        {
            FacebookPictureDownloader.EnQueue(fid);
        }

        this.init = true;
    }

    public string Fid
    {
        get { return this.fid; }
    }

    public bool Sendable
    {
        get { return this.sendable; }
    }

    public void sendButtonClick()
    {
        if (GameRanking.instance != null && sendable && init)
        {
            GameRanking.instance.sendButton(fid);
        }
    }

    public void setuneable()
    {
        if (!me)
        {
            sendable = false;
            sendunable.SetActive(true);
        }
    }
}
