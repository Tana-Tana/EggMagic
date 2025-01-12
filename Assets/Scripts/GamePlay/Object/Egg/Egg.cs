using Assets.Scripts.GamePlay.Object.Egg;
using UnityEngine;


public class Egg : MonoBehaviour
{
    [Header("Component", order = 0)]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } set { spriteRenderer = value; } }

    [Header("Atribute", order = 1)]
    [SerializeField] private EggInfor infor;
    public EggInfor Infor { get => infor; set => infor = value; }

}
