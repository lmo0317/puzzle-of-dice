using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

    private int m_nState = GameData.OBJECT_STATE_NONE;
    private int m_nSpeed = GameData.OBJECT_CURSOR_MOVE_SPEED;
    private int m_nMoveCount = 0;
	private Vector3 m_vecStartPosition;
	private bool m_bEvent;
    public GameObject m_Model;
    private Cat m_ModelScript;
    public int getState() { return m_nState; }

    public void Clear()
    {
        m_ModelScript.Stop();
        m_nState = GameData.OBJECT_STATE_NONE;
        m_nMoveCount = 0;
    }

 	void Start ()
	{
        m_ModelScript = m_Model.GetComponent("Cat") as Cat;
	}
	
	void Update ()
	{
		if( m_nState == GameData.OBJECT_STATE_MOVE_RIGHT )
		{
			MoveRight();
		}
		else if( m_nState == GameData.OBJECT_STATE_MOVE_LEFT )
		{
			MoveLeft();
		}
		else if( m_nState == GameData.OBJECT_STATE_MOVE_UP )
		{
			MoveUp();
		}
        else if (m_nState == GameData.OBJECT_STATE_MOVE_DOWN)
        {
            MoveDown();
        }
	}
	
	public void MoveCursor(int direction,bool bDiceMove)
	{
        MoveCursor(direction, GameData.OBJECT_CURSOR_MOVE_SPEED,bDiceMove);
	}
	
	public void MoveCursor(int direction, int speed,bool bDiceMove)
	{
        Vector3[] moveRotate = { new Vector3(0.0f, 90.0f, 0.0f), 
                                   new Vector3(0.0f, 270.0f, 0.0f ), 
                                   new Vector3(0.0f, 0.0f, 0.0f), 
                                   new Vector3(0.0f, 180.0f, 0.0f)};

        Vector3[] movePos = { new Vector3(1.0f, 0.0f, 0.0f), 
                                   new Vector3(-1.0f, 0.0f, 0.0f ), 
                                   new Vector3(0.0f, 0.0f, 1.0f), 
                                   new Vector3(0.0f, 0.0f, -1.0f)};

    	m_nState = direction;
		m_nMoveCount = 0;
		m_nSpeed = speed;
        m_vecStartPosition = transform.position;
		m_bEvent = true;

        //다음 움직일 위치에 블록이 있으면 점프
        //다음 움직일 위치에 블록이 없고 현재 블록 위에 있다면 밑으로 떨어짐 
        if (bDiceMove)
        {
            //바닥에 주사위가 있는지 없는지 검사
            Board board = MainGame.g_Instance.getBoard();
            DiceSet diceSetCurrent = board.GetDice(transform.position);
            DiceSet diceSetNext = board.GetDice(transform.position + movePos[m_nState - 1]);

            if (diceSetCurrent == null && diceSetNext != null)
            {
                m_ModelScript.JumpUp(diceSetCurrent, diceSetNext);
            }
            else
            {
                m_ModelScript.Move(diceSetCurrent,diceSetNext);
            }
        }
        else
        {
            Board board = MainGame.g_Instance.getBoard();
            DiceSet diceSetCurrent = board.GetDice(transform.position);
            if (diceSetCurrent != null)
            {
                //현재 위치에 주사위가 있을경우
                DiceSet diceSetNext = board.GetDice(transform.position + movePos[m_nState - 1]);
                if (diceSetNext != null)
                {
                    //다음 위치에 주사위가 있을경우 걷는 SKY_MOVE
                    m_ModelScript.Move(diceSetCurrent,diceSetNext);
                }
                else
                {
                    //다음 위치에 주사위가 없을경우 떨어지는 FALL
                    m_ModelScript.JumpDown(diceSetCurrent, diceSetNext);
                }
            }
            else
            {
                //현재 위치에 주사위가 없을경우
                DiceSet diceSetNext = board.GetDice(transform.position + movePos[m_nState - 1]);
                if (diceSetNext != null)
                {
                    //다음 위치에 주사위가 있을경우 JUMP
                    m_ModelScript.JumpUp(diceSetCurrent, diceSetNext);
                }
                else
                {
                    //다음 위치에 주사위가 없을경우 MOVE
                    m_ModelScript.Move(diceSetCurrent, diceSetNext);
                }
            }
        }
        
        m_Model.transform.rotation = Quaternion.Euler(moveRotate[m_nState - 1]);        
	}
	
	public void MoveCursorNoEvent(int direction,bool bDiceMove)
	{
        MoveCursorNoEvent(direction, GameData.OBJECT_CURSOR_MOVE_SPEED,bDiceMove);
	}
	
	public void MoveCursorNoEvent(int direction, int speed,bool bDiceMove)
	{
        MoveCursor(direction, GameData.OBJECT_CURSOR_MOVE_SPEED,bDiceMove);
		m_bEvent = false;
	}
	
	void MoveRight ()
	{
		m_nMoveCount++;
        transform.Translate(1.0f / m_nSpeed, 0.0f, 0.0f, Space.World);

		if( m_nMoveCount >= m_nSpeed )
		{
			m_nState = GameData.OBJECT_STATE_NONE;
            transform.position = new Vector3(m_vecStartPosition.x + 1.0f, 0.0f, m_vecStartPosition.z);
			MoveEnd();
		}
	}
	
	void MoveLeft ()
	{
		m_nMoveCount++;
        transform.Translate(-1.0f / m_nSpeed, 0.0f, 0.0f, Space.World);
		if( m_nMoveCount >= m_nSpeed )
		{
			m_nState = GameData.OBJECT_STATE_NONE;
            transform.position = new Vector3(m_vecStartPosition.x - 1.0f, 0.0f, m_vecStartPosition.z);	
			MoveEnd();
		}
	}
	
	void MoveUp ()
	{
		m_nMoveCount++;
        transform.Translate(0.0f, 0.0f, 1.0f / m_nSpeed, Space.World);
		if( m_nMoveCount >= m_nSpeed )
		{
			m_nState = GameData.OBJECT_STATE_NONE;
            transform.position = new Vector3(m_vecStartPosition.x, 0.0f, m_vecStartPosition.z + 1.0f);
			MoveEnd();
		}
	}
	
	void MoveDown ()
	{
		m_nMoveCount++;
        transform.Translate(0.0f, 0.0f, -(1.0f / m_nSpeed), Space.World);
		if( m_nMoveCount >= m_nSpeed )
		{
			m_nState = GameData.OBJECT_STATE_NONE;
            transform.position = new Vector3(m_vecStartPosition.x, 0.0f, m_vecStartPosition.z - 1.0f);
			MoveEnd();
		}
	}

    void MoveCursorEnd()
    {

    }

    void MoveDiceEnd()
    {

    }
	
	void MoveEnd()
	{
        //Debug.Log("Cursor Move End");

        if (m_bEvent == true) {
            MainGame.g_Instance.OnMoveEnd(transform.position);
        }

        m_ModelScript.OnMoveEnd();
	}

    public bool IsCursor(int x, int z)
    {
        if (transform.position.x == x && transform.position.z == z)
        {
            return true;
        }

        return false;
    }
}
