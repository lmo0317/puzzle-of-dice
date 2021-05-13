using UnityEngine;
using System.Collections;

public class ShopCat : MonoBehaviour {

    public UILabel label_gold;

	// Use this for initialization
	void Start () {
        if (label_gold != null)
            label_gold.text = CMainData.Gold.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
