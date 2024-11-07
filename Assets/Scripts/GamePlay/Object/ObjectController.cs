using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using DG.Tweening;
using UnityEngine;

public class ObjectController : Singleton<ObjectController>
{
    [Header("-----------Prefabs Ground-----------")]
    [SerializeField] private Ground oddGroundPrefab;
    [SerializeField] private Ground evenGroundPrefab;

    [Header("----------------Matrix---------------")]
    [SerializeField] private int column;
    [SerializeField] private int row;

    private Ground[,] _ground;
    private Egg[,] _egg;

    // BFS
    private int[] _x; // tọa độ x ô liền kề
    private int[] _y; // tọa độ y ô liền kề
    private bool[,] isVisited; // kiểm tra ô đã được duyệt hay chưa
    private Queue<Pair<int, int>> myQueue; // queue để duyệt BFS
    private Stack<Pair<Pair<int, int>, Pair<int, int>>> myStack; // lưu đường đi
    private Color _colorHideAsset = new Color(1, 1, 1, 0);

    // CheckMerge
    public bool CheckMerge = false;
    public bool isPrepareMerge = false;

    private void Start()
    {
        _x = new int[] { 0, 0, -1, 1 };
        _y = new int[] { -1, 1, 0, 0 };
        _ground = new Ground[row + 5, column + 5];
        _egg = new Egg[row + 5, column + 5];
        GenerateGroundAndEggOfGround();
    }

    private void GenerateGroundAndEggOfGround()
    {
        for (int i = 1; i <= row; ++i)
        {
            for (int j = 1; j <= column; ++j)
            {
                int sum = i + j;
                if (sum % 2 == 0)
                {
                    _ground[i, j] = Instantiate(oddGroundPrefab, transform);
                }
                else
                {
                    _ground[i, j] = Instantiate(evenGroundPrefab, transform);
                }
                _ground[i, j].PositionPair = new Pair<int, int>(i, j);
                _ground[i, j].ThisEgg.LoadInitEgg(); // Load level cho egg hiện tại
                _egg[i, j] = _ground[i, j].ThisEgg; // Thiết lập thông tin của trứng tại đất hiện tại
                //Debug.Log(_egg[i,j].IdName);
                _ground[i, j].LevelGround = _egg[i, j].IdName; // thiết lập level của đất theo level của trứng
            }
        }
    }

    public void SetInitGround()
    {
        isPrepareMerge = false;
        for (int i = 1; i <= row; ++i)
        {
            for (int j = 1; j <= column; ++j)
            {
                if (_ground[i, j].isSelected)
                {
                    _ground[i, j].isSelected = false;
                    _ground[i, j].transform.position -= Vector3.up * 0.1f;
                }
            }
        }
    }


    public void DoSomethingWithGroundIsSelected(string levelGround, Pair<int, int> thisSelectedPosition)
    {
        CheckMerge = false;
        isVisited = null;
        isVisited = new bool[row + 5, column + 5]; // đặt lại mảng cần duyệt
        PickGroundWithTheSameLevelByBFS(levelGround, thisSelectedPosition);
    }

    private void PickGroundWithTheSameLevelByBFS(string levelGround, Pair<int, int> thisSelectedPosition)
    {
        myQueue = null;
        myQueue = new Queue<Pair<int, int>>(); // tạo mới 1 queue chứa hàng và cột
        myQueue.Enqueue(thisSelectedPosition); // thêm ô hiện tại vào queue
        //Debug.Log("Ô duyệt bằng BFS có tọa độ: " + thisSelectedPosition.First + " " + thisSelectedPosition.Second);
        //Debug.Log(levelGround);

        while (myQueue.Count > 0) // queue chưa rỗng
        {
            Pair<int, int> myPosition = myQueue.Dequeue();  // lấy phản tử đầu tiên đồng thời xóa
            isVisited[myPosition.First, myPosition.Second] = true; // đánh dấu ô đã được duyệt
            //Debug.Log("Ô duyệt bằng BFS có tọa độ: " + myPosition.First + " " + myPosition.Second);
            for (int i = 0; i < 4; ++i)
            {
                // lưu vị trí hiện tại duyệt vào trong pair
                thisSelectedPosition = new Pair<int, int>(myPosition.First + _y[i], myPosition.Second + _x[i]);
                int x = thisSelectedPosition.First;
                int y = thisSelectedPosition.Second;

                // Kiểm tra các ô xung quanh vị trí hiện tại bằng matrix Ground
                if (_ground[x, y] != null && _ground[x, y].LevelGround.Equals(levelGround) && !isVisited[x, y]) // ô chưa được chuyệt và chung level
                {
                    isVisited[x, y] = true; // đánh dấu ô đã được duyệt
                    myQueue.Enqueue(thisSelectedPosition); // thêm ô được chọn vào queue
                    CheckMerge = true; // có trường hợp có thể merge
                    isPrepareMerge = true; // xác định có ô merge
                }
            }

            if (CheckMerge)
            {
                _ground[myPosition.First, myPosition.Second].isSelected = true; // đánh dấu ô này được chọn
                _ground[myPosition.First, myPosition.Second].transform.position += Vector3.up * 0.1f; // hiệu ứng ô được chọn
            }
        }

        //Debug.Log("--------------------------------------------------------------");
    }

