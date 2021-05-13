using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour {

    private Animator _playerAnim;
    private int m_nState = (int)CURSOR_STATE.CURSOR_STATE_NONE;
    private const float c_fHeight = 0.5f;
    private const float c_fLowHeight = -0.5f;
    private float m_fMoveTime = 0.0f;
    private float m_fJumpUpTarget = 0.0f;

	void Start () {
	    _playerAnim = GetComponent<Animator>();	
		SetState ((int)CURSOR_STATE.CURSOR_STATE_IDLE);        
	}
	
	void Update () {
        StateProcess();


	}

    private void SetState(int nState)
    {
        m_nState = nState;
        switch (m_nState)
        {
            case (int)CURSOR_STATE.CURSOR_STATE_JUMP_DOWN:
            case (int)CURSOR_STATE.CURSOR_STATE_RUN:
            case (int)CURSOR_STATE.CURSOR_STATE_JUMP_UP:
                _playerAnim.SetBool("run", true);
                break;
            default:
                _playerAnim.SetBool("run", false);
                break;
        }
    }

    public void StateProcess()
    {
        switch (m_nState)
        {
            case (int)CURSOR_STATE.CURSOR_STATE_JUMP_UP:
                JumpUpProcess();
                break;
            case (int)CURSOR_STATE.CURSOR_STATE_JUMP_DOWN:
                JumpDownProcess();
                break;
            case (int)CURSOR_STATE.CURSOR_STATE_RUN:
                RunProcess();
                break;
            case (int)CURSOR_STATE.CURSOR_STATE_IDLE:
                IdleProcess();
                break;
            default:
                break;

        }
    }

    public void IdleProcess()
    {
        Board board = MainGame.g_Instance.getBoard();
        if (board != null)
        {
            DiceSet diceSetCurrent = board.GetDice(transform.position);
            if (diceSetCurrent != null)
            {
                transform.position = new Vector3(transform.position.x, diceSetCurrent.script.transform.position.y + c_fHeight, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, c_fLowHeight, transform.position.z);
            }
        }
    }

    public void RunProcess()
    {
        Board board = MainGame.g_Instance.getBoard();
        if (board != null)
        {
            DiceSet diceSetCurrent = board.GetDice(transform.position);
            if (diceSetCurrent != null)
            {
                //Debug.Log(diceSetCurrent.script.transform.position.y);
                transform.position = new Vector3(transform.position.x, diceSetCurrent.script.transform.position.y + c_fHeight, transform.position.z);
            }
            else
            {
                if (MainGame.g_Instance.GetCursorEvent().target == GameData.EVENT_TARGET_CURSOR)
                {
                    transform.position = new Vector3(transform.position.x, c_fLowHeight, transform.position.z);
                }
            }
        }
    }

    public void JumpUpProcess()
    {
        float fDT = Time.deltaTime;
        m_fMoveTime += fDT;
        if (m_fMoveTime <= GameData.OBJECT_MOVE_TIME)
        {
            transform.Translate(0.0f, m_fJumpUpTarget * (fDT / GameData.OBJECT_MOVE_TIME), 0.0f, Space.World);
        }
    }

    public void JumpUpEnd()
    {
        //Debug.Log("Jump Up End");
        transform.position = new Vector3(transform.position.x, m_fJumpUpTarget, transform.position.z);
    }

    public void JumpDownProcess()
    {
        float fDT = Time.deltaTime;
        m_fMoveTime += fDT;
        if (m_fMoveTime <= GameData.OBJECT_MOVE_TIME)
        {
            //Debug.Log(c_fLowHeight * (fDT / GameData.OBJECT_MOVE_TIME));
            transform.Translate(0.0f, c_fLowHeight * (fDT / GameData.OBJECT_MOVE_TIME), 0.0f, Space.World);
        }
    }

    public void JumpDownEnd()
    {
        //Debug.Log("Jump Down End");
        transform.position = new Vector3(transform.position.x, c_fLowHeight, transform.position.z);
    }

    public void Move(DiceSet diceSetCurrent, DiceSet diceSetNext)
    {
        m_fMoveTime = 0;
        if (diceSetNext != null)    {
            m_fJumpUpTarget = diceSetNext.script.transform.position.y + c_fHeight;
            //Debug.Log("dice set next not null = " + m_fJumpUpTarget);
        }
        else {
            m_fJumpUpTarget = c_fHeight;
            //Debug.Log("dice set next null = " + m_fJumpUpTarget);
        }

        SetState((int)CURSOR_STATE.CURSOR_STATE_RUN);
    }

    public void JumpUp(DiceSet diceSetCurrent, DiceSet diceSetNext)
    {
        m_fMoveTime = 0;

        if (diceSetNext != null)
        {
            m_fJumpUpTarget = diceSetNext.script.transform.position.y + c_fHeight;
            Debug.Log("dice set next not null = " + m_fJumpUpTarget);
        }
        else
        {
            m_fJumpUpTarget = c_fHeight;
            Debug.Log("dice set next null = " + m_fJumpUpTarget);
        }

        SetState((int)CURSOR_STATE.CURSOR_STATE_JUMP_UP);
    }

    public void JumpDown(DiceSet diceSetCurrent, DiceSet diceSetNext)
    {
        m_fMoveTime = 0;
        if (diceSetNext != null)
        {
            m_fJumpUpTarget = diceSetNext.script.transform.position.y + c_fHeight;
            Debug.Log("dice set next not null = " + m_fJumpUpTarget);
        }
        else
        {
            m_fJumpUpTarget = c_fHeight;
            Debug.Log("dice set next null = " + m_fJumpUpTarget);
        }

        SetState((int)CURSOR_STATE.CURSOR_STATE_JUMP_DOWN);
    }

    public void OnMoveEnd()
    {
        //Debug.Log("OnMoveEnd()");
        switch (m_nState)
        {
            case (int)CURSOR_STATE.CURSOR_STATE_JUMP_UP:
                JumpUpEnd();
                break;
            case (int)CURSOR_STATE.CURSOR_STATE_JUMP_DOWN:
                JumpDownEnd();
                break;
            default:
                break;

        }

        SetState((int)CURSOR_STATE.CURSOR_STATE_IDLE);
    }

    public void Stop()
    {
        SetState((int)CURSOR_STATE.CURSOR_STATE_IDLE);
        transform.position = new Vector3(transform.position.x, c_fLowHeight, transform.position.z);
    }
}
