// board object

using UnityEngine;
using System.Collections;

public class DiceSet
{
    public const int EMITTER_TYPE_NONE = 0;
    public const int EMITTER_TYPE_NEXT = 1;
    public const int EMITTER_TYPE_CURRENT = 2;
    public const int EMITTER_TYPE_DONE = 3;

    public GameObject dice = null;
	public Dice script = null;
	public bool checking = false;
	public int nChain = 0;
    public int nEmitterType = EMITTER_TYPE_NONE;
}

public class Board : MonoBehaviour
{
	private DiceSet[] m_aBoards = null;
	
    //effect
    private float m_fFlashTimeStamp = 0.0f;
    private float m_fWaveEffectTimeStamp = 0.0f;
    private bool m_bFlash = false;
    private bool m_bWaveEffect = false;

    //count
    private int m_nDiceCount = 0;
	
    //dice check
    private ArrayList m_aCheckDice = null;
    private ArrayList m_aWaveEffectCheckDice = null;

    //object
	public GameObject Dice;
	public GameObject Floor;
    public GameObject Floor2;


    //Count - GameResult
    public int[] result_DiceN = null;
    public int result_maxChain = 0;
    public long result_maxChainScore = 0;
	
	void Start ()
	{
		CreateBoardMesh();
	}
	
	void Update ()
	{
        diceFlashEffect();
        ChainEffectProcess();
	}
	
	void CreateBoardMesh()
	{
		for(int i=0;i<GameData.BOARD_SIZE_WIDTH;++i)
		{
			for(int j=0;j<GameData.BOARD_SIZE_HEIGHT;++j)
			{
				if(j % 2 == 0 && i % 2 == 0) {
                    Instantiate(Floor, new Vector3(i, -1.7f, j), Quaternion.Euler(new Vector3(-90, 0, 0)));
				}
				else  {
                    Instantiate(Floor2, new Vector3(i, -1.7f, j), Quaternion.Euler(new Vector3(-90, 0, 0)));
				}
			}
		}
	}
	
	public void initBoards()
	{
        result_DiceN = new int[7];
        for (int i = 0; i < result_DiceN.Length; i++)
        {
            result_DiceN[i] = 0;
        }
        result_maxChain = 0;
        result_maxChainScore = 0;

		m_nDiceCount = 0;		
		if(m_aBoards != null)
		{
			for( int i = 0; i < m_aBoards.Length; i++)
			{
				if(m_aBoards[i] != null)
				{
					if(m_aBoards[i].dice != null)
					{
						Destroy(m_aBoards[i].dice);
					}
				}
				m_aBoards[i] = null;					
			}
		}
		
		m_aBoards = new DiceSet[GameData.BOARD_SIZE_WIDTH*GameData.BOARD_SIZE_HEIGHT];		
		for( int i = 0; i < m_aBoards.Length; i++ )
		{
			m_aBoards[i] = null;
		}

        m_aCheckDice = new ArrayList();
        m_aCheckDice.Clear();

        m_aWaveEffectCheckDice = new ArrayList();
        m_aWaveEffectCheckDice.Clear();
	}
	
	public void makeStartDice()
	{
        for (int i = 0; i < GameData.CREATE_DICE_COUNT; ++i)
        {
            //Vector3 vecPos = MainGame.g_Instance.GetCursor().transform.position;
            //Debug.Log("Start Make Dice [ " + vecPos.x + "," + vecPos.y + "," + vecPos.z + " ]");
            CreateDice(true);
        }
	}
	
	public void CreateDice(bool bPreCreate)
	{
		if( m_nDiceCount >= GameData.DICE_TOTAL_COUNT )
		{
			// game end
		}
		else 
		{
			int x = 0, y = 0, dir = 0;
			do
			{
                x = Random.Range(0, GameData.BOARD_SIZE_WIDTH);
                y = Random.Range(0, GameData.BOARD_SIZE_HEIGHT);
                //x,y 위치 dice 가 null이 아니고 
			}
			while( GetDice(x, y) != null || MainGame.g_Instance.GetCursor().IsCursor(x,y) == true);

            //Debug.Log("POSITION X = " + x + "," + "Y = " + y);
            //Debug.Log("CURSOR X = " + MainGame.g_Instance.GetCursor().transform.position.x + "," + "Y = " + MainGame.g_Instance.GetCursor().transform.position.z);
			
			do
			{
                dir = Random.Range(1, GameData.MAX_DIRECTION + 1);
			}
			while( CheckCreateDirection(x, y, dir) == false );

            //Vector3 vecPos = MainGame.g_Instance.GetCursor().transform.position;
            //Debug.Log("Start Make Dice [ " + vecPos.x + "," + vecPos.y + "," + vecPos.z + " ]");
            
            CreateDice(x, y, dir, bPreCreate);
		}
	}

