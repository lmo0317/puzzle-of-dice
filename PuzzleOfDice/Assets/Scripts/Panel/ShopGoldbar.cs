using UnityEngine;
using System.Collections;

public class ShopGoldbar : MonoBehaviour
{
    public UILabel label_gold;

    public UILabel label_10;
    public UILabel label_50;
    public UILabel label_100;
    public UILabel label_500;
    public UILabel label_1000;

    private string price_unit;

    /*
    public Game_ServerConnection gameServerConnection;
    public Title_ServerConnection serverconnection;
    public GameOver_ServerConnection gameOverConnection;
    */

    // Use this for initialization
    void Start()
    {
        price_unit = CMainData.Currency;

        if (label_gold != null)
            label_gold.text = CMainData.Gold.ToString();

        /*
        if (serverconnection != null)
        {
            serverconnection.PriceCheck("http://thepipercat.com/money01.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold10StringCheck));
            serverconnection.PriceCheck("http://thepipercat.com/money02.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold50StringCheck));
            serverconnection.PriceCheck("http://thepipercat.com/money03.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold100StringCheck));
            serverconnection.PriceCheck("http://thepipercat.com/money04.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold500StringCheck));
            serverconnection.PriceCheck("http://thepipercat.com/money05.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold1000StringCheck));
        }
        else if (gameServerConnection != null)
        {
            gameServerConnection.PriceCheck("http://thepipercat.com/money01.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold10StringCheck));
            gameServerConnection.PriceCheck("http://thepipercat.com/money02.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold50StringCheck));
            gameServerConnection.PriceCheck("http://thepipercat.com/money03.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold100StringCheck));
            gameServerConnection.PriceCheck("http://thepipercat.com/money04.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold500StringCheck));
            gameServerConnection.PriceCheck("http://thepipercat.com/money05.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold1000StringCheck));
        }
        else if (gameOverConnection != null)
        {
            gameOverConnection.PriceCheck("http://thepipercat.com/money01.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold10StringCheck));
            gameOverConnection.PriceCheck("http://thepipercat.com/money02.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold50StringCheck));
            gameOverConnection.PriceCheck("http://thepipercat.com/money03.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold100StringCheck));
            gameOverConnection.PriceCheck("http://thepipercat.com/money04.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold500StringCheck));
            gameOverConnection.PriceCheck("http://thepipercat.com/money05.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold1000StringCheck));
        }
        */

        if(ServerConnection.g_instance != null)
        {
            ServerConnection.g_instance.PriceCheck("http://thepipercat.com/money01.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold10StringCheck));
            ServerConnection.g_instance.PriceCheck("http://thepipercat.com/money02.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold50StringCheck));
            ServerConnection.g_instance.PriceCheck("http://thepipercat.com/money03.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold100StringCheck));
            ServerConnection.g_instance.PriceCheck("http://thepipercat.com/money04.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold500StringCheck));
            ServerConnection.g_instance.PriceCheck("http://thepipercat.com/money05.html", new CallBackMethod.CallBackClass.PriceCheckCallBack(gold1000StringCheck));
        }
    }

    bool getGoldString(string html,ref string _usd,ref string _local)
    {
        string usd = "", local = "", temp, templocal;
        int i = 1, j, z, l;

        if (html.Contains("product:price:amount"))
        {
            while (i > 0)
            {
                i = html.IndexOf("product:price:amount", i + 1);

                if (i < 0)
                {
                    break;
                }

                j = html.IndexOf("content", i + 1);
                z = html.IndexOf("\"", j + 1);
                l = html.IndexOf("\"", z + 1);

                temp = html.Substring(z + 1, l - z - 1);

                i = html.IndexOf("product:price:currency", l);
                j = html.IndexOf("content", i + 1);
                z = html.IndexOf("\"", j + 1);
                l = html.IndexOf("\"", z + 1);

                templocal = html.Substring(z + 1, l - z - 1);

                if (templocal.Equals(price_unit))
                {
                    local = temp;
                    break;
                }
                else if (templocal.Equals("USD"))
                {
                    usd = temp;
                }
            }

            _usd = usd;
            _local = local;

            return true;
        }

        return false;
    }

    public void gold10StringCheck(string html)
    {
        string usd = "", local = "";

        if (getGoldString(html,ref usd,ref local) == false)
            return;

        if (!local.Equals(""))
        {
            label_10.text = local + " " + price_unit;
        }
        else if (!usd.Equals(""))
        {
            label_10.text = (System.Convert.ToSingle(usd) * CMainData.Exchange).ToString(string.Format("#,##0.###")) + " " + price_unit;
        }
    }

    public void gold50StringCheck(string html)
    {
        string usd = "", local = "";

        if (getGoldString(html,ref usd,ref local) == false)
            return;

        if (!local.Equals(""))
        {
            label_50.text = local + " " + price_unit;
        }
        else if (!usd.Equals(""))
        {
            label_50.text = (System.Convert.ToSingle(usd) * CMainData.Exchange).ToString(string.Format("#,##0.###")) + " " + price_unit;
        }
    }

    public void gold100StringCheck(string html)
    {
        string usd = "", local = "";

        if (getGoldString(html, ref usd, ref local) == false)
            return;

        if (!local.Equals(""))
        {
            label_100.text = local + " " + price_unit;
        }
        else if (!usd.Equals(""))
        {
            label_100.text = (System.Convert.ToSingle(usd) * CMainData.Exchange).ToString(string.Format("#,##0.###")) + " " + price_unit;
        }
    }

    public void gold500StringCheck(string html)
    {
        string usd = "", local = "";

        if (getGoldString(html, ref usd, ref local) == false)
            return;

        Debug.Log("USD = " + usd + "," + "Exchange = " + CMainData.Exchange);

        if (!local.Equals(""))
        {
            label_500.text = local + " " + price_unit;
        }
        else if (!usd.Equals(""))
        {
            label_500.text = (System.Convert.ToSingle(usd) * CMainData.Exchange).ToString(string.Format("#,##0.###")) + " " + price_unit;
        }
    }

    public void gold1000StringCheck(string html)
    {
        string usd = "", local = "";

        if (getGoldString(html, ref usd, ref local) == false)
            return;

        if (!local.Equals(""))
        {
            label_1000.text = local + " " + price_unit;
        }
        else if (!usd.Equals(""))
        {
            label_1000.text = (System.Convert.ToSingle(usd) * CMainData.Exchange).ToString(string.Format("#,##0.###")) + " " + price_unit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (label_gold != null)
            label_gold.text = CMainData.Gold.ToString();
    }

    public static bool PayErrorCheck(string result)
    {
        if(result.Contains("error_code"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void gold10()
    {
        Debug.Log("Gold10");
        FaceBook.CallFBPay(product:"http://thepipercat.com/money01.html",
        callback:delegate(FBResult r) 
        {
            if(r.Error != null)
            {
                Debug.Log("Result: " + r.Text);
            }
            else
            {
                if (!PayErrorCheck(r.Text))
                {
                    CMainData.Gold += 10;
                }
                Debug.Log("Result: " + r.Text);
                //testLabel.text = r.Text;
            }          
        });
    }

    public void gold50()
    {
        Debug.Log("Gold50");
        FaceBook.CallFBPay(product: "http://thepipercat.com/money02.html", 
        callback:delegate(FBResult r) 
        {
            if (r.Error != null)
            {
                Debug.Log("Result: " + r.Text);
            }
            else
            {
                if (!PayErrorCheck(r.Text))
                {
                    CMainData.Gold += 50;
                }
                Debug.Log("Result: " + r.Text);
                //testLabel.text = r.Text;
            }
        });
    }

    public void gold100()
    {
        Debug.Log("Gold100");
        FaceBook.CallFBPay(product: "http://thepipercat.com/money03.html", 
        callback:delegate(FBResult r) 
        {
            if (r.Error != null)
            {
                Debug.Log("Result: " + r.Text);
            }
            else
            {
                if (!PayErrorCheck(r.Text))
                {
                    CMainData.Gold += 100;
                }
                Debug.Log("Result: " + r.Text);
                //testLabel.text = r.Text;
            }
        });
    }

    public void gold500()
    {
        Debug.Log("Gold500");
        FaceBook.CallFBPay(product: "http://thepipercat.com/money04.html", 
        callback:delegate(FBResult r) 
        {
            if (r.Error != null)
            {
                Debug.Log("Result: " + r.Text);
            }
            else
            {
                if (!PayErrorCheck(r.Text))
                {
                    CMainData.Gold += 500;
                }
                Debug.Log("Result: " + r.Text);
                //testLabel.text = r.Text;
            }
        });
    }

    public void gold1000()
    {
        Debug.Log("Gold1000");
        FaceBook.CallFBPay(product: "http://thepipercat.com/money05.html", 
        callback:delegate(FBResult r) 
        {
            if (r.Error != null)
            {
                Debug.Log("Result: " + r.Text);
            }
            else
            {
                if (!PayErrorCheck(r.Text))
                {
                    CMainData.Gold += 1000;
                }
                Debug.Log("Result: " + r.Text);
                //testLabel.text = r.Text;
            }
        });
    }

    public void ShopGoldBarCloseButton()
    {
        if(SceneTitle.instance != null)
        {
            SceneTitle.instance.ShopGoldBarCloseButton();
        }
        else if(SceneGameOver.g_instance != null)
        {
            SceneGameOver.g_instance.ShopGoldBarCloseButton();
        }
    }
}
