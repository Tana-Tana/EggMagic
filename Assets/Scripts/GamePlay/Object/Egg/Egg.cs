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

    private int _levelMaxOfEggType = 3;

    public int LevelMaxOfEggType { get { return _levelMaxOfEggType; } set {  _levelMaxOfEggType = value;}}

    public void LoadInitEgg()
    {
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + IdentifyInitId());
        SetInforEgg(eggInfor);
    }

    private string IdentifyInitId()
    {
        int number = Random.Range(0, _levelMaxOfEggType);
        eggType = (EggType)number;
        return eggType.ToString();
    }

    public void UpLevelEgg()
    {
        if (_levelMaxOfEggType < (int)eggType + 1)
        {
            _levelMaxOfEggType = (int)eggType + 1;
        }

        eggType = (EggType)((int)eggType + 1);
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType.ToString());
        SetInforEgg(eggInfor);
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
        if(_levelMaxOfEggType < 11)
        {
            SetLevelEasy();
        }
        else
        {
            SetLevelHard();
        }
    }

    private void SetLevelHard()
    {
        int number = Random.Range(_levelMaxOfEggType - 8, _levelMaxOfEggType);
        eggType = (EggType)number;
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType.ToString());
        SetInforEgg(eggInfor);
    }

    private void SetLevelEasy()
    {
        int number = Random.Range(_levelMaxOfEggType - 3, _levelMaxOfEggType);
        eggType = (EggType)number;
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + eggType.ToString());
        SetInforEgg(eggInfor);
    }
}
