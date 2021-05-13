using UnityEngine;
using System.Collections;

public class TextureUpdate : MonoBehaviour
{

    public string fid = null;
    private UITexture texture;

    private float currentT = 0.0f;
    //private float nextT = 3.0f;

    //private bool send = true;

    // Use this for initialization
    void Start()
    {
        texture = GetComponent<UITexture>();
        currentT = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentT += Time.deltaTime;

        if (fid != null)
        {
            if (texture == null)
            {
                Debug.LogError(this.name + "," + this.gameObject.name + "," + "TextureUpdate, Texture NULL");
                return;
            }

            if (texture.mainTexture == null)
            {
                Debug.LogError("TextureUpdate, Texture.MainTexture NULL");
                return;
            }   

            if (texture.mainTexture.name.Equals("MS_ranking_bg_face"))
            {
                if (FaceBook.getfriendImages().ContainsKey(fid))
                {
                    currentT = 0.0f;

                    Texture picture = null;
                    FaceBook.getfriendImages().TryGetValue(fid, out picture);

                    if (picture != null)
                        texture.mainTexture = picture;
                }
            }
            else
            {
                Debug.Log("TextureUpdate, Texture Correct!");
                Destroy(this);
            }
        }
    }
}
