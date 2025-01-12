using System;
using Assets.Scripts.Common;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [Header("Tile", order = 0)]
    [SerializeField] private Tile tileEvenPrefab;
    [SerializeField] private Tile tileOddPrefab;
    [SerializeField] private Transform tilesTransform;
    
    // private
    private Tile[,] tiles = new Tile[10, 10];
    private int[] di = { -1, 0, 1, 0 };
    private int[] dj = { 0, 1, 0, -1 };
    private float xTile = -1.66f;
    private float yTile = 0.39f;
    private int countSortingOrder = 1;
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
                tiles[i, j].row = i;
                tiles[i,j].col = j;
            }
            countSortingOrder += 2;
            yTile -= 0.8f;
        }
    }

    private void OnEnable()
    {
        Messenger.AddListener<Egg[,]>(EventKey.SET_TYPE_TILE, setTypeTile);
        Messenger.AddListener<int, int>(EventKey.BFS, FindTheWayToEggGo);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<Egg[,]>(EventKey.SET_TYPE_TILE, setTypeTile);
        Messenger.RemoveListener<int, int>(EventKey.BFS, FindTheWayToEggGo);
    }

    private void FindTheWayToEggGo(int row, int col)
    {
        
    }

    private void setTypeTile(Egg[,] eggs)
    {
        for (int i =1;i<= 5; ++i)
        {
            for(int j = 1; j <= 5; ++j)
            {
                tiles[i, j].Egg = eggs[i,j];
                tiles[i, j].Egg.transform.SetParent(tiles[i, j].transform);
            }
        }
    }


}
