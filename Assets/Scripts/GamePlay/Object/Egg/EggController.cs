using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.GamePlay.Object.Egg;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;
using Random = UnityEngine.Random;

public class EggController : MonoBehaviour
{
    [Header("Egg", order = 0)]
    [SerializeField] private Egg eggPrefab;

    // private
    private EggPool eggPool;
    private Egg[,] eggs = new Egg[10, 10];
    private KeyValuePair<float,float>[,] positionEgg = new KeyValuePair<float,float>[10, 10];
    private bool[,] checkNullEgg = new bool[10,10];
    private int[] dRow = { -1, 0, 1, 0 };
    private int[] dCol = { 0, 1, 0, -1 };
    private float xEgg = -1.65f;
    private float yEgg = 0.42f;

    private void Start()
    {
        eggPool = new EggPool();
        GenerateEgg(eggPrefab);
        RandomAttributeStart(eggs);
        Messenger.Broadcast(EventKey.SET_TYPE_TILE, eggs);
    }

    private void RandomAttributeStart(Egg[,] eggs)
    {
        for(int i = 1; i <= 5; ++i)
        {
            for (int j = 1; j <= 5; ++j)
            {
                EggType eggType = (EggType)Random.Range(1,4); // random level 1 -> 3
                eggs[i, j].Type = eggType;
                eggs[i,j].Infor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType); // load infor cua level vao trung
                eggs[i, j].SpriteRenderer.sprite = eggs[i, j].Infor.ImageGamePlay; // load sprite level
                eggs[i, j].Row = i;
                eggs[i, j].Col = j;
            }
        }
    }

    private void GenerateEgg(Egg eggPrefab)
    {
        for (int i = 1; i <= 5; ++i)
        {
            for (int j = 1; j <= 5; ++j)
            {
                Egg egg = Instantiate(eggPrefab); // sinh trứng
                egg.transform.position = new Vector3(xEgg + 0.82f * (j - 1), yEgg, 0); // Đặt lại tọa độ cho trứng

                eggs[i, j] = egg; // thêm trứng vào ma trận để dễ xử lí
                positionEgg[i, j] = new KeyValuePair<float, float>(eggs[i, j].transform.position.x, eggs[i, j].transform.position.y); // lưu vị trí của quả trứng
            }
            yEgg -= 0.8f;
        }
        yEgg += (0.8f * 6);
    }

    private void OnEnable()
    {
        Messenger.AddListener<int,int>(EventKey.EGG_MOVING, Move);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<int,int>(EventKey.EGG_MOVING, Move);
    }

    private async void Move(int rowSelection, int colSelection)
    {
        // merge
        Dictionary<int, Dictionary<Egg, Egg>> dictRes = new Dictionary<int, Dictionary<Egg, Egg>>(); // lưu kết quả và thứ tự di chuyển
        SearchOrderOfMovement(rowSelection, colSelection, dictRes);
        await ActionOfEgg(dictRes, rowSelection, colSelection);
        Debug.Log(eggPool.amoutPool());
        // hạ
        Messenger.Broadcast(EventKey.RESET_ALL_TILES);
        // sinh trứng
        await DropEgg();
        // reset lại quan hệ cha con trứng đất
        Messenger.Broadcast(EventKey.SET_TYPE_TILE, eggs);
    }

    private void SetEggOrigin()
    {
        for (int i = 1; i <= 5; ++i)
        {
            for (int j = 1; j <= 5; ++j)
            {
                eggs[i, j].isMove = false;
                eggs[i, j].Row = i;
                eggs[i, j].Col = j;
                eggs[i,j].gameObject.SetActive(true);
            }
        }
    }

    private async UniTask DropEgg()
    {
        SetNotParent();
        int[] _amoutEggOfColumn = new int[10];
        CheckTileHaveEggCurrent(_amoutEggOfColumn);
        bool[,] visited = new bool[10,10];
        for (int j = 1; j <= 5; ++j) // duyệt từng cột
        {
            for (int i=5 ; i>= 5 - _amoutEggOfColumn[j] + 1;--i ) // rơi trứng
            {
                if (checkNullEgg[i,j])
                {
                    for (int k = i - 1; k >= 1; --k)
                    {
                        if (checkNullEgg[i,j] && !checkNullEgg[k,j] && !visited[k,j])
                        {
                            visited[k,j] = true;
                            eggs[k, j].transform.DOMove(new Vector3(positionEgg[i, j].Key, positionEgg[i, j].Value, 0), 0.1f).SetEase(Ease.OutSine);
                            eggs[i,j] = eggs[k, j];
                            checkNullEgg[k, j] = true;
                            checkNullEgg[i,j] = false;
                        }
                    }
                }
            }
            for (int i = 5 - _amoutEggOfColumn[j]; i >= 1; --i) // lấy trứng từ pool
            {
                Egg egg = eggPool.GetObject();
                eggs[i, j] = egg;
                egg.transform.position = new Vector3(egg.transform.position.x + 0.8f * (j - 1), egg.transform.position.y, 0);
                egg.transform.DOMove(new Vector3(positionEgg[i, j].Key, positionEgg[i, j].Value, 0), 0.1f).SetEase(Ease.OutSine);
                checkNullEgg[i, j] = false;
                eggPool.RemoveObject();
            }
        }
        SetEggOrigin();
        await UniTask.Delay(100);
    }

    private void SetNotParent()
    {
        for (int i = 1; i <= 5; ++i)
        {
            for (int j =1;j<=5;++j)
            {
                if (eggs[i,j] != null)
                {
                    eggs[i,j].transform.SetParent(null);
                }
            }
        }
    }

    private void CheckTileHaveEggCurrent(int[] _amoutEggOfColumn)
    {
        for (int i = 1; i <= 5; ++i)
        {
            _amoutEggOfColumn[i] = 0;
        }

        for (int j = 1; j <= 5; ++j)
        {
            for (int i = 1; i <= 5; ++i)
            {
                if (!checkNullEgg[i, j])
                {
                    ++_amoutEggOfColumn[j];
                }
            }
        }
    }

    private async UniTask ActionOfEgg(Dictionary<int, Dictionary<Egg, Egg>> dictRes, int row, int col)
    {
        int cnt = dictRes.Count;
        while (cnt-- > 0)
        {
            foreach (var item in dictRes)
            {
                if (item.Key == cnt)
                {
                    Dictionary<Egg, Egg> dict = item.Value;
                    if (dict != null && dict.Count > 0)
                    {
                        foreach (var itemEgg in dict)
                        {
                            Egg startPositionEgg = itemEgg.Key;
                            Egg endPositionEgg = itemEgg.Value;
                            checkNullEgg[startPositionEgg.Row, startPositionEgg.Col] = true;
                            eggPool.AddObject(eggs[startPositionEgg.Row, startPositionEgg.Col]);

                            eggs[startPositionEgg.Row, startPositionEgg.Col].transform
                                .DOMove(new Vector3(positionEgg[endPositionEgg.Row, endPositionEgg.Col].Key, positionEgg[endPositionEgg.Row, endPositionEgg.Col].Value, 0), 0.1f)
                                .SetEase(Ease.OutSine)
                                .OnComplete(() =>
                                {
                                    eggs[startPositionEgg.Row, startPositionEgg.Col].gameObject.SetActive(false);
                                    eggs[startPositionEgg.Row, startPositionEgg.Col].transform.position = new Vector3(xEgg, yEgg, 0);
                                });
                        }
                        await UniTask.Delay(60);
                    }
                }
            }
        }
        await UniTask.Delay(40);
        Messenger.Broadcast(EventKey.SET_PARENT_EGG_SELECTED, row, col, eggs);
        eggs[row, col].LevelUp();
    }

    private void SearchOrderOfMovement(int rowSelection, int colSelection, Dictionary<int, Dictionary<Egg, Egg>> dictRes)
    {
        bool[,] visited = new bool[10, 10]; // kiểm tra đã duyệt ô đó chưa
        Queue<Egg> queue = new Queue<Egg>(); // Lưu thứ tự duyệt
        eggs[rowSelection, colSelection].OrderOfMovement = 1;
        queue.Enqueue(eggs[rowSelection, colSelection]);
        visited[rowSelection, colSelection] = true;

        Dictionary<Egg, Egg> dict = new Dictionary<Egg, Egg>();
        while (queue.Count > 0)
        {
            Egg egg = queue.Dequeue();
            if (!dictRes.ContainsKey(egg.OrderOfMovement)) // trong dictRes không tồn tại key hiện tại => chứng tỏ đã hết giá trị key trước
            {
                dictRes[egg.OrderOfMovement - 1] = dict;
                dict = new Dictionary<Egg, Egg>();
                dictRes.Add(egg.OrderOfMovement, dict);
            }

            for (int i = 0; i < 4; ++i)
            {
                int row = egg.Row + dRow[i];
                int col = egg.Col + dCol[i];
                if (visited[row, col]) continue;
                if (eggs[row, col] != null)
                {
                    if (eggs[row, col].isMove)
                    {
                        visited[row, col] = true;
                        eggs[row, col].OrderOfMovement = egg.OrderOfMovement + 1;
                        dict.Add(eggs[row, col], eggs[egg.Row, egg.Col]);
                        queue.Enqueue(eggs[row, col]);
                    }
                }
            }
        }
    }
}
