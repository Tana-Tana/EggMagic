using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using DG.Tweening;
using UnityEditor.Build;
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

    // BFS
    private int[] _x; // tọa độ x ô liền kề
    private int[] _y; // tọa độ y ô liền kề
    private bool[,] isVisited; // kiểm tra ô đã được duyệt hay chưa
    private int[,] _numberArrMerge;
    private List<Pair<Pair<int, int>, List<Pair<int, int>>>> _roadList;
    private Queue<Pair<int, int>> myQueue; // queue để duyệt BFS
    private Color _colorHideAsset = new Color(1, 1, 1, 0);
    private int _maxNumberMerge = -1;

    // CheckMerge
    public bool CheckMerge = false;
    public bool isPrepareMerge = false;
    public bool isFinishMerge = true;
    private bool[,] _checkHaveEgg; // lưu ô còn trứng
    private int[] _amoutEggOfColumn;

    //Pool
    private List<Egg> _poolEgg;

    //Score
    private int _scorePending = 0;

    private void Start()
    {
        _x = new int[] { 0, 0, -1, 1 };
        _y = new int[] { -1, 1, 0, 0 };
        _ground = new Ground[row + 5, column + 5];
        _poolEgg = new List<Egg>();
        _checkHaveEgg = new bool[row + 5, column + 5];
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
                //Debug.Log(_egg[i,j].IdName);
            }
        }

    }

    public async Task SetInitGround()
    {
        isPrepareMerge = false;
        CheckMerge = false;
        bool checkAction = false;
        for (int i = 1; i <= row; ++i)
        {
            for (int j = 1; j <= column; ++j)
            {
                if (_ground[i, j].isSelected)
                {
                    checkAction= true;
                    _ground[i, j].isSelected = false;
                    _ground[i, j].transform.DOMove(-(Vector3.up * 0.1f), 0.25f).SetRelative();
                }
            }
        }

        if (checkAction)
        {
            await Task.Delay(250);
        }
    }


    public void DoSomethingWithGroundIsSelected(string idName, Pair<int, int> thisSelectedPosition)
    {
        CheckMerge = false;
        isVisited = new bool[row + 5, column + 5]; // đặt lại mảng cần duyệt
        PickGroundWithTheSameLevelByBFS(idName, thisSelectedPosition);
    }

    private async void PickGroundWithTheSameLevelByBFS(string idName, Pair<int, int> thisSelectedPosition)
    {
        myQueue = new Queue<Pair<int, int>>(); // tạo mới 1 queue chứa hàng và cột
        myQueue.Enqueue(thisSelectedPosition); // thêm ô hiện tại vào queue
        //Debug.Log("Ô duyệt bằng BFS có tọa độ: " + thisSelectedPosition.First + " " + thisSelectedPosition.Second);
        //Debug.Log(idName);
        _scorePending = 0; // reset ScorePending

        while (myQueue.Count > 0) // queue chưa rỗng
        {
            Pair<int, int> myPosition = myQueue.Dequeue();  // lấy phản tử đầu tiên đồng thời xóa
            _scorePending += _ground[myPosition.First, myPosition.Second].ThisEgg.ScoreEgg; // pendingScore

            isVisited[myPosition.First, myPosition.Second] = true; // đánh dấu ô đã được duyệt
            //Debug.Log("Ô duyệt bằng BFS có tọa độ: " + myPosition.First + " " + myPosition.Second);
            for (int i = 0; i < 4; ++i)
            {
                // lưu vị trí hiện tại duyệt vào trong pair
                thisSelectedPosition = new Pair<int, int>(myPosition.First + _y[i], myPosition.Second + _x[i]);
                int x = thisSelectedPosition.First;
                int y = thisSelectedPosition.Second;

                if (_ground[x, y] != null && _ground[x, y].ThisEgg != null)
                {
                    //Debug.Log((_ground[x, y].ThisEgg == null) + " " + x + " " + y);
                    // Kiểm tra các ô xung quanh vị trí hiện tại bằng matrix Ground
                    if (_ground[x, y].ThisEgg.IdName.Equals(idName) && !isVisited[x, y]) // ô chưa được chuyệt và chung level
                    {
                        isVisited[x, y] = true; // đánh dấu ô đã được duyệt
                        myQueue.Enqueue(thisSelectedPosition); // thêm ô được chọn vào queue
                        CheckMerge = true; // có trường hợp có thể merge
                        isPrepareMerge = true; // xác định có ô merge
                    }
                }
            }

            if (CheckMerge)
            {
                _ground[myPosition.First, myPosition.Second].isSelected = true; // đánh dấu ô này được chọn
                _ground[myPosition.First, myPosition.Second].transform.DOMove((Vector3.up * 0.1f), 0.25f).SetRelative(); // hiệu ứng ô được chọn
            }
        }
        if(CheckMerge) await Task.Delay(250);
        //Debug.Log("--------------------------------------------------------------");
    }

    public async void MergeEgg(Pair<int, int> positionPair)
    {
        isFinishMerge = false;
        isVisited = new bool[row + 5, column + 5]; // đặt lại mảng cần duyệt
        _numberArrMerge = new int[row + 5, column + 5]; // mảng lưu cấp độ merge
        _roadList = new List<Pair<Pair<int, int>, List<Pair<int, int>>>>();
        InitNumberArrMerge();
        CreatingTheWay(positionPair); // tạo đường đi cho trứng
        await ActionOfEgg(positionPair); // trứng di chuyển

        //await Task.Delay(110);
        GamePlayController.Instance.Score += _scorePending;
        Messenger.Broadcast(EventKey.RESET_TIME_AND_UPDATE_SCORE);
        isFinishMerge = true;
    }

    private void InitNumberArrMerge()
    {
        for (int i = 1; i <= row; i++)
        {
            for (int j = 1; j <= column; j++)
            {
                _numberArrMerge[i, j] = -1;
            }
        }
    }

    private async Task ActionOfEgg(Pair<int, int> positionPair)
    {
        bool checkAction = false;
        for (int i = _maxNumberMerge - 1; i >= 0; i--)
        {
            foreach (Pair<Pair<int, int>, List<Pair<int, int>>> pair in _roadList)
            {
                Pair<int, int> endPoint = pair.First;
                List<Pair<int, int>> startPointList = pair.Second;
                int numberCurrentEnd = _numberArrMerge[endPoint.First, endPoint.Second]; // số hiện tại của điểm đến
                if (numberCurrentEnd == i)
                {
                    if (startPointList.Count > 0)
                    {
                        foreach (Pair<int, int> startPoint in startPointList)  // Di chuyển điểm đi tới điểm đến
                        {
                            //_egg[startPoint.First, startPoint.Second].Image.DOFade(0, 0.1f);
                            _ground[startPoint.First, startPoint.Second].ThisEgg.transform
                                .DOMove(_ground[endPoint.First, endPoint.Second].ThisEgg.transform.position, 0.1f)
                                .SetEase(Ease.OutQuad)
                                .OnComplete(() =>
                                {
                                    _ground[endPoint.First, endPoint.Second].SetCenterEgg();
                                    _ground[startPoint.First, startPoint.Second].ThisEgg.gameObject.SetActive(false);
                                    _poolEgg.Add(_ground[startPoint.First, startPoint.Second].ThisEgg);
                                    
                                    _ground[startPoint.First, startPoint.Second].ThisEgg.transform.SetParent(null);
                                    _ground[startPoint.First, startPoint.Second].ThisEgg = null;
                                });
                        }
                        checkAction = true;
                    }

                }
            }
            if (checkAction) await Task.Delay(60); // chờ 60s để 
        }
        if (checkAction)
        {
            await Task.Delay(40);
        }

        _ground[positionPair.First, positionPair.Second].ThisEgg.UpLevelEgg(); // nâng cấp trứng sau merge

        //Init
        CheckMerge = false;
        SetMatrixEgg();
        await SetInitGround();
    }

    private void SetMatrixEgg()
    {
        CheckTileHaveEggCurrent();

        for (int j = 1; j <= column; ++j) // duyệt từng cột
        {
            for (int i = row; i >= row - _amoutEggOfColumn[j] + 1; --i) // trứng hiện có thì rơi xuống
            {
                if (_ground[i, j].ThisEgg == null)
                {
                    for (int iCheck = i - 1; iCheck >= 1; --iCheck)
                    {
                        if (_checkHaveEgg[iCheck, j])
                        {
                            _checkHaveEgg[iCheck, j] = false;
                            _ground[i, j].ThisEgg = _ground[iCheck, j].ThisEgg;
                            _ground[iCheck, j].ThisEgg.transform
                                .DOMove(_ground[i, j].transform.position, 0.1f)
                                .SetEase(Ease.OutQuad)
                                .OnComplete(() => _ground[i,j].SetCenterEgg());
                            _ground[iCheck, j].ThisEgg = null;
                            break;
                        }
                    }
                }
            }

            for (int i = row - _amoutEggOfColumn[j]; i >= 1; --i) // lấy trứng từ pool
            {
                _poolEgg[0].transform.position = _ground[1, j].transform.position + Vector3.up;
                _poolEgg[0].SetRandomEgg();
                _poolEgg[0].gameObject.SetActive(true);
                _ground[i, j].ThisEgg = _poolEgg[0];
                _poolEgg[0].transform
                    .DOMove(_ground[i, j].transform.position, 0.1f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => _ground[i, j].SetCenterEgg());
                _poolEgg.Remove(_poolEgg[0]);
            }
        }
    }

    private void CheckTileHaveEggCurrent()
    {
        _amoutEggOfColumn = new int[column + 5];
        for (int i = 1; i <= column; ++i)
        {
            _amoutEggOfColumn[i] = 0;
        }

        for (int j = 1; j <= column; ++j)
        {
            for (int i = 1; i <= row; ++i)
            {
                if (_ground[i, j].ThisEgg != null)
                {
                    _checkHaveEgg[i, j] = true;
                    ++_amoutEggOfColumn[j];
                }
                else
                {
                    _checkHaveEgg[i, j] = false;
                }
            }
        }

        //for (int j = 1; j <= column; ++j)
        //{
        //    Debug.Log(_amoutEggOfColumn[j]);
        //}
        //Debug.Log("------------------------------------------------");
    }

    private void CreatingTheWay(Pair<int, int> positionPair)
    {
        myQueue = new Queue<Pair<int, int>>(); // tạo mới 1 queue chứa hàng và cột
        myQueue.Enqueue(positionPair); // thêm ô hiện tại vào queue
        _numberArrMerge[positionPair.First, positionPair.Second] = 0;

        while (myQueue.Count > 0) // queue chưa rỗng
        {
            List<Pair<int, int>> myList = new List<Pair<int, int>>(); // danh sách các điểm đi

            Pair<int, int> myPosition = myQueue.Dequeue();  // lấy phản tử đầu tiên đồng thời xóa
            isVisited[myPosition.First, myPosition.Second] = true; // đánh dấu ô đã được duyệt
            if (_maxNumberMerge < _numberArrMerge[myPosition.First, myPosition.Second])
            {
                _maxNumberMerge = _numberArrMerge[myPosition.First, myPosition.Second];
            }

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
                    _numberArrMerge[positionPair.First, positionPair.Second] = _numberArrMerge[myPosition.First, myPosition.Second] + 1;
                    myList.Add(positionPair);
                }
            }

            if (myList.Count > 0)
            {
                _roadList.Add(new Pair<Pair<int, int>, List<Pair<int, int>>>(myPosition, myList));
            }
        }
    }
}
