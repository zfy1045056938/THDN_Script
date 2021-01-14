    using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using Mirror;
    using UnityEngine.UI;
    using Random = Unity.Mathematics.Random;


/// <summary>
/// CORE MODULE
/// Include these module
///  Match Module
///  
/// 
/// </summary>
[RequireComponent((typeof(BoardDeadLock)))]
[RequireComponent((typeof(BoardShuffle)))]
public class Board : MonoBehaviour
{
   

 public int width;
    public int height;

    public int borderSize;
    public Camera matchcamera;
    public GameObject tileNormalPrefab;
    public GameObject tileObstaclePrefab;
    public GameObject[] GamePiecesPrefabs;

    public GameObject adjacentBombPrefab;
    public GameObject columnBombPrefab;
    public GameObject rowBombPrefab;
    public GameObject colorBombPrefab;

    public int maxCollectiables = 3;
    public int CollectiableCount = 0;

    [Range(0, 1)]
    public float chanceForCollectiable = 0.1f;
    public GameObject[] CollectiablePrefabs;

    GameObject m_clickedTileBomb;
    GameObject m_targetTileBomb;

    public float swapTime = 0.5f;

    Tile[,] m_allTiles;
    GamePieces[,] m_allGamePiecess;

    Tile m_clickedTile;
    Tile m_targetTile;

   public bool m_playerInputEnabled = true;

    public StartingObject[] startingTiles;
    public StartingObject[] startingGamePiecess;

    ParticleManager m_particleManager;

    public int fillYOffset = 10;
    public float fillMoveTime = 0.5f;

    int m_scoreMultiplier = 0;

    [System.Serializable]
    public class StartingObject
    {
        public GameObject prefab;
        public int x;
        public int y;
        public int z;
    }

    void Start()
    {
        m_allTiles = new Tile[width, height];
        m_allGamePiecess = new GamePieces[width, height];
        m_particleManager = GameObject.FindWithTag("ParticleManager").GetComponent<ParticleManager>();
        matchcamera = GameObject.FindGameObjectWithTag("MatchCamera").GetComponent<Camera>();

    }

    public void SetupBoard()
    {
        SetupTiles();
        SetupGamePiecess();
        List<GamePieces> startingCollectiables = FindAllCollectiables();
        CollectiableCount = startingCollectiables.Count;
        SetupCamera();
        FillBoard(fillYOffset, fillMoveTime);
    }

    void MakeTile(GameObject prefab, int x, int y, int z = 0)
    {
        if (prefab != null && IsWithinBounds(x, y))
        {
            GameObject tile = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            tile.name = "Tile (" + x + "," + y + ")";
            m_allTiles[x, y] = tile.GetComponent<Tile>();
            tile.transform.parent = transform;
            m_allTiles[x, y].Init(x, y, this);
        }
    }