    public DiceSet CreateDice(int x, int y, int direction, bool bPreCreate)
	{
		DiceSet dices = new DiceSet();
		dices.dice = Instantiate(Dice, new Vector3((float)x, -1, (float)y), Quaternion.identity) as GameObject;
		dices.script = dices.dice.GetComponent("Dice") as Dice;
		dices.script.SetDirection(direction);

        if (bPreCreate)
        {
            dices.script.AppearDiceOneShot();
        }
        else
        {
            dices.script.AppearDice();
        }
		SetDice(x, y, dices);
		m_nDiceCount++;
        return dices;
	}
	
	public void DestroyDice(Vector3 pos)
	{
		DestroyDice(pos.x, pos.z);
	}
	
	public void DestroyDice(float x, float y)
	{
		DestroyDice((int)x, (int)y);
	}
	
	public void DestroyDice(int x, int y)
	{
		SetDice(x, y, null);
		m_nDiceCount--;
	}
	
	public DiceSet GetDice(Vector3 pos)
	{
		return GetDice(pos.x, pos.z);
	}
	
	public DiceSet GetDice(float x, float y)
	{
		return GetDice((int)x, (int)y);
	}
	
	public DiceSet GetDice(int x, int y)
	{
        if (m_aBoards == null)
            return null;

		return m_aBoards[GameData.BOARD_SIZE_WIDTH*y+x];
	}
	
	public void SetDice(Vector3 pos, DiceSet obj)
	{
		SetDice(pos.x, pos.z, obj);
	}
	
	public void SetDice(float x, float y, DiceSet obj)
	{
		SetDice((int)x, (int)y, obj);
	}

    public void RemoveDice(int x, int y)
    {
        DiceSet _diceSet = GetDice(x, y);
        if (_diceSet != null)
        {
            _diceSet.script.Remove();
            DestroyDice(5, 1);
        }
    }
	
	public void SetDice(int x, int y, DiceSet obj)
	{
        m_aBoards[GameData.BOARD_SIZE_WIDTH * y + x] = obj;
	}
	
	public void MoveDice(Vector3 pos, int direction, DiceSet obj)
	{
		SetDice(pos, null);
		if( direction == GameData.EVENT_DIRECTION_RIGHT )
		{
			SetDice(pos.x + 1, pos.z, obj);
		}
		else if( direction == GameData.EVENT_DIRECTION_LEFT )
		{
			SetDice(pos.x - 1, pos.z, obj);
		}
		else if( direction == GameData.EVENT_DIRECTION_UP )
		{
			SetDice(pos.x, pos.z + 1, obj);
		}
		else if( direction == GameData.EVENT_DIRECTION_DOWN )
		{
			SetDice(pos.x, pos.z - 1, obj);
		}
	}
	
	public long CheckDices(Vector3 pos)
	{
		return CheckDices((int)pos.x, (int)pos.z);
	}
	