    public void MergeEgg(Pair<int, int> positionPair)
    {
        isVisited = null;
        isVisited = new bool[row + 5, column + 5]; // đặt lại mảng cần duyệt
        myStack = new Stack<Pair<Pair<int, int>, Pair<int, int>>>(); // Lưu đường đi cho trứng gần nhất
        CreatingTheWay(positionPair); // tạo đường đi cho trứng
        ActionOfEgg(); // trứng di chuyển

        
    }

    private void ActionOfEgg()
    {
        //Debug.Log(myStack.Count);
        //while (myStack.Count > 0)
        //{
        //    Pair<Pair<int, int>, Pair<int, int>> pair = myStack.Pop();
        //    Pair<int, int> positionStart = pair.First;
        //    Pair<int, int> positionEnd = pair.Second;

        //    Debug.Log("Điểm bắt đầu: " + positionStart.First + " " + positionStart.Second);
        //    Debug.Log("Điểm kết thúc: " + positionEnd.First + " " + positionEnd.Second);
        //}
        if (myStack.Count > 0)
        {
            Pair<Pair<int, int>, Pair<int, int>> pair = myStack.Pop();
            Pair<int, int> positionStart = pair.First;
            Pair<int, int> positionEnd = pair.Second;

            // Di chuyển start -> end -> ẩn image Egg -> reset position egg.
            Vector3 _startTransformPosition = _egg[positionStart.First, positionStart.Second].transform.position;
            //Debug.Log("Transform Start = " + _startTransformPosition);
            _egg[positionStart.First, positionStart.Second].transform
                .DOMove(_egg[positionEnd.First, positionEnd.Second].transform.position, 0.05f)
                .OnComplete(() =>
                {
                    //Debug.Log("Transform Start = " + _startTransformPosition);
                    _egg[positionStart.First, positionStart.Second].transform.position = _startTransformPosition;

                    _egg[positionStart.First, positionStart.Second].Image.color = _colorHideAsset;
                    _ground[positionStart.First, positionStart.Second].CheckHaveEgg = false;
                    ActionOfEgg();
                });
        }
        else
        {
            // Init
            SetInitGround();
            CheckMerge = false;
        }
    }

    private void CreatingTheWay(Pair<int, int> positionPair)
    {
        myQueue = null;
        myQueue = new Queue<Pair<int, int>>(); // tạo mới 1 queue chứa hàng và cột
        myQueue.Enqueue(positionPair); // thêm ô hiện tại vào queue

        while (myQueue.Count > 0) // queue chưa rỗng
        {
            Pair<int, int> myPosition = myQueue.Dequeue();  // lấy phản tử đầu tiên đồng thời xóa
            isVisited[myPosition.First, myPosition.Second] = true; // đánh dấu ô đã được duyệt
            //Debug.Log("Ô duyệt bằng BFS có tọa độ: " + myPosition.First + " " + myPosition.Second);
            for (int i = 0; i < 4; ++i)
            {
                // lưu vị trí hiện tại duyệt vào trong pair
                positionPair = new Pair<int, int>(myPosition.First + _y[i], myPosition.Second + _x[i]);
                int x = positionPair.First;
                int y = positionPair.Second;

                // Kiểm tra các ô xung quanh vị trí hiện tại bằng matrix Ground
                if (_ground[x, y] != null && _ground[x, y].isSelected && !isVisited[x, y]) // ô chưa được chuyệt và đang được chọn
                {
                    isVisited[x, y] = true; // đánh dấu ô đã được duyệt
                    myQueue.Enqueue(positionPair); // thêm ô được chọn vào queue
                    myStack.Push(new Pair<Pair<int, int>, Pair<int, int>>(positionPair, myPosition)); // điểm đi -> điểm đến
                }
            }
        }
    }
}
