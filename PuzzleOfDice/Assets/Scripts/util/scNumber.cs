// draw number

using UnityEngine;
using System.Collections;

public class scNumber : MonoBehaviour {
	
	private static Rect rect;
	
	public Texture2D texNum;
	
	private long mScore = 0;
	public int posX = 0;
	public int posY = 0;
	public int drawCount = 8;
	public TextAlignment anchor = TextAlignment.Left;
	public int interval = 0;
	public bool isDrawZero = false;
	public float magnification = 1.0f;
	public TextAlignment ScreenAlign;
	public bool isHide = false;
	public bool isUsePlus = false;
	public bool isUseMinus = false;
	
	private float oneImageW = (1.0f/12.0f);
	private int imgW = 0;
	private int imgH = 0;
	ArrayList arrResultNum = new ArrayList();
	ArrayList arrDivisionNum = new ArrayList();
		
	//public int[] sc = new int[3];
	//public string[] toolbarStrings = new string[] {"Toolbar1", "Toolbar2", "Toolbar3"};
	
		
	// Use this for initialization
	void Start () {
		//x position
		posX += getScreen( ScreenAlign );
		
		//use GUI image size
		imgW = texNum.width / 12;
		imgH = texNum.height;
		
		if( isDrawZero == true ){
			isUsePlus = false;
			isUseMinus = false;
		}
			
		//set devision value
		int temp = 1;
		arrDivisionNum.Add( temp );
		for( int i = 1 ; i < drawCount ; i++ )
		{
			temp = temp*10;
			arrDivisionNum.Add( temp );
		}
		/*
		for( i = 0 ; i < arrDivisionNum.Count ; i++ )
		{
			arrDivisionNum.Remove(i);
		}
		arrDivisionNum.Clear();*/
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	// Update GUI
	void OnGUI () {
		if( isHide == true ) return;
		//draw score
		drawScore( (int)mScore , posX , posY , drawCount , magnification , anchor , interval , isDrawZero , isUsePlus , isUseMinus );
	}
	
	/*
	 * 	@brief set score
	 * @param long score : score
	 * */
	public void setScore( long score )
	{
		mScore = score;
	}
	
	/*
	 * @brief Does not draw
	 * @param bool hide : (true)hide , (false)show
	 * */
	public void setHide( bool hide )
	{
		isHide = hide;
	}
	
	/*
	 * @brief set x position
	 * @param int x : x position
	 * */
	public void setPosX( int x )
	{
		posX = x;
	}
	
	/*
	 * @brief set y position
	 * @param int y : y position
	 * */
	public void setPosY( int y )
	{
		posY = y;
	}
	
	/*
	 * @brief set magnification
	 * @param float mag : magnification value ( 1.0f)
	 * */
	public void setMagnification( float mag )
	{
		magnification = mag;
	}
	
	/*
	 * @brief get Screen alignment value
	 * @param TextAlignment align
	 * */
	public int getScreen( TextAlignment align )
	{
		return align.Equals( TextAlignment.Center ) ? (Screen.width>>1) : ( align.Equals( TextAlignment.Right) ? (Screen.width) : 0 );
	}
	
	/*
	 * @brief draw score
	 * @param int score
	 * @param int x
	 * @param int y
	 * @param int count : draw number count
	 * @param int anchor : ( 0 = left , 1 = center , 2 = right )
	 * @param interval : draw number interval
	 * @param zero : draw zero number
	 * */
	void drawScore ( long score, int x, int y, int count, float magnification = 1.0f ,TextAlignment anchor = TextAlignment.Left
		, int interval = 0, bool zero = false , bool plus = false , bool minus = false )
	{	
		if( count <= 0 ) return;
		
		bool drawMinus = false;
		
		if( minus == true ){
			if( score < 0 ){
				score = score * (-1);
				drawMinus = true;
			}
		}
		else{
			if( score < 0 ) score = 0;
		}
			
		int i = 0;
		int j = 0;
		int drawFirstCount = -1;
		bool tempDrawZero = false;
		float drawX = x;
		int drawScore = (int)score;

		//set draw x postion
		if( anchor.Equals( TextAlignment.Left ) ) {}		
		else if( anchor.Equals( TextAlignment.Center ) ) drawX -= (( ((float)imgW*(float)magnification*(float)count)+((float)interval*magnification*(float)(count-1)) ) / 2);
		else if( anchor.Equals( TextAlignment.Right ) ) drawX -= ( ((float)imgW*(float)magnification*(float)count)+((float)interval*magnification*(float)(count-1)) );
		
		//set result value
		int temp = 0;
		for( i = 0 , j = count-1 ; i < count ; i++ , j-- )
		{
			temp = drawScore / (int)arrDivisionNum[j];
			drawScore -= ( temp * (int)arrDivisionNum[j] );
			arrResultNum.Add( temp );
		}
		
		//draw number
		//GUI.BeginGroup( new Rect(drawX,y,drawX+(imgW*count)+(interval*(count-1)),y+imgH) );
		//GUI.BeginGroup( new Rect( 0, y, Screen.width, y+imgH ) );
		
		for( i = 0 ; i < count ; i++ )
		{
			if( score == 0 && i == count-1 ){
				tempDrawZero = true;
			}
			
			if( tempDrawZero == false ){
				if( i == 0 ) tempDrawZero = zero;
				if( (tempDrawZero == false) && ((int)arrResultNum[i] != 0) ) tempDrawZero = true;
			}
			
			//draw number
			if( tempDrawZero == true || zero == true ){
				if( drawFirstCount == -1) drawFirstCount = i;
				rect.Set( drawX+((float)imgW*magnification*(float)i)+((float)interval*magnification*(float)i) , y , (float)imgW*magnification , (float)imgH*magnification );
				//GUI.DrawTexture( rect , (Texture2D)getNumberTexture( (int)arrResultNum[i] ) ); //12 image count 
				GUI.DrawTextureWithTexCoords( rect , (Texture2D)texNum, new Rect( oneImageW*((int)arrResultNum[i]),0.0f,oneImageW,1.0f)); //1 image
			}
		}
		
		//draw plus, minus
		if( zero == false ){
			if( drawFirstCount != -1 ){
				drawFirstCount--;
				rect.Set( drawX+((float)imgW*magnification*(float)drawFirstCount)+((float)interval*magnification*(float)drawFirstCount) , y 
					, (float)imgW*magnification , (float)imgH*magnification );
				if( plus == true && score != 0 ){
					GUI.DrawTextureWithTexCoords( rect , (Texture2D)texNum , new Rect( oneImageW*10,0.0f,oneImageW,1.0f) );
				}	
				if( drawMinus == true ){
					GUI.DrawTextureWithTexCoords( rect , (Texture2D)texNum , new Rect( oneImageW*11,0.0f,oneImageW,1.0f) );
				}
			}
		}
		
		//GUI.EndGroup();
		
		//release ArrayList
		for( i = 0 ; i < arrResultNum.Count ; i++ )
		{
			arrResultNum.Remove(i);
		}
		arrResultNum.Clear();
	}
}
