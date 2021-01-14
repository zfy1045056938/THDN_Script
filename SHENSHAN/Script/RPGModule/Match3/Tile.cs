using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Mathematics;

using UnityEngine.EventSystems;

public enum TileType{
    None,
    Breakable,
    Normal,
    Obstcle,
}
public class Tile : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler,IPointerUpHandler
{
    public TileType tileType = TileType.Normal;

    public int xIndex;
    public int yIndex;

    private Board board;

    private SpriteRenderer m_sprite;

    public int breakableValue = 0;

    public Sprite[] breakableSprites;
    public Color normalColor;

    public GamePieces pieces;
   

    private void Awake()
    {
        m_sprite = FindObjectOfType<SpriteRenderer>();
    }

    public void Init(int x, int y, Board board)
    {
        this.xIndex = x;
        this.yIndex = y;
        this.board = board;

        if (tileType == TileType.Breakable)
        {
            if (breakableSprites[breakableValue] != null)

            {
                m_sprite.sprite = breakableSprites[breakableValue];
            }
        }
    }


    void OnMouseDown()
    {
        if(board!=null){
             board.ClickTile(this);
        }
    }

    void OnMouseUp()
    {
        if (board != null)
        {
            board.ReleaseTile();
        }
    }

   

    public void BreakTile()
    {
        if (tileType == TileType.Breakable)
        {
            return;
        }
        StartCoroutine(BreakTileRoutine());
    }

   

    public IEnumerator BreakTileRoutine()
    {
        breakableValue = math.clamp(breakableValue--, 0, breakableValue);
        //
        yield return new WaitForSeconds(0.4f);
        //
        if (breakableSprites[breakableValue] != null)
        {
            m_sprite.sprite = breakableSprites[breakableValue];
        }
        //
        if (breakableValue == 0)
        {
            tileType = TileType.Normal;
            m_sprite.color=normalColor;
        }
    }

    private void OnMouseEnter()
    {
        
        if (board != null)
        {
            board.DragToTile(this);
        }
    }

    /// <summary>
/// Move To Target Tile
/// </summary>
/// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (board != null&& eventData.IsPointerMoving())
        {
         
            board.DragToTile(this);
        }
        }

/// <summary>
/// Click the 
/// </summary>
/// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (board != null)
            {
                Debug.Log("U Click " + this.name);
                board.ClickTile(this);
            }
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    { 
Debug.Log("Up");        
        if (board != null && eventData.pointerEnter)
        {
            board.ReleaseTile();
        }
    }

   
}
