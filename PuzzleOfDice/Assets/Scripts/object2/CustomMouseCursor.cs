using UnityEngine;
using System.Collections;

public class CustomMouseCursor : MonoBehaviour
{
    public Texture pointer; // Inspector 뷰에서 마우스 포인터로 사용할 이미지를 드래그하여 할당해주면 된다

    public static bool isButton = false;

    void Start()
    {
        Screen.showCursor = false;
        isButton = false;
    }

    void Update()
    {
    }

    void OnGUI()
    {
        Vector2 pos = Event.current.mousePosition;
        if(isButton) {
            Screen.showCursor = false;
            GUI.Label(new Rect(pos.x, pos.y, 48, 48), pointer);
        }
        else {
            Screen.showCursor = true;
        }        
    }
}