    void MakeGamePieces(GameObject prefab, int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
    {
        if (prefab != null && IsWithinBounds(x, y))
        {
            prefab.GetComponent<GamePieces>().Init(this);
            PlaceGamePieces(prefab.GetComponent<GamePieces>(), x, y);

            if (falseYOffset != 0)
            {
                prefab.transform.position = new Vector3(x, y + falseYOffset, 0);
                prefab.GetComponent<GamePieces>().Move(x, y, moveTime);
            }

            prefab.transform.parent = transform;
        }
    }

    GameObject MakeBomb(GameObject prefab, int x, int y)
    {
        if (prefab != null && IsWithinBounds(x, y))
        {
            GameObject bomb = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            bomb.GetComponent<Bomb>().Init(this);
            bomb.GetComponent<Bomb>().SetCoord(x, y);
            bomb.transform.parent = transform;
            return bomb;
        }
        return null;
    }

    void SetupTiles()
    {
        foreach (StartingObject sTile in startingTiles)
        {
            if (sTile != null)
            {
                MakeTile(sTile.prefab, sTile.x, sTile.y, sTile.z);
            }

        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allTiles[i, j] == null)
                {
                    MakeTile(tileNormalPrefab, i, j);
                }
            }
        }
    }

    void SetupGamePiecess()
    {
        foreach (StartingObject sPiece in startingGamePiecess)
        {
            if (sPiece != null)
            {
                GameObject piece = Instantiate(sPiece.prefab, new Vector3(sPiece.x, sPiece.y, 0), Quaternion.identity) as GameObject;
                MakeGamePieces(piece, sPiece.x, sPiece.y, fillYOffset, fillMoveTime);
            }
        }
    }

    void SetupCamera()
    {
     
     
       matchcamera.transform.position  = new Vector3(0.86f, 3.96f, -10f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float verticalSize = (float)height / 2f + (float)borderSize;

        float horizontalSize = ((float)width / 2f + (float)borderSize) / aspectRatio;

        matchcamera.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;

    }

    GameObject GetRandomObject(GameObject[] objectArray)
    {
        int randomIdx =UnityEngine.Random.Range(0, objectArray.Length);
        if (objectArray[randomIdx] == null)
        {
            Debug.LogWarning("ERROR:  BOARD.GetRandomObject at index " + randomIdx + "does not contain a valid GameObject!");
        }
        return objectArray[randomIdx];
    }

    GameObject GetRandomGamePieces()
    {
        return GetRandomObject(GamePiecesPrefabs);
    }

    GameObject GetRandomCollectiable()
    {
        return GetRandomObject(CollectiablePrefabs);
    }

    public void PlaceGamePieces(GamePieces GamePieces, int x, int y)
    {
        if (GamePieces == null)
        {
            Debug.LogWarning("BOARD:  Invalid GamePieces!");
            return;
        }

        GamePieces.transform.position = new Vector3(x, y, 0);
        GamePieces.transform.rotation = Quaternion.identity;

        if (IsWithinBounds(x, y))
        {
            m_allGamePiecess[x, y] = GamePieces;
        }

        GamePieces.SetCoord(x, y);
    }

    bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    GamePieces FillRandomGamePiecesAt(int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
    {
        if (IsWithinBounds(x, y))
        {
            GameObject randomPiece = Instantiate(GetRandomGamePieces(), Vector3.zero, Quaternion.identity) as GameObject;
            MakeGamePieces(randomPiece, x, y, falseYOffset, moveTime);
            return randomPiece.GetComponent<GamePieces>();
        }
        return null;
    }

    GamePieces FillRandomCollectiableAt(int x, int y, int falseYOffset = 0, float moveTime = 0.1f)
    {
        if (IsWithinBounds(x, y))
        {
            GameObject randomPiece = Instantiate(GetRandomCollectiable(), Vector3.zero, Quaternion.identity) as GameObject;
            MakeGamePieces(randomPiece, x, y, falseYOffset, moveTime);
            return randomPiece.GetComponent<GamePieces>();
        }
        return null;
    }

    public void FillBoard(int falseYOffset = 0, float moveTime = 0.1f)
    {
        int maxInterations = 100;
        int iterations = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allGamePiecess[i, j] == null && m_allTiles[i, j].tileType != TileType.Obstcle)
                {
                    GamePieces piece = null;
                    if (j == height - 1 && CanAddCollectiable())
                    {
                        piece = FillRandomCollectiableAt(i, j, falseYOffset, moveTime);
                        CollectiableCount++;
                    }
                    else
                    {
                        piece = FillRandomGamePiecesAt(i, j, falseYOffset, moveTime);
                        iterations = 0;

                        while (HasMatchOnFill(i, j))
                        {
                            ClearPieceAt(i, j);
                            piece = FillRandomGamePiecesAt(i, j, falseYOffset, moveTime);
                            iterations++;

                            if (iterations >= maxInterations)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    bool HasMatchOnFill(int x, int y, int minLength = 3)
    {
        List<GamePieces> leftMatches = FindMatches(x, y, new Vector2(-1, 0), minLength);
        List<GamePieces> downwardMatches = FindMatches(x, y, new Vector2(0, -1), minLength);

        if (leftMatches == null)
        {
            leftMatches = new List<GamePieces>();
        }

        if (downwardMatches == null)
        {
            downwardMatches = new List<GamePieces>();
        }

        return (leftMatches.Count > 0 || downwardMatches.Count > 0);

    }

    public void ClickTile(Tile tile)
    {
        if (m_clickedTile == null)
        {
            m_clickedTile = tile;
            //Debug.Log("clicked tile: " + tile.name);
        }
    }

    public void DragToTile(Tile tile)
    {
        if (m_clickedTile != null && IsNextTo(tile, m_clickedTile))
        {
            m_targetTile = tile;
        }
    }

    public void ReleaseTile()
    {
        if (m_clickedTile != null && m_targetTile != null)
        {
            SwitchTiles(m_clickedTile, m_targetTile);
        }

        m_clickedTile = null;
        m_targetTile = null;
    }

    void SwitchTiles(Tile clickedTile, Tile targetTile)
    {
        StartCoroutine(SwitchTilesRoutine(clickedTile, targetTile));
    }

    IEnumerator SwitchTilesRoutine(Tile clickedTile, Tile targetTile)
    {
        if (m_playerInputEnabled)
        {
            GamePieces clickedPiece = m_allGamePiecess[clickedTile.xIndex, clickedTile.yIndex];
            GamePieces targetPiece = m_allGamePiecess[targetTile.xIndex, targetTile.yIndex];

            if (targetPiece != null && clickedPiece != null)
            {
                clickedPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);
                targetPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);

                yield return new WaitForSeconds(swapTime);

                List<GamePieces> clickedPieceMatches = FindMatchesAt(clickedTile.xIndex, clickedTile.yIndex);
                List<GamePieces> targetPieceMatches = FindMatchesAt(targetTile.xIndex, targetTile.yIndex);
                List<GamePieces> colorMatches = new List<GamePieces>();

                if (IsColorBomb(clickedPiece) && !IsColorBomb(targetPiece))
                {
                    clickedPiece.matchValue = targetPiece.matchValue;
                    colorMatches = FindAllMatchValue(clickedPiece.matchValue);
                }
                else if (!IsColorBomb(clickedPiece) && IsColorBomb(targetPiece))
                {
                    targetPiece.matchValue = clickedPiece.matchValue;
                    colorMatches = FindAllMatchValue(targetPiece.matchValue);
                }
                else if (IsColorBomb(clickedPiece) && IsColorBomb(targetPiece))
                {
                    foreach (GamePieces piece in m_allGamePiecess)
                    {
                        if (!colorMatches.Contains(piece))


                        
                        {
                            colorMatches.Add(piece);
                        }
                    }
                }

                if (targetPieceMatches.Count == 0 && clickedPieceMatches.Count == 0 && colorMatches.Count == 0)
                {
                    clickedPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);
                    targetPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);
                }
                else
                {
//                    if (GameManager.Instance != null)
//                    {
//                        GameManager.Instance.movesLeft--;
//                        GameManager.Instance.UpdateMoves();
//                    }

                    yield return new WaitForSeconds(swapTime);
                    Vector2 swipeDirection = new Vector2(targetTile.xIndex - clickedTile.xIndex, targetTile.yIndex - clickedTile.yIndex);
                    m_clickedTileBomb = DropBomb(clickedTile.xIndex, clickedTile.yIndex, swipeDirection, clickedPieceMatches);
                    m_targetTileBomb = DropBomb(targetTile.xIndex, targetTile.yIndex, swipeDirection, targetPieceMatches);

                    if (m_clickedTileBomb != null && targetPiece != null)
                    {
                        GamePieces clickedBombPiece = m_clickedTileBomb.GetComponent<GamePieces>();
                        if (!IsColorBomb(clickedBombPiece))
                        {
                            clickedBombPiece.ChangeColor(targetPiece);
                        }
                    }

                    if (m_targetTileBomb != null && clickedPiece != null)
                    {
                        GamePieces targetBombPiece = m_targetTileBomb.GetComponent<GamePieces>();

                        if (!IsColorBomb(targetBombPiece))
                        {
                            targetBombPiece.ChangeColor(clickedPiece);
                        }
                    }


                    ClearAndRefillBoard(clickedPieceMatches.Union(targetPieceMatches).ToList().Union(colorMatches).ToList());

                }
            }
        }

    }

    bool IsNextTo(Tile start, Tile end)
    {
        if (Mathf.Abs(start.xIndex - end.xIndex) == 1 && start.yIndex == end.yIndex)
        {
            return true;
        }

        if (Mathf.Abs(start.yIndex - end.yIndex) == 1 && start.xIndex == end.xIndex)
        {
            return true;
        }

        return false;
    }

    List<GamePieces> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength = 3)
    {
        List<GamePieces> matches = new List<GamePieces>();

        GamePieces startPiece = null;

        if (IsWithinBounds(startX, startY))
        {
            startPiece = m_allGamePiecess[startX, startY];
        }

        if (startPiece != null)
        {
            matches.Add(startPiece);
        }
        else
        {
            return null;
        }

        int nextX;
        int nextY;

        int maxValue = (width > height) ? width : height;

        for (int i = 1; i < maxValue - 1; i++)
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!IsWithinBounds(nextX, nextY))
            {
                break;
            }

            GamePieces nextPiece = m_allGamePiecess[nextX, nextY];

            if (nextPiece == null)
            {
                break;
            }
            else
            {
                if (nextPiece.matchValue == startPiece.matchValue && !matches.Contains(nextPiece) && nextPiece.matchValue != MatchValue.None)
                {
                    matches.Add(nextPiece);
                }
                else
                {
                    break;
                }
            }
        }

        if (matches.Count >= minLength)
        {
            return matches;
        }
			
        return null;

    }

    List<GamePieces> FindVerticalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePieces> upwardMatches = FindMatches(startX, startY, new Vector2(0, 1), 2);
        List<GamePieces> downwardMatches = FindMatches(startX, startY, new Vector2(0, -1), 2);

        if (upwardMatches == null)
        {
            upwardMatches = new List<GamePieces>();
        }

        if (downwardMatches == null)
        {
            downwardMatches = new List<GamePieces>();
        }

        var combinedMatches = upwardMatches.Union(downwardMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : null;

    }

    List<GamePieces> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<GamePieces> rightMatches = FindMatches(startX, startY, new Vector2(1, 0), 2);
        List<GamePieces> leftMatches = FindMatches(startX, startY, new Vector2(-1, 0), 2);

        if (rightMatches == null)
        {
            rightMatches = new List<GamePieces>();
        }

        if (leftMatches == null)
        {
            leftMatches = new List<GamePieces>();
        }

        var combinedMatches = rightMatches.Union(leftMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : null;

    }

    List<GamePieces> FindMatchesAt(int x, int y, int minLength = 3)
    {
        List<GamePieces> horizMatches = FindHorizontalMatches(x, y, minLength);
        List<GamePieces> vertMatches = FindVerticalMatches(x, y, minLength);

        if (horizMatches == null)
        {
            horizMatches = new List<GamePieces>();
        }

        if (vertMatches == null)
        {
            vertMatches = new List<GamePieces>();
        }
        var combinedMatches = horizMatches.Union(vertMatches).ToList();
        return combinedMatches;
    }

    List<GamePieces> FindMatchesAt(List<GamePieces> GamePiecess, int minLength = 3)
    {
        List<GamePieces> matches = new List<GamePieces>();

        foreach (GamePieces piece in GamePiecess)
        {
            matches = matches.Union(FindMatchesAt(piece.xIndex, piece.yIndex, minLength)).ToList();
        }
        return matches;

    }

    List<GamePieces> FindAllMatches()
    {
        List<GamePieces> combinedMatches = new List<GamePieces>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var matches = FindMatchesAt(i, j);
                combinedMatches = combinedMatches.Union(matches).ToList();
            }
        }
        return combinedMatches;
    }

    void HighlightTileOff(int x, int y)
    {
        if (m_allTiles[x, y].tileType != TileType.Breakable)
        {
            SpriteRenderer spriteRenderer = m_allTiles[x, y].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        }
    }

    void HighlightTileOn(int x, int y, Color col)
    {
        if (m_allTiles[x, y].tileType != TileType.Breakable)
        {
            SpriteRenderer spriteRenderer = m_allTiles[x, y].GetComponent<SpriteRenderer>();
            spriteRenderer.color = col;
        }
    }

    void HighlightMatchesAt(int x, int y)
    {
        HighlightTileOff(x, y);
        var combinedMatches = FindMatchesAt(x, y);
        if (combinedMatches.Count > 0)
        {
            foreach (GamePieces piece in combinedMatches)
            {
                HighlightTileOn(piece.xIndex, piece.yIndex, piece.GetComponent<SpriteRenderer>().color);
            }
        }
    }

    void HighlightMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                HighlightMatchesAt(i, j);

            }
        }
    }

    void HighlightPieces(List<GamePieces> GamePiecess)
    {
        foreach (GamePieces piece in GamePiecess)
        {
            if (piece != null)
            {
                HighlightTileOn(piece.xIndex, piece.yIndex, piece.GetComponent<SpriteRenderer>().color);
            }
        }
    }
 
    void ClearPieceAt(int x, int y)
    {
        GamePieces pieceToClear = m_allGamePiecess[x, y];

        if (pieceToClear != null)
        {
            m_allGamePiecess[x, y] = null;
            Destroy(pieceToClear.gameObject);
        }

        //HighlightTileOff(x,y);
    }

    /// <summary>
    /// 
    /// </summary>
   public void ClearBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                ClearPieceAt(i, j);
            }
        }
    }
