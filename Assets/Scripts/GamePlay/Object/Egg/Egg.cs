using System;
using Assets.Scripts.Common;
using Assets.Scripts.GamePlay.Object.Egg;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Egg : MonoBehaviour
{
    [Title("EggInspector")]
    [SerializeField] private Image image;
    public Image Image { get { return image; } set { image = value; } }


    [Title("Information Egg")]
    [SerializeField] private string idName;
    public string IdName { get { return idName; } set { idName = value; } }

    [SerializeField] private Sprite avatar;
    public Sprite Avatar { get { return avatar; } set { avatar = value; } }
    [SerializeField] private EggType eggType;
    public EggType EggType { get { return eggType; } set { eggType = value; } }


    private int _scoreEgg;
    public int ScoreEgg { get { return _scoreEgg; } set { _scoreEgg = value; } }

    public void LoadInitEgg()
    {
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + IdentifyInitId());
        SetInforEgg(eggInfor);
    }

    private string IdentifyInitId()
    {
        int number = Random.Range(1, GamePlayController.Instance.LevelMaxOfEgg + 1);
        eggType = (EggType)number;
        SetScoreEgg((int)eggType);
        return eggType.ToString();
    }

    public void UpLevelEgg()
    {
        eggType = (EggType)((int)eggType + 1);
        SetScoreEgg((int)eggType);
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType.ToString());
        SetInforEgg(eggInfor);

        if (GamePlayController.Instance.LevelMaxOfEgg < (int)eggType)
        {
            GamePlayController.Instance.LevelMaxOfEgg = (int)eggType;
        }
    }

    private void SetInforEgg(EggInfor eggInfor)
    {
        idName = eggInfor.IdName;
        avatar = eggInfor.Avatar;
        image.sprite = avatar;
        image.SetNativeSize();
    }

    public void SetRandomEgg()
    {
        if(GamePlayController.Instance.LevelMaxOfEgg < 11)
        {
            Debug.Log("Set Easy!");
            SetLevelEasy();
        }
        else
        {
            Debug.Log("Set Hard!");
            SetLevelHard();
        }
    }

    private void SetLevelHard()
    {
        int number = Random.Range(GamePlayController.Instance.LevelMaxOfEgg - 7, GamePlayController.Instance.LevelMaxOfEgg);
        eggType = (EggType)number;
        SetScoreEgg((int)eggType);
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType.ToString());
        SetInforEgg(eggInfor);
    }

    private void SetLevelEasy()
    {
        int number = Random.Range(1, Math.Min(4, GamePlayController.Instance.LevelMaxOfEgg));
        eggType = (EggType)number;
        SetScoreEgg((int)eggType);
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType.ToString());
        SetInforEgg(eggInfor);
    }

    private void SetScoreEgg(int number)
    {
        _scoreEgg = number;
    }
}