	public long CheckDices(int x, int y)
	{
        long score = 0;
        DiceSet diceSet = GetDice(x, y);
        if(diceSet == null)
        {
            return 0;
        }

		int direction = diceSet.script.GetDirection();
		if( direction > 1 )
		{
            diceSet.checking = true;
            m_aCheckDice.Add(diceSet);
	
			//주사위 기준으로 4방향을 재귀로 검사한다.
			CheckDiceSide(x+1, y, direction);
			CheckDiceSide(x-1, y, direction);
			CheckDiceSide(x, y+1, direction);
			CheckDiceSide(x, y-1, direction);
			
			//인접한 모든 같은 방향 주사위 정보 m_aCheckDice
			int defaultCount = 0;
			int bonusCount = 0;
            int nCount = 0;
            int nMaxChain = 0;

            for (int i = 0; i < m_aCheckDice.Count; i++)
			{
                DiceSet ds = (DiceSet)m_aCheckDice[i];
                if (m_aCheckDice.Count >= direction)
				{
					if( ds.script.GetState() == GameData.OBJECT_STATE_DISAPPEAR )
					{
                    	bonusCount++;
						ds.script.PlusDisappearTime();
					}
					else
					{
						defaultCount++;
						ds.script.DisappearDice();
					}
                    nCount++;
				}				
				//검사된 모든 주사위의 체크를 풀어준다.
				ds.checking = false;
			}

            if (defaultCount != 0 || bonusCount != 0)
            {
                if (defaultCount != 0 && bonusCount == 0)
                {
                    //처음 주사위 맞출때
                    SoundManager.g_Instance.PlayEffectSound("dice_clear_01");
                    result_DiceN[direction] += direction;
                }
                else if (defaultCount != 0 && bonusCount != 0)
                {
                    //체인 으로 맞출때
                    SoundManager.g_Instance.PlayEffectSound("chain_success_01");
                    string strName = "chain_random";
                    strName += "_";                    
                    strName += Random.Range(1,7);
                    SoundManager.g_Instance.PlayEffectSound(strName);

                    for (int i = 0; i < m_aCheckDice.Count; ++i)
                    {
                        DiceSet ds = (DiceSet)m_aCheckDice[i];
                        ds.nChain++;
                        if(ds.nChain > nMaxChain)
                        {
                            nMaxChain = ds.nChain;
                        }
                    }

                    for(int i=0;i<m_aCheckDice.Count;++i) {
                        DiceSet ds = (DiceSet)m_aCheckDice[i];
                        ds.nChain = nMaxChain;
                    }

                    ClearWaveEffect();

                    diceSet.nEmitterType = DiceSet.EMITTER_TYPE_CURRENT;
                    m_aWaveEffectCheckDice = new ArrayList(m_aCheckDice);
                    ChainEffect(nMaxChain);

                    if (nMaxChain > CScoreManager.m_nMaxChain)
                    {
                        CScoreManager.m_nMaxChain = nMaxChain;
                    }

                    result_DiceN[direction] += 1;
                }

                score = CScoreManager.GetScore(direction, defaultCount, bonusCount, nMaxChain);

                //Debug.Log("? : " + result_DiceN.Length);

                if( ScoreLabelObj.instance != null) {
                    ScoreLabelObj.instance.createScoreLabel(score);
                }

                //result_DiceN[0] += 1;
                //result_DiceN[direction] += 1;
                if (nMaxChain > result_maxChain)
                {
                    result_maxChain = nMaxChain;
                }
                if(nMaxChain > 0 && score > result_maxChainScore)
                {
                    result_maxChainScore = score;
                }


                //Debug.Log("TEST !!!");
                //for(int i = 0 ; i < result_DiceN.Length; i++)
                //{
                //    Debug.Log(i + " : " + result_DiceN[i]);
                //}
                //Debug.Log("maxChain : " + result_maxChain);
                //Debug.Log("maxChainScore : " + result_maxChainScore);
            }
            
            m_aCheckDice.Clear();
		}
		
		return score;
	}

    public bool IsEmpty()
    {        
        for (int i = 0; i < GameData.BOARD_SIZE_HEIGHT; i++)
        {
            for (int j = 0; j < GameData.BOARD_SIZE_WIDTH; j++)
            {
                DiceSet diceSet = GetDice(i, j);
                if (diceSet != null)
                {
                    return false;
                }
            }
        }

        return true;   
    }

    public bool IsAlmostEmpty()
    {
        for (int i = 0; i < GameData.BOARD_SIZE_HEIGHT; i++)
        {
            for (int j = 0; j < GameData.BOARD_SIZE_WIDTH; j++)
            {
                DiceSet diceSet = GetDice(i, j);
                if (diceSet != null)
                {
                    if (diceSet.script.GetState() != GameData.OBJECT_STATE_DISAPPEAR)
                        return false;

                    if (diceSet.script.getPercent() < 0.8f)
                        return false;
                }
            }
        }

        return true;
    }

