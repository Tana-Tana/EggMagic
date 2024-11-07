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
    [SerializeField] private EggType eggType;

    private int _levelMaxOfEggType = 3;
    public int LevelMaxOfEggType { get { return _levelMaxOfEggType; } set {  _levelMaxOfEggType = value;}}

    public void LoadInitEgg()
    {
        EggInfor eggInfor = Resources.Load<EggInfor>(GameConfig.EGG_INFOR_PATH + IdentifyInitId());
        idName = eggInfor.IdName;
        avatar = eggInfor.Avatar;
        image.sprite = avatar;
        image.SetNativeSize();
    }

    private string IdentifyInitId()
    {
        int number = Random.Range(0, _levelMaxOfEggType);
        eggType = (EggType)number;
        return eggType.ToString();
    }
}
