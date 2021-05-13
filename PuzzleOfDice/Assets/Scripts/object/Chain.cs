using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour {

    private int state;

    private string currentNum = null;
    private int currentSize = -1;
    private int currentChainCheck = 0;
    private int currentNumCheck = 0;
    private float time = 0.0f;
    private float changetime = 0.0f;

    private bool numstate = false;

    public static Queue chainQueue;

    public UILabel label_chain;

    private string[] ChainString = { "C", "CH", "CHA", "CHAI", "CHAIN" };

    public UILabel label_number;
    public UILabel label_number_black;

    // Use this for initialization
    void Start()
    {
        chainQueue = new Queue();
        state = GameData.CHAIN_READY;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameData.CHAIN_READY:
                allOff();
                if (chainQueue.Count > 0)
                {
                    state = GameData.CHAIN_PLAY_LABEL;
                    currentNum = ((int)chainQueue.Dequeue()).ToString();
                    currentSize = currentNum.Length;

                    currentChainCheck = 0;
                    currentNumCheck = 1;
                    numstate = false;

                    time = 0.0f;
                }
                break;
            case GameData.CHAIN_PLAY_LABEL:
                if (chainQueue.Count > 0)
                {
                    state = GameData.CHAIN_FAST;
                }
                time += Time.deltaTime;
                if (time > GameData.CHAIN_DurationTime)
                {
                    if (currentChainCheck > 5)
                    {
                        state = GameData.CHAIN_PLAY_NUMBER;
                        break;
                    }

                    ChainOn(currentChainCheck++);
                    time = 0.0f;
                }
                break;
            case GameData.CHAIN_PLAY_NUMBER:
                if (chainQueue.Count > 0)
                {
                    state = GameData.CHAIN_FAST;
                }
                time += Time.deltaTime;
                if (time > GameData.CHAIN_DurationTime)
                {
                    if (currentNumCheck > currentSize)
                    {
                        state = GameData.CHAIN_PLAY_FLIKER;
                        changetime = 0.0f;
                        if(label_number_black != null)
                            label_number_black.text = currentNum;

                        break;
                    }
                    
                    NumberOn(currentNumCheck++);
                    time = 0.0f;
                }
                break;
            case GameData.CHAIN_PLAY_FLIKER:
                if (chainQueue.Count > 0)
                {
                    state = GameData.CHAIN_FAST;
                    break;
                }
                time += Time.deltaTime;
                changetime += Time.deltaTime;
                if (changetime > GameData.CHAIN_DurationTime)
                {
                    NumberChange(numstate);
                    numstate = !numstate;

                    changetime = 0.0f;
                }
                if (time > GameData.CHAIN_DurationTime * 7)
                {
                    state = GameData.CHAIN_PLAY_LOCK;
                    NumberChange(true);
                    time = 0.0f;
                }
                break;
            case GameData.CHAIN_PLAY_LOCK:
                time += Time.deltaTime;
                if (chainQueue.Count > 0 || time > GameData.CHAIN_ChainRemainTime)
                {
                    state = GameData.CHAIN_READY;
                    break;
                }
                break;
            case GameData.CHAIN_FAST:
                time += Time.deltaTime;
                ChainOn(5);
                if (time > GameData.CHAIN_FastDurationTime)
                {
                    if (currentNumCheck > currentSize)
                    {
                        state = GameData.CHAIN_READY;
                        break;
                    }
                    NumberOn(currentNumCheck++);
                    time = 0.0f;
                }
                break;
        }
    }

    private void allOff()
    {
        if(label_chain != null)
            label_chain.text = "";
        if (label_number != null)
            label_number.text = "";
        if (label_number_black != null)
            label_number_black.text = "";
    }

    private void ChainOn(int value)
    {
        if (value < 5)
        {
            if (label_chain != null)
                label_chain.text = ChainString[value];
        }
        else
        {
            if (label_chain != null)
                label_chain.text = ChainString[4];
        }
    }

    private void NumberOn(int value)
    {
        if (label_number != null)
        {
            NGUITools.SetActive(label_number.gameObject, true);
            label_number.text = currentNum.Substring(0, value);
        }
    }

    private void NumberChange(bool numstate)
    {
        if (label_number != null)
            NGUITools.SetActive(label_number.gameObject, numstate);
    }
}
