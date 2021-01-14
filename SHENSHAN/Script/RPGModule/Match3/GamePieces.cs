using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Mathematics;

public enum InterType{
    Linear,
    SmootherStep,
}

public enum MatchValue{
    None,
    Red,
    MUT,
}

public class GamePieces : MonoBehaviour
    {
        public int xIndex;
        public int yIndex;
        
            private Board m_board;
        private bool is_isMoving = false;

        public InterType inter = InterType.SmootherStep;
        
        //Score
        public int scoreValue = 20;

        public AudioClip clearSound;
        
        
        
        
        public MatchValue matchValue=MatchValue.MUT;

        
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

        ///        
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
