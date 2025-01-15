using System.Security.Cryptography;
using Assets.Scripts.Common;
using Assets.Scripts.GamePlay.Object.Egg;
using Unity.VisualScripting;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [Header("Component", order = 0)]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } set { spriteRenderer = value; } }

    [Header("Atribute", order = 1)]
    [SerializeField] private EggInfor infor;
    public EggInfor Infor { get => infor; set => infor = value; }
    [SerializeField] private EggType type;
    public EggType Type { get => type; set => type = value; }
    private int row = 0; public int Row { get => row; set => row = value; }
    private int col = 0; public int Col { get => col; set => col = value; }
    private int orderOfMovement = 0; public int OrderOfMovement { get => orderOfMovement; set => orderOfMovement = value; }

    public bool isMove = false;

    public void LevelUp()
    {
        type = ((int)type + 1 < 14) ? (EggType)((int)type + 1) : (EggType)((int)type);
        infor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + type);
        spriteRenderer.sprite = infor.ImageGamePlay;
    }

    public void SetRandomLevel()
    {

    }


}
