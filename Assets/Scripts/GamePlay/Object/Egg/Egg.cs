using System.Security.Cryptography;
using Assets.Scripts.Common;
using Assets.Scripts.GamePlay.Object.Egg;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [Header("Component", order = 0)]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } set { spriteRenderer = value; } }
    [SerializeField] private Animator animator;

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

        PlayAudioMerge();

        GamePlayController.Instance.LevelMaxOfEgg = (int)type > GamePlayController.Instance.LevelMaxOfEgg ? (int)type : GamePlayController.Instance.LevelMaxOfEgg;
        infor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + type);
        spriteRenderer.sprite = infor.ImageGamePlay;

        Messenger.Broadcast(EventKey.EFFECT_EGG_UP, (int)type, row, col);
        PlayEffectEgg();
    }

    private void PlayEffectEgg()
    {
        //
    }

    private void PlayAudioMerge()
    {
        if ((int)type < 6)
        {
            SoundController.Instance.PlayOneShotAudio(GameConfig.ADD_SCORE_LEVEL1_5_AUDIO);
        }
        else if ((int)type < 8)
        {
            SoundController.Instance.PlayOneShotAudio(GameConfig.ADD_SCORE_LEVEL6_7_AUDIO);
        }
        else if ((int)type < 10)
        {
            SoundController.Instance.PlayOneShotAudio(GameConfig.ADD_SCORE_LEVEL8_9_AUDIO);
        }
        else if ((int)type < 13)
        {
            SoundController.Instance.PlayOneShotAudio(GameConfig.ADD_SCORE_LEVEL10_12_AUDIO);
        }
        else
        {
            SoundController.Instance.PlayOneShotAudio(GameConfig.ADD_SCORE_LEVEL_13_14_AUDIO);
        }
    }

    public void SetRandomLevel()
    {
        int randomLv;
        if (GamePlayController.Instance.LevelMaxOfEgg > 9)
        {
            randomLv = Random.Range(1, GamePlayController.Instance.LevelMaxOfEgg);
        }
        else
        {
            randomLv = Random.Range(1, Mathf.Min(4, GamePlayController.Instance.LevelMaxOfEgg));
        }
        type = (EggType)randomLv;
        infor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + type);
        spriteRenderer.sprite = infor.ImageGamePlay;
    }

}
