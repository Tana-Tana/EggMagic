using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [Header("Tile", order = 0)]
    [SerializeField] private Tile tileEvenPrefab;
    [SerializeField] private Tile tileOddPrefab;
    [SerializeField] private Transform tilesTransform;

    // Tile
    private Tile[,] tiles = new Tile[10, 10];
    private int[] dRow = { -1, 0, 1, 0 };
    private int[] dCol = { 0, 1, 0, -1 };
    private float xTile = -1.66f;
    private float yTile = 0.39f;
    private int countSortingOrder = 1;
    // Color
    private Color selectedColor = new Color(0, 230/ 255f, 38/ 255f, 1f);

    private void Awake()
    {
        GenerateTile(tileEvenPrefab,tileOddPrefab);
    }

    private void GenerateTile(Tile tileEvenPrefab, Tile tileOddPrefab)
    {
        for( int i =  1; i <= 5; ++i)
        {
            for (int j =1;j<= 5; ++j)
            {
                Tile obj = ((i+j) % 2 == 0) ? Instantiate(tileEvenPrefab, tilesTransform) : Instantiate(tileOddPrefab, tilesTransform);
                obj.transform.position = new Vector3(xTile + 0.82f * (j-1), yTile, 0);
                obj.SpriteRenderer.sortingLayerName = GameConfig.OBJECT_LAYER;
                obj.SpriteRenderer.sortingOrder = countSortingOrder;

                tiles[i,j]  = obj;
                tiles[i, j].Row = i;
                tiles[i,j].Col = j;
            }
            countSortingOrder += 2;
            yTile -= 0.8f;
        }
    }

    private void OnEnable()
    {
        Messenger.AddListener<Egg[,]>(EventKey.SET_TYPE_TILE, SetChildrenOfTile);
        Messenger.AddListener<int, int>(EventKey.BFS, FindAWay);
        Messenger.AddListener(EventKey.RESET_ALL_TILES, LowerTheLand);
        Messenger.AddListener<int,int>(EventKey.SET_EGG_FREE, SetNotChildrenOfTile);
        Messenger.AddListener<int, int, Egg[,]>(EventKey.SET_PARENT_EGG_SELECTED, SetParentEggSelected);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<Egg[,]>(EventKey.SET_TYPE_TILE, SetChildrenOfTile);
        Messenger.RemoveListener<int, int>(EventKey.BFS, FindAWay);
        Messenger.RemoveListener(EventKey.RESET_ALL_TILES, LowerTheLand);
        Messenger.RemoveListener<int, int>(EventKey.SET_EGG_FREE, SetNotChildrenOfTile);
        Messenger.RemoveListener<int, int, Egg[,]>(EventKey.SET_PARENT_EGG_SELECTED, SetParentEggSelected);
    }

    private void SetParentEggSelected(int row, int col, Egg[,] eggs)
    {
        tiles[row, col].Egg.transform.SetParent(tiles[row, col].transform);
    }

    
    private void FindAWay(int row, int col)
    {
        // BFS
        bool[,] visited = new bool[10, 10];
        Queue<Tile> queue = new Queue<Tile>();

        queue.Enqueue(tiles[row, col]);
        visited[row, col] = true;

        while (queue.Count > 0)
        {
            Tile tile = queue.Dequeue();
            tiles[tile.Row, tile.Col].clicked = true;
            string level = null;

            if (tiles[tile.Row, tile.Col].Egg != null) // lấy trứng của ô hiện tại
            {
                level = tiles[tile.Row, tile.Col].Egg.Infor.Level;
            }
            else
            {
                Debug.Log("Tile không có trứng");
            }

            for (int i = 0; i < 4; ++i) // duyệt 4 hướng
            {
                if (!visited[tile.Row + dRow[i], tile.Col + dCol[i]]) // ô chưa được duyệt
                {
                    if (tiles[tile.Row + dRow[i], tile.Col + dCol[i]] != null && tiles[tile.Row + dRow[i], tile.Col + dCol[i]].Egg != null) // kiểm tra null
                    {
                        if (tiles[tile.Row + dRow[i], tile.Col + dCol[i]].Egg.Infor.Level.Equals(level)) // chung level
                        {
                            GamePlayController.Instance.checkBFS = true;
                            tiles[tile.Row + dRow[i], tile.Col + dCol[i]].clicked = true;
                            queue.Enqueue(tiles[tile.Row + dRow[i], tile.Col + dCol[i]]);
                            visited[tile.Row + dRow[i], tile.Col + dCol[i]] = true;
                        }

                    }
                }

            }
        }

        RaiseTheLand();
        if (!GamePlayController.Instance.checkBFS) tiles[row, col].clicked = false;
    }

    private void RaiseTheLand()
    {
        if (GamePlayController.Instance.checkBFS)
        {
            GamePlayController.Instance.checkMerging = true;
            for (int i = 1; i <= 5; ++i) //nâng các ô được chọn lên 1 bậc và đổi màu
            {
                for (int j = 1; j <= 5; ++j)
                {
                    if (tiles[i, j].clicked)
                    {
                        tiles[i, j].transform.Translate(Vector3.up * 0.05f);
                        tiles[i, j].SpriteRenderer.color = selectedColor;
                    }
                }
            }
        }
    }

    private void LowerTheLand()
    {
        GamePlayController.Instance.checkBFS = false;
        GamePlayController.Instance.checkMerging = false;
        for (int i = 1; i <= 5; ++i) //nâng các ô được chọn lên 1 bậc và đổi màu
        {
            for (int j = 1; j <= 5; ++j)
            {
                if (tiles[i, j].clicked)
                {
                    tiles[i, j].transform.Translate(Vector3.down * 0.05f);
                    tiles[i, j].SpriteRenderer.color = Color.white;
                    tiles[i, j].clicked = false;
                }
            }
        }
    }

    private void SetChildrenOfTile(Egg[,] eggs)
    {
        for (int i =1;i<= 5; ++i)
        {
            for(int j = 1; j <= 5; ++j)
            {
                tiles[i,j].ShadowEgg.gameObject.SetActive(true);
                tiles[i, j].Egg = eggs[i,j];
                tiles[i, j].Egg.transform.SetParent(tiles[i, j].transform);
            }
        }
    }
    private void SetNotChildrenOfTile(int row, int col)
    {
        for (int i = 1; i <= 5; ++i)
        {
            for (int j = 1; j <= 5; ++j)
            {
                if (tiles[i, j].clicked)
                {
                    tiles[i, j].Egg.isMove = true;
                    tiles[i, j].Egg.transform.SetParent(null);
                    if (i == row && j == col) continue;
                    else
                    {
                        tiles[i, j].ShadowEgg.gameObject.SetActive(false);
                        tiles[i, j].Egg = null;
                    }
                }
            }
        }
    }

}
