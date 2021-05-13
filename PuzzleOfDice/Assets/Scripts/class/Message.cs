using UnityEngine;
using System.Collections;

public class Message {
    private int type = 0;
    private string senderName = null;
    private string fid = null;

    private GameObject obj = null;

    public Message(int type, string senderName, string fid)
    {
        this.type = type;
        this.senderName = senderName;
        this.fid = fid;        
    }

    public int Type
    {
        get { return type; }
        set { type = value; }
    }

    public string Sendder
    {
        get { return senderName; }
        set { senderName = value; }
    }

    public string Fid
    {
        get { return fid; }
        set { fid = value; }
    }

    public GameObject Obj
    {
        get { return obj; }
        set { obj = value;}
    }
}
