using System;
using Assets.Scripts.Common;
using Assets.Scripts.GamePlay.Object.Egg;
using UnityEngine;
using Random = UnityEngine.Random;

public class EggController : MonoBehaviour
{
    [Header("Egg", order = 0)]
    [SerializeField] private Egg eggPrefab;
    [SerializeField] private Transform eggsTranform;

    // private
    private EggPool eggPool;
    private Egg[,] eggs;
    private float xEgg = -1.65f;
    private float yEgg = 0.42f;
    private int countSortingOrder = 3;

    private void Start()
    {
        eggPool = new EggPool();
        eggs = new Egg[10,10];
        GenerateEgg(eggPrefab);
        RandomLevelStart(eggs);
        Messenger.Broadcast(EventKey.SET_TYPE_TILE, eggs);
    }

    private void RandomLevelStart(Egg[,] eggs)
    {
        for(int i = 1; i <= 5; ++i)
        {
            for (int j = 1; j <= 5; ++j)
            {
                EggType eggType = (EggType)Random.Range(1,4); // random level 1 -> 3
                eggs[i,j].Infor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType); // load infor cua level vao trung
                eggs[i, j].SpriteRenderer.sprite = eggs[i, j].Infor.ImageGamePlay; // load sprite level
            }
        }
    }

    private void GenerateEgg(Egg eggPrefab)
    {
        for (int i = 1; i <= 5; ++i)
        {
            for (int j = 1; j <= 5; ++j)
            {
                Egg egg = Instantiate(eggPrefab, eggsTranform); // sinh trứng
                egg.transform.position = new Vector3(xEgg + 0.82f * (j - 1), yEgg, 0); // Đặt lại tọa độ cho trứng
                egg.SpriteRenderer.sortingLayerName = GameConfig.OBJECT_LAYER; // Đặt lại sorting layer
                egg.SpriteRenderer.sortingOrder = countSortingOrder;
                eggs[i, j] = egg; // thêm trứng vào ma trận để dễ xử lí
            }
            countSortingOrder += 2;
            yEgg -= 0.8f;
        }
    }

}