/// <summary>
/// When match3 then cal score and collect by goal
/// </summary>
/// <param name="GamePiecess"></param>
/// <param name="bombedPieces"></param>
    void ClearPieceAt(List<GamePieces> GamePiecess, List<GamePieces> bombedPieces)
{
    int p = 0;
        foreach (GamePieces piece in GamePiecess)
        {
            if (piece != null)
            {
                ClearPieceAt(piece.xIndex, piece.yIndex);

                
                
                int bonus = 0;

                if (GamePiecess.Count >= 4)
                {
                    bonus = 20;
                }
                
                //Collect
              

//                piece.ScorePoints(m_scoreMultiplier, bonus);

                if (m_particleManager != null)
                {
                    if (bombedPieces.Contains(piece))
                    {
                        m_particleManager.BombFXAt(piece.xIndex, piece.yIndex);
                    }
                    else
                    {
                        m_particleManager.ClearPieceFXAt(piece.xIndex, piece.yIndex);
                    }
                }
                //if match >=3 then can add 
                p++;
                if (p >= 3)
                {
                    //CollectPool
                    switch (piece.matchValue)
                    {
                        // case MatchValue.STR:
                        //     CollectionsPool.instance.CollectRecord(piece.matchValue,p);
                        //     break;
                        // case MatchValue.DEX:
                        //     CollectionsPool.instance.CollectRecord(piece.matchValue,p);
                        //     break;
                        // case MatchValue.INT:
                        //     CollectionsPool.instance.CollectRecord(piece.matchValue,p);
                        //     break;
                        // case MatchValue.SWORD:
                        //     CollectionsPool.instance.CollectRecord(piece.matchValue,p);
                        //     break;
                        // case MatchValue.ARMOR:
                        //     CollectionsPool.instance.CollectRecord(piece.matchValue,p);
                        //     break;
                        // case MatchValue.MUT:
                        //     CollectionsPool.instance.CollectRecord(piece.matchValue,p);
                        //     break;
                    }
                }
                
                //
            }
        }
    }

    void BreakTileAt(int x, int y)
    {
        Debug.Log("BreakTileis:\t["+x+","+y+"]");
        Tile tileToBreak = m_allTiles[x, y];

        if (tileToBreak != null && tileToBreak.tileType == TileType.Breakable)
        {
            if (m_particleManager != null)
            {
                m_particleManager.BreakTileFXAt(tileToBreak.breakableValue, x, y, 0);
            }
            tileToBreak.BreakTile();
        }
    }

    void BreakTileAt(List<GamePieces> GamePiecess)
    {
        foreach (GamePieces piece in GamePiecess)
        {
            if (piece != null)
            {
                BreakTileAt(piece.xIndex, piece.yIndex);
            }
        }
    }

    public List<GamePieces> CollaspeColumn(List<int> gp)
    {
        List<GamePieces> mp = new List<GamePieces>();
        foreach (int column in gp)
        {
            mp = mp.Union(CollapseColumn(column)).ToList();
        }

        return mp;
    }
    
    
    List<GamePieces> CollapseColumn(int column, float collapseTime = 0.1f)
    {
        List<GamePieces> movingPieces = new List<GamePieces>();

        for (int i = 0; i < height - 1; i++)
        {
            if (m_allGamePiecess[column, i] == null && m_allTiles[column, i].tileType != TileType.Obstcle)
            {
                for (int j = i + 1; j < height; j++)
                {
                    if (m_allGamePiecess[column, j] != null)
                    {
                        m_allGamePiecess[column, j].Move(column, i, collapseTime * (j - i));

                        m_allGamePiecess[column, i] = m_allGamePiecess[column, j];
                        m_allGamePiecess[column, i].SetCoord(column, i);

                        if (!movingPieces.Contains(m_allGamePiecess[column, i]))
                        {
                            movingPieces.Add(m_allGamePiecess[column, i]);
                        }

                        m_allGamePiecess[column, j] = null;

                        break;
                    }
                }
            }
        }
        return movingPieces;
    }

    List<GamePieces> CollapseColumn(List<GamePieces> GamePiecess)
    {
        List<GamePieces> movingPieces = new List<GamePieces>();

        List<int> columnsToCollapse = GetColumns(GamePiecess);

        foreach (int column in columnsToCollapse)
        {
            movingPieces = movingPieces.Union(CollapseColumn(column)).ToList();
        }

        return movingPieces;
    }


    List<GamePieces> CollapseColumn(List<int> columnToCollapse)
    {
        List<GamePieces> movingPieces = new List<GamePieces>();

        foreach (int column in columnToCollapse)
        {
            movingPieces = movingPieces.Union(CollapseColumn(column)).ToList();
        }

        return movingPieces;

    }
    


    List<int> GetColumns(List<GamePieces> GamePiecess)
    {
        List<int> columns = new List<int>();

        foreach (GamePieces piece in GamePiecess)
        {
            if (!columns.Contains(piece.xIndex))
            {
                columns.Add(piece.xIndex);
            }
        }

        return columns;
    }

   public void ClearAndRefillBoard(List<GamePieces> GamePiecess)
    {
        StartCoroutine(ClearAndRefillBoardRoutine(GamePiecess));
    }


    IEnumerator ClearAndRefillBoardRoutine(List<GamePieces> GamePiecess)
    {
        m_playerInputEnabled = false;

        List<GamePieces> matches = GamePiecess;

        m_scoreMultiplier = 0;

        do
        {
            m_scoreMultiplier++;

            yield return StartCoroutine(ClearAndCollapseRoutine(matches));

            // add pause here 
            yield return null;

            yield return StartCoroutine(RefillRoutine());

            matches = FindAllMatches();

            yield return new WaitForSeconds(0.2f);

        }
        while (matches.Count != 0);

        m_playerInputEnabled = true;

    }

    IEnumerator ClearAndCollapseRoutine(List<GamePieces> GamePiecess)
    {
        List<GamePieces> movingPieces = new List<GamePieces>();
        List<GamePieces> matches = new List<GamePieces>();

        //HighlightPieces(GamePiecess);
        yield return new WaitForSeconds(0.2f);

        bool isFinished = false;

        while (!isFinished)
        {
            List<GamePieces> bombedPieces = GetBombedPieces(GamePiecess);
            GamePiecess = GamePiecess.Union(bombedPieces).ToList();

            bombedPieces = GetBombedPieces(GamePiecess);
            GamePiecess = GamePiecess.Union(bombedPieces).ToList();

            List<GamePieces> collectedPieces = FindCollectiablesAt(0, true);

            List<GamePieces> allCollectiables = FindAllCollectiables();
            List<GamePieces> blockers = GamePiecess.Intersect(allCollectiables).ToList();
            collectedPieces = collectedPieces.Union(blockers).ToList();
            CollectiableCount -= collectedPieces.Count;

            GamePiecess = GamePiecess.Union(collectedPieces).ToList();

            List<int> columntocollapse = GetColumns(GamePiecess);

            ClearPieceAt(GamePiecess, bombedPieces);
            BreakTileAt(GamePiecess);

            if (m_clickedTileBomb != null)
            {
                ActivateBomb(m_clickedTileBomb);
                m_clickedTileBomb = null;
            }

            if (m_targetTileBomb != null)
            {
                ActivateBomb(m_targetTileBomb);
                m_targetTileBomb = null;

            }

            yield return new WaitForSeconds(0.25f);

            movingPieces = CollapseColumn(columntocollapse);
            
            while (!IsCollapsed(movingPieces))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);

            matches = FindMatchesAt(movingPieces);
            collectedPieces = FindCollectiablesAt(0, true);
            matches = matches.Union(collectedPieces).ToList();


            if (matches.Count == 0)
            {
                isFinished = true;
                break;
            }
            else
            {
                m_scoreMultiplier++;
                // if (SoundManager.Instance !=null)
                // {
                // 	SoundManager.Instance.PlayBonusSound();
                // }
                yield return StartCoroutine(ClearAndCollapseRoutine(matches));
            }
        }
        yield return null;
    }

   public IEnumerator RefillRoutine()
    {
        FillBoard(fillYOffset, fillMoveTime);

        yield return null;

    }

    public void ResetFill(){
        StartCoroutine(RefillRoutine());
    }

    bool IsCollapsed(List<GamePieces> GamePiecess)
    {
        foreach (GamePieces piece in GamePiecess)
        {
            if (piece != null)
            {
                if (piece.transform.position.y - (float)piece.yIndex > 0.001f)
                {
                    return false;
                }
            }
        }
        return true;
    }

    List<GamePieces> GetRowPieces(int row)
    {
        List<GamePieces> GamePiecess = new List<GamePieces>();

        for (int i = 0; i < width; i++)
        {
            if (m_allGamePiecess[i, row] != null)
            {
                GamePiecess.Add(m_allGamePiecess[i, row]);
            }
        }
        return GamePiecess;
    }

    List<GamePieces> GetColumnPieces(int column)
    {
        List<GamePieces> GamePiecess = new List<GamePieces>();

        for (int i = 0; i < height; i++)
        {
            if (m_allGamePiecess[column, i] != null)
            {
                GamePiecess.Add(m_allGamePiecess[column, i]);
            }
        }
        return GamePiecess;
    }

    List<GamePieces> GetAdjacentPieces(int x, int y, int offset = 1)
    {
        List<GamePieces> GamePiecess = new List<GamePieces>();

        for (int i = x - offset; i <= x + offset; i++)
        {
            for (int j = y - offset; j <= y + offset; j++)
            {
                if (IsWithinBounds(i, j))
                {
                    GamePiecess.Add(m_allGamePiecess[i, j]);
                }

            }
        }

        return GamePiecess;
    }

    List<GamePieces> GetBombedPieces(List<GamePieces> GamePiecess)
    {
        List<GamePieces> allPiecesToClear = new List<GamePieces>();

        foreach (GamePieces piece in GamePiecess)
        {
            if (piece != null)
            {
                List<GamePieces> piecesToClear = new List<GamePieces>();

                Bomb bomb = piece.GetComponent<Bomb>();

                if (bomb != null)
                {
                    switch (bomb.bombType)
                    {
                        // case BombType.Column:
                        //     piecesToClear = GetColumnPieces(bomb.xIndex);
                        //     break;
                        // case BombType.Row:
                        //     piecesToClear = GetRowPieces(bomb.yIndex);
                        //     break;
                        // case BombType.Adjacent:
                        //     piecesToClear = GetAdjacentPieces(bomb.xIndex, bomb.yIndex, 1);
                        //     break;
                        // case BombType.Color:
							
                        //     break;
                    }

                    allPiecesToClear = allPiecesToClear.Union(piecesToClear).ToList();
                    allPiecesToClear = RemoveCollectiables(allPiecesToClear);
                }
            }
        }

        return allPiecesToClear;
    }

    bool IsCornerMatch(List<GamePieces> GamePiecess)
    {
        bool vertical = false;
        bool horizontal = false;
        int xStart = -1;
        int yStart = -1;

        foreach (GamePieces piece in GamePiecess)
        {
            if (piece != null)
            {
                if (xStart == -1 || yStart == -1)
                {
                    xStart = piece.xIndex;
                    yStart = piece.yIndex;
                    continue;
                }

                if (piece.xIndex != xStart && piece.yIndex == yStart)
                {
                    horizontal = true;
                }

                if (piece.xIndex == xStart && piece.yIndex != yStart)
                {
                    vertical = true;
                }
            }
        }

        return (horizontal && vertical);

    }

    GameObject DropBomb(int x, int y, Vector2 swapDirection, List<GamePieces> GamePiecess)
    {
        GameObject bomb = null;

        if (GamePiecess.Count >= 4)
        {
            if (IsCornerMatch(GamePiecess))
            {
                if (adjacentBombPrefab != null)
                {
                    bomb = MakeBomb(adjacentBombPrefab, x, y);
                }
            }
            else
            {
                if (GamePiecess.Count >= 5)
                {
                    if (colorBombPrefab != null)
                    {
                        bomb = MakeBomb(colorBombPrefab, x, y);

                    }
                }
                else
                {
                    if (swapDirection.x != 0)
                    {
                        if (rowBombPrefab != null)
                        {
                            bomb = MakeBomb(rowBombPrefab, x, y);
                        }

                    }
                    else
                    {
                        if (columnBombPrefab != null)
                        {
                            bomb = MakeBomb(columnBombPrefab, x, y);
                        }
                    }
                }
            }
        }
        return bomb;
    }

    void ActivateBomb(GameObject bomb)
    {
        int x = (int)bomb.transform.position.x;
        int y = (int)bomb.transform.position.y;


        if (IsWithinBounds(x, y))
        {
            m_allGamePiecess[x, y] = bomb.GetComponent<GamePieces>();
        }
    }

    List<GamePieces> FindAllMatchValue(MatchValue mValue)
    {
        List<GamePieces> foundPieces = new List<GamePieces>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (m_allGamePiecess[i, j] != null)
                {
                    if (m_allGamePiecess[i, j].matchValue == mValue)
                    {
                        foundPieces.Add(m_allGamePiecess[i, j]);
                    }
                }
            }
        }
        return foundPieces;
    }

    bool IsColorBomb(GamePieces GamePieces)
    {
        Bomb bomb = GamePieces.GetComponent<Bomb>();

        // if (bomb != null)
        // {
        //     return (bomb.bombType == BombType.Color);
        // }
        return false;
    }
        
    List<GamePieces> FindCollectiablesAt(int row, bool clearedAtBottomOnly = false)
    {
        List<GamePieces> foundCollectiables = new List<GamePieces>();

        for (int i = 0; i < width; i++)
        {
            if (m_allGamePiecess[i,row] !=null)
            {
                Collectiable CollectiableComponent = m_allGamePiecess[i,row].GetComponent<Collectiable>();

                if (CollectiableComponent !=null)
                {
                    if (!clearedAtBottomOnly || (clearedAtBottomOnly && CollectiableComponent.clearBybottom))
                    {
                        foundCollectiables.Add(m_allGamePiecess[i, row]);                  
                    }
                }
            }
        }
        return foundCollectiables;
    }

    List<GamePieces> FindAllCollectiables()
    {
        List<GamePieces> foundCollectiables = new List<GamePieces>();

        for (int i = 0; i < height; i++)
        {
            List<GamePieces> CollectiableRow = FindCollectiablesAt(i);
            foundCollectiables = foundCollectiables.Union(CollectiableRow).ToList();
        }

        return foundCollectiables;
    }

    bool CanAddCollectiable()
    {
        return (UnityEngine.Random.Range(0f, 1f) <= chanceForCollectiable && CollectiablePrefabs.Length > 0 && CollectiableCount < maxCollectiables);
    }

    List<GamePieces> RemoveCollectiables(List<GamePieces> bombedPieces)
    {
        List<GamePieces> CollectiablePieces = FindAllCollectiables();
        List<GamePieces> piecesToRemove = new List<GamePieces>();

        foreach (GamePieces piece in CollectiablePieces)
        {
            Collectiable CollectiableComponent = piece.GetComponent<Collectiable>();

            if (CollectiableComponent != null)
            {
                if (!CollectiableComponent)
                {
                    piecesToRemove.Add(piece);
                }
            }
        }

        return bombedPieces.Except(piecesToRemove).ToList();
    }




}


    