    public bool IsFull()
    {
        for (int i = 0; i < GameData.BOARD_SIZE_HEIGHT; i++)
        {
            for (int j = 0; j < GameData.BOARD_SIZE_WIDTH; j++)
            {
                DiceSet diceSet = GetDice(i, j);
                if (diceSet == null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    void ChainEffect(int nChain)
    {
        m_fWaveEffectTimeStamp = GameData.WAVE_EFFECT_TIME;
        m_bWaveEffect = true;
        if (Chain.chainQueue != null)
        {
            Chain.chainQueue.Enqueue(nChain);
        }
    }

    void ClearWaveEffect()
    {
        for (int i = 0; i < m_aWaveEffectCheckDice.Count; i++)
        {
            DiceSet ds = (DiceSet)m_aWaveEffectCheckDice[i];
            ds.nEmitterType = DiceSet.EMITTER_TYPE_NONE;
        }
    }

    void ChainEffectProcess()
    {
        if (m_bWaveEffect == false)
        {
            return;
        }

        m_fWaveEffectTimeStamp += Time.deltaTime;
        if (m_fWaveEffectTimeStamp > GameData.WAVE_EFFECT_TIME)
        {
            m_fWaveEffectTimeStamp = 0;
            bool bHaveEffect = false;
            for (int i = 0; i < m_aWaveEffectCheckDice.Count; i++)
            {
                DiceSet ds = (DiceSet)m_aWaveEffectCheckDice[i];
                if (ds == null)
                    continue;

                if (ds.dice == null)
                    continue;

                //현재 emitter가 켜져 있을경우
                if (ds.nEmitterType == DiceSet.EMITTER_TYPE_CURRENT)
                {
                    bHaveEffect = true;
                    int x = (int)ds.dice.transform.position.x;
                    int y = (int)ds.dice.transform.position.z;
                    ChainEffectSide(x - 1, y);
                    ChainEffectSide(x + 1, y);
                    ChainEffectSide(x, y - 1);
                    ChainEffectSide(x, y + 1);

                    ds.script.waveEffectBegin();
                    ds.nEmitterType = DiceSet.EMITTER_TYPE_DONE;
                }
            }

            if (bHaveEffect == false)
            {
                ClearWaveEffect();
                m_bWaveEffect = false;
            }

            checkDiceEffectState();
        }
    }

    void checkDiceEffectState()
    {
        for (int i = 0; i < m_aWaveEffectCheckDice.Count; i++)
        {
            DiceSet ds = (DiceSet)m_aWaveEffectCheckDice[i];
            if (ds.nEmitterType == DiceSet.EMITTER_TYPE_NEXT)
            {
                ds.nEmitterType = DiceSet.EMITTER_TYPE_CURRENT;
            }
        }
    }

    void ChainEffectSide(int x,int y)
    {
        if (x >= 0 && y >= 0 && x < GameData.BOARD_SIZE_WIDTH && y < GameData.BOARD_SIZE_HEIGHT)
        {
            DiceSet ds = GetDice(x, y);
            if (ds != null && ds.nEmitterType == DiceSet.EMITTER_TYPE_NONE)
            {
                ds.nEmitterType = DiceSet.EMITTER_TYPE_NEXT;
            }
        }
    }

    void diceFlashEffect()
    {
        m_fFlashTimeStamp += Time.deltaTime;
        if (m_fFlashTimeStamp > GameData.FLASH_TIME)
        {
            m_bFlash = !m_bFlash;
            m_fFlashTimeStamp = 0;
            for (int i = 0; i < GameData.BOARD_SIZE_HEIGHT; ++i)
            {
                for (int j = 0; j < GameData.BOARD_SIZE_WIDTH; ++j)
                {
                    DiceSet diceSet = GetDice(j, i);
                    if (diceSet != null && diceSet.script.GetState() == GameData.OBJECT_STATE_DISAPPEAR)
                    {
                        if (m_bFlash)
                            diceSet.script.doEffect(GameData.OBJECT_EFFECT_STATE_CLEAR_ON);
                        else
                            diceSet.script.doEffect(GameData.OBJECT_EFFECT_STATE_CLEAR_OFF);

                    }
                }
            }
        }
    }
	
	void CheckDiceSide(int x, int y, int direction)
	{
		if( x >= 0 && y >= 0 && x < GameData.BOARD_SIZE_WIDTH && y < GameData.BOARD_SIZE_HEIGHT )
		{
			DiceSet diceSet = GetDice(x, y);
			if( diceSet != null )
			{
				if( diceSet.script.GetDirection() == direction )
				{
					if( diceSet.checking == false )
					{
						if( diceSet.script.GetState() != GameData.OBJECT_STATE_APPEAR )
						{
							diceSet.checking = true;
                            m_aCheckDice.Add(diceSet);
							
							CheckDiceSide(x+1, y, direction);
							CheckDiceSide(x-1, y, direction);
							CheckDiceSide(x, y+1, direction);
							CheckDiceSide(x, y-1, direction);
						}
					}
				}
			}
		}
	}
	
	bool CheckCreateDirection(int x, int y, int direction)
	{
		if( GetDiceDirection(x+1, y) == direction )
			return false;
		
		if( GetDiceDirection(x-1, y) == direction )
			return false;
		
		if( GetDiceDirection(x, y+1) == direction )
			return false;
		
		if( GetDiceDirection(x, y-1) == direction )
			return false;
		
		return true;
	}
	
	int GetDiceDirection(int x, int y)
	{
		if( x >= 0 && y >= 0 && x < GameData.BOARD_SIZE_WIDTH && y < GameData.BOARD_SIZE_HEIGHT )
		{
			DiceSet diceSet = GetDice(x, y);
			if( diceSet != null )
			{
				return diceSet.script.GetDirection();
			}
		}
		
		return 0;
	}
}
