using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CScoreManager
{
    public static int m_nMaxChain = 0;

    public static long GetScore(int direction, int defaultCount, int bonusCount,int nChain)
    {
        if (nChain == 0)
        {
            //기본 점수
            //A = 주사위 번호
            //B = 처음 사라지는 주사위 개수
            //X = 기본 점수
            //X = ( A + ( B – A ) ) * B
            long score = (direction + (defaultCount - direction)) * defaultCount;
			//long score = (direction + (defaultCount - direction)) * defaultCount;
			//Debug.Log("DIR = " + direction + ","+ "DEF = " + defaultCount + "," + "SCORE = " + score);
            return score;
        }
        else
        {
            //A = 체인 성공한 주사위 숫자
            //B = 체인 횟수
            //X = 체인 점수
            //X = A * B + B^2
			//20140421
			//X = 체인 주사위 번호 * 체인횟수^2 + 체인횟수(보너스점수)

			long score = (long)(Math.Pow((direction - 1), 2) * Math.Pow(nChain, 2)) + nChain;
			//20140626 backup long score = (long)((direction - 1) * Math.Pow(direction * 2 + nChain, 2)) + nChain;
			//20140624 backup long score = (long)(direction * Math.Pow(nChain, 2)) + nChain;
			//long score = (long)((direction + (defaultCount - direction)) * defaultCount + Math.Pow(nChain, 2)) * nChain;
			//Debug.Log("DIR = " + direction + "," + "CHAIN = " + nChain + "," + "SCORE = " + score);
            return score;
        }
    }
}