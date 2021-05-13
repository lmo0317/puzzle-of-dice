// dice object

using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    //state
	private int m_nState = GameData.OBJECT_STATE_NONE;
    //private int m_nEffectState = GameData.OBJECT_EFFECT_STATE_NONE;
    
    //time
    private float m_fMoveTime = GameData.OBJECT_MOVE_TIME;
    private float m_fMoveTotalTime;
    
    //effect
    private float m_fAppearEffectTime;
    private bool m_bAppearEffect = false;

    private bool m_bMoveAble = true;
    private bool m_bDestroyAble = true;

    //위치
	private Vector3 m_vecStartPosition;
    private Quaternion m_quaternionStartRotation;
    
    //object
    public GameObject Thunderbolt;
	public GameObject CircularLightWall;
	public GameObject ElectricGround;

    //주사위 2 클리어 -> 노란색 계열 반짝임 (RGB 255 255 128)
    //주사위 3 클리어 -> 연두색 계열 반짝임 (RGB 128 255 128)
    //주사위 4 클리어 -> 연한 옥색 계열 반짝임 (RGB 128 255 255)
    //주사위 5 클리어 -> 파란색 계열 반짝임 (RGB 0 128 255)
    //주사위 6 클리어 -> 보라색 계열 반짝임 (RGB 128 0 255)

    Color[] aColor = { new Color(255 / (float)255, 255 / (float)255, 255 / (float)255), new Color(255 / (float)255, 255 / (float)255, 255 / (float)255), 
                       new Color(255 / (float)255, 255 / (float)255, 128 / (float)255), new Color(128 / (float)255, 255 / (float)255, 128 / (float)255),
                       new Color(128 / (float)255, 255 / (float)255, 255 / (float)255), new Color(0 / (float)255,   128 / (float)255, 255 / (float)255),
                       new Color(255 / (float)255, 0 / (float)255,   128 / (float)255) };

    public bool getMoveAble() { return m_bMoveAble; }
    public bool getDestoryAble() { return m_bDestroyAble; }
    public void setMoveable(bool bMoveAble) { m_bMoveAble = bMoveAble; }
    public void setDestoryAble(bool bDestoryAble) { m_bDestroyAble = bDestoryAble; }
    public float getPercent() { return m_fMoveTime / m_fMoveTotalTime; }
	
	void Start ()
	{
        if (MainGame.g_Instance != null)
        {
            MainGame.g_Instance.increaseCreateDiceCount();
        }
	}
	
	void Update ()
	{
		if( CMainData.getPause() == true ) {
            return;
		}

        switch (m_nState)
        {
            case GameData.OBJECT_STATE_MOVE_RIGHT:
            case GameData.OBJECT_STATE_MOVE_LEFT:
            case GameData.OBJECT_STATE_MOVE_UP:
            case GameData.OBJECT_STATE_MOVE_DOWN:
                Move();
                break;
            case GameData.OBJECT_STATE_APPEAR:
                Appear();
                break;
            case GameData.OBJECT_STATE_DISAPPEAR:
                Disappear();
                break;
            default:
                break;
        }

        appearEffectProcess();

        if (m_nState == GameData.OBJECT_STATE_NONE)
        {
            if (MainGame.g_Instance.GetCursor().transform.position.x == transform.position.x &&
                MainGame.g_Instance.GetCursor().transform.position.z == transform.position.z)
            {
                transform.position = new Vector3(transform.position.x, -0.1f, this.transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, 0.0f, this.transform.position.z);
            }
        }
	}
	
	public int GetState()
	{
		return m_nState;
	}
	
    //use extern
	public void MoveDice(int direction)
	{
        SoundManager.g_Instance.PlayEffectSound("dice_move_01");
        MoveDice(direction, GameData.OBJECT_MOVE_TIME);
	}
	
	public void MoveDice(int direction, float fMoveTotalTime)
	{
        m_nState = direction;
        m_fMoveTime = 0;
        m_fMoveTotalTime = fMoveTotalTime;
		m_vecStartPosition = transform.position;
        m_quaternionStartRotation = transform.rotation;
    }

    public void AppearDiceOneShot()
    {
        m_nState = GameData.OBJECT_STATE_NONE;
        m_fMoveTime = 0;
        m_vecStartPosition = this.transform.position;
        this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
        AppearEnd();
    }
	
	public void AppearDice()
	{
        if (SoundManager.g_Instance)
        {
            SoundManager.g_Instance.PlayEffectSound("dice_make_01");
            SoundManager.g_Instance.PlayEffectSound("dice_makelocation_01");
        }
		AppearDice(GameData.OBJECT_APPEAR_TIME);
	}

    public void AppearDice(float fMoveTotalTime)
	{
		m_nState = GameData.OBJECT_STATE_APPEAR;
        m_fMoveTime = 0;
        m_fMoveTotalTime = fMoveTotalTime;
		m_vecStartPosition = this.transform.position;

		Instantiate(Thunderbolt,new Vector3(transform.position.x,0,transform.position.z),Quaternion.Euler(new Vector3(90,0,0)));
		Instantiate(CircularLightWall,new Vector3(transform.position.x,-0.5f,transform.position.z),Quaternion.Euler(new Vector3(90,0,0)));
	}
	
	public void DisappearDice()
	{
        //if (CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL) DisappearDice(GameData.OBJECT_DISAPPEAR_TIME_IN_TUTORIAL);
        //else DisappearDice(GameData.OBJECT_DISAPPEAR_TIME);

        DisappearDice(GameData.OBJECT_DISAPPEAR_TIME);
	}

    public void DisappearDice(float fMoveTotalTime)
	{
		m_nState = GameData.OBJECT_STATE_DISAPPEAR;
        m_fMoveTime = 0;
        m_fMoveTotalTime = fMoveTotalTime;
		m_vecStartPosition = this.transform.position;
	}
	
	public void PlusDisappearTime()
	{
        float fValue = CTimeManager.m_TimeDisappearDice * (CTimeManager.m_TimeDisappearPlusPercent * 0.01f);
        m_fMoveTime -= fValue;

        if (m_fMoveTime <= 0)
        {
            fValue += m_fMoveTime;
            m_fMoveTime = 0;
        }

        this.transform.Translate(0.0f, (1.0f / m_fMoveTotalTime) * fValue, 0.0f, Space.World);
    }

    public void waveEffectBegin()
    {
        Instantiate(ElectricGround, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.Euler(new Vector3(90, 0, 0)));
    }

    int GetDiceIndex()
    {
        switch (m_nState)
        {
            case GameData.OBJECT_STATE_MOVE_RIGHT:
                return 0;
            case GameData.OBJECT_STATE_MOVE_LEFT:
                return 1;
            case GameData.OBJECT_STATE_MOVE_UP:
                return 2;
            case GameData.OBJECT_STATE_MOVE_DOWN:
                return 3;
            default:
                return 0;
        }
    }
    	
    //use update
    void Move()
    {
        float fDT = Time.deltaTime;
        int nIndex = GetDiceIndex();

        Vector3[] moveRotate = { new Vector3(0.0f, 0.0f, -90.0f), 
                                   new Vector3(0.0f, 0.0f, 90.0f ), 
                                   new Vector3(90.0f, 0.0f, 0.0f), 
                                   new Vector3(-90.0f, 0.0f, 0.0f)};

        Vector3[] moveTranslate = { new Vector3(fDT / m_fMoveTotalTime, 0.0f, 0.0f),
                                      new Vector3(-fDT / m_fMoveTotalTime, 0.0f, 0.0f),
                                      new Vector3(0.0f, 0.0f, fDT / m_fMoveTotalTime),
                                      new Vector3(0.0f, 0.0f, -(fDT / m_fMoveTotalTime))};

        Vector3[] movePosition = { new Vector3(m_vecStartPosition.x + 1.0f, 0.0f, m_vecStartPosition.z),
                                     new Vector3(m_vecStartPosition.x - 1.0f, 0.0f, m_vecStartPosition.z), 
                                     new Vector3(m_vecStartPosition.x, 0.0f, m_vecStartPosition.z + 1.0f), 
                                     new Vector3(m_vecStartPosition.x, 0.0f, m_vecStartPosition.z - 1.0f) };

        m_fMoveTime += fDT;
        if (m_fMoveTime <= m_fMoveTotalTime / 2)
            transform.Translate(0.0f, fDT / (m_fMoveTotalTime * 2), 0.0f, Space.World);
        else
            transform.Translate(0.0f, -(fDT / (m_fMoveTotalTime * 2)), 0.0f, Space.World);

        transform.Rotate(moveRotate[nIndex] * (fDT / m_fMoveTotalTime), Space.World);
        transform.Translate(moveTranslate[nIndex], Space.World);

        if (m_fMoveTime >= m_fMoveTotalTime)
        {
            m_nState = GameData.OBJECT_STATE_NONE;
            transform.rotation = m_quaternionStartRotation;
            transform.Rotate(moveRotate[nIndex], Space.World);            
            this.transform.position = movePosition[nIndex];
            MoveEnd();
        }
    }
	
	void Appear()
	{
        float fDT = Time.deltaTime;
        m_fMoveTime += fDT;
        transform.Translate(0.0f, (fDT / m_fMoveTotalTime), 0.0f, Space.World);
        if (m_fMoveTime >= m_fMoveTotalTime)
		{
			m_nState = GameData.OBJECT_STATE_NONE;
			this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
			AppearEnd();
		}
	}
	
	void Disappear()
	{
        //튜토리얼일 경우 주사위는 사라지지 않는다.
        if (CMainData.getGameMode() == GameData.GAME_MODE_TUTORIAL)
        {
            float percent = getPercent();
            if (percent > 0.9)
            {
                //전체 주사위가 사라진 시간이 목표 시간보다 많아 질경우
                return;
            }
        }

        float fDT = Time.deltaTime;
        m_fMoveTime += fDT;
        this.transform.Translate(0.0f, -(fDT / m_fMoveTotalTime), 0.0f, Space.World);

        if (m_fMoveTime >= m_fMoveTotalTime)
		{
			m_nState = GameData.OBJECT_STATE_NONE;
			this.transform.position = new Vector3(this.transform.position.x, -1.1f, this.transform.position.z);
			DisappearEnd();
		}
	}

    public void Remove()
    {
        Destroy(gameObject);
    }

    //end
	void MoveEnd()
	{
		MainGame.g_Instance.OnMoveEnd(this.transform.position);
	}
	
	void AppearEnd()
	{
        //appear이 끝날경우 한번 깜빡 거림
        appearEffectBegin();
		MainGame.g_Instance.OnAppearEnd(this.transform.position);
	}
	
	void DisappearEnd()
	{
        Remove();
		MainGame.g_Instance.OnDisappearEnd(this.transform.position);
	}
	
    //function
	bool CheckRange(float val, float range)
	{
		if( range-0.1f < val && val < range+0.1f )
			return true;
			
		return false;
	}

    public void appearEffectBegin()
    {
        doEffect(GameData.OBJECT_EFFECT_STATE_MAKE_ON);
        m_fAppearEffectTime = 0;
        m_bAppearEffect = true;
    }

    public void appearEffectEnd()
    {
        doEffect(GameData.OBJECT_EFFECT_STATE_MAKE_OFF);
        m_fAppearEffectTime = 0;
        m_bAppearEffect = false;
    }

    public void appearEffectProcess()
    {
        if (m_bAppearEffect == false)
            return;

        m_fAppearEffectTime += Time.deltaTime;
        if (m_fAppearEffectTime > GameData.FLASH_TIME)
        {
            appearEffectEnd();
        }
    }

    public void doEffect(int nEffectState)
    {
        int nDirection = GetDirection();
        //m_nEffectState = nEffectState;
        
        switch (nEffectState)
        {
            case GameData.OBJECT_EFFECT_STATE_CLEAR_ON:
                {
                    Shader shader = Shader.Find("Custom/light");
                    renderer.material.shader = shader;
                    renderer.material.color = aColor[nDirection];
                }
                break;
            case GameData.OBJECT_EFFECT_STATE_CLEAR_OFF:
                {
                    Shader shader = Shader.Find("Diffuse");
                    renderer.material.shader = shader;
                    renderer.material.color = aColor[0];
                }
                break;
            case GameData.OBJECT_EFFECT_STATE_MAKE_ON:
                {
                    Shader shader = Shader.Find("Custom/light");
                    renderer.material.shader = shader;
                    renderer.material.color = aColor[0];
                }
                break;
            case GameData.OBJECT_EFFECT_STATE_MAKE_OFF:
                {
                    Shader shader = Shader.Find("Diffuse");
                    renderer.material.shader = shader;
                    renderer.material.color = aColor[0];
                }
                break;

            default:
                break;
        }
    }

    public void SetDirection(int direction)
    {
        //방향에 맞게 주사위 회전
        if (direction == 1) { /*default*/ }
        else if (direction == 2) this.transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
        else if (direction == 3) this.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);
        else if (direction == 4) this.transform.Rotate(90.0f, 0.0f, 0.0f, Space.World);
        else if (direction == 5) this.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
        else if (direction == 6) this.transform.Rotate(0.0f, 0.0f, 180.0f, Space.World);
        else Debug.Log("ERROR : Func [ SetDirection ] Cause [ direction ] ");
    }

    public int GetDirection()
    {
        if (CheckRange(this.transform.up.y, 1.0f) == true) return 1;
        else if (CheckRange(this.transform.up.y, -1.0f) == true) return 6;
        else if (CheckRange(this.transform.right.y, 1.0f) == true) return 2;
        else if (CheckRange(this.transform.right.y, -1.0f) == true) return 5;
        else if (CheckRange(this.transform.forward.y, 1.0f) == true) return 3;
        else if (CheckRange(this.transform.forward.y, -1.0f) == true) return 4;

        return 1;
    }

    public void Push()
    {
        transform.Translate(0, -0.1f, 0, Space.World);
    }

    public void Pop()
    {
        transform.Translate(0, 0.1f, 0, Space.World); 
    }
}