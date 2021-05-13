using UnityEngine;
using System.Collections;

public class StringData : ScriptableObject
{
    public static string getString(string key)
    {
        Localization loc = Localization.instance;

        //Debug.Log(loc.currentLanguage);

        if (loc != null)
        {
            string val = string.IsNullOrEmpty(key) ? "" : loc.Get(key);
            return val;
        }
        else
            return "";
    }

    /** GameMessageBox **/
    public static string label_message_num_key = "1017"; // ex) 요청 %s 개
    //public static string label_message_num = "요청 ";

    public static string label_message_panel = "내 메세지함";

    /** Message Text **/
    public static string Type_Unknown_key = "01";
    public static string Type1_Title_key = "11";
    public static string Type1_Message_key = "12";
    public static string Type1_AcceptButton_key = "13";
    public static string Type2_Title_key = "21";
    public static string Type2_Message_key = "22";
    public static string Type2_AcceptButton_key = "23";
    public static string Type3_Title_key = "31";
    public static string Type3_Message_key = "32";
    public static string Type3_AcceptButton_key = "33";
    /*public static string Type1_Title = "친구가 선물을 보냈습니다!";
    public static string Type1_Message = "님으로부터 주사위를 받았습니다!\n주사위로 보답하고 싶나요?";
    public static string Type1_AcceptButton = "좋아요!";
    public static string Type2_Title = "선물을 받았어요!";
    public static string Type2_Message = "님이 추가 주사위를 선물했습니다! 보답으로 주사위를 보내시겠어요?";
    public static string Type2_AcceptButton = "수락";
    public static string Type3_Title = "님을 도와주세요";
    public static string Type3_Message = "님이 주사위 도움을 받고 싶어\n합니다!";
    public static string Type3_AcceptButton = "보내기";*/


    /** GameOverRenewRanking **/
    public static string RenewRanking_key = "1061";
    //public static string RenewRanking_Pre = "축하합니다! ";
    //public static string RenewRanking_Post = "님을 이겼습니다.";

    public static string[] Tutorial_key = {"3001","3002","3006","3007","3008","3009","3010","3011","3012","3013","3014","3015"};
}
