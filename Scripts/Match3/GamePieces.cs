using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Mathematics;
using TMPro;
using System.Reflection;
using System.Linq;


public enum GPType
{
    None,
    STR,
    DEX,
    INTE,
    CON,
    VOID,
    CHAOS,
    ATk,
    ARMOR,
    HP,
    MANA,
}

[System.Serializable]
public struct GamePiece
{
    public int gid;
    public string gName;
    public Sprite gSprite;
    public GPType gpType;
    public string gDetail;

    public GamePiece(int gid, string gName, Sprite gSprite, GPType gpType, string gDetail)
    {
        this.gid = gid;
        this.gName = gName;
        this.gSprite = gSprite;
        this.gpType = gpType;
        this.gDetail = gDetail;
    }

}

/// <summary>
/// cps:: when matches  pieces += dic[gps];
/// board: matches clear gps
/// TODO
/// </summary>
public class GamePieces : MonoBehaviour
    { 
        public int xIndex;
        public int yIndex;
    public int amount = 0;  //default amount can infulence by buffs or others
    public GamePiece gp;
            private Board m_board;
        private bool is_isMoving = false;

        public InterType inter = InterType.SmootherStep;
        
        //Score
        public int scoreValue = 20;

       
   
    //cps amount ,when use target skill reduce skill required gps
    
        public GPType matchValue=GPType.None;

        
        public void Init(Board board)
        {
          this.m_board=board;
        }
        
        
        public void Move(int x, int y, float moveTime)
        {
            if (!is_isMoving)
            {
                 StartCoroutine(MoveRoutline(new Vector3(x, y,0), moveTime));
            }
           
        }

        
        private IEnumerator MoveRoutline(Vector3 pos, float moveTime)
        {
            Vector3 startPos = transform.position;
            //
            bool reachedDestination = false;

            float elapsedTime = 0f;

            is_isMoving = true;

            //
            while (!reachedDestination)
            {
                if (Vector3.Distance(  transform.position, pos) < 0.01f)
                {
                    reachedDestination = true;
                    //
                    if (m_board != null)
                    {
                        m_board.PlaceGamePieces(this, (int) pos.x, (int) pos.y);
                    }

                    break;
                }


                //
                elapsedTime += Time.deltaTime;

                float t = math.clamp(elapsedTime / moveTime, 0f, 1f);

                switch (inter)
                {
                    case InterType.Linear:
                        break;
                    case InterType.SmootherStep:
                        t=t*t*(3-2*t);
                        break;
                    default:
                        break;
                }

                //
                transform.position = Vector3.Lerp(startPos, pos, t);
                //
                yield return null;
            }

            is_isMoving = false;
        }
    

        public void SetCoord(int gpX, int gpY)
        {
            this.xIndex = gpX;
            this.yIndex = gpY;
        }

        public GamePieces ChangeColor(GamePieces targetPiece)
        {
            return null;
        }

        public void ScorePoints(int mScoreMultiplier, int bonus)
        {
            throw new System.NotImplementedException();
        }
    }
