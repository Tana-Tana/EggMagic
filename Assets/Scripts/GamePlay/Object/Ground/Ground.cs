using Assets.Scripts.Common;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [Header("----------Egg And Ground----------")]
    [SerializeField] private Egg thisEgg;
    public Egg ThisEgg {  get { return thisEgg; } set { thisEgg = value; } }

    [SerializeField] private bool checkHaveEgg = true;
    public bool CheckHaveEgg { get { return checkHaveEgg; } set { checkHaveEgg = value; } }

    [SerializeField] private string levelGround;
    public string LevelGround { get { return levelGround; } set { levelGround = value; } }

    private Pair<int, int> _positionPair;
    public Pair<int, int> PositionPair { get { return _positionPair; } set { _positionPair = value; } }

    public bool isSelected = false;

    public void OnClick()
    {
        if (!isSelected && !ObjectController.Instance.isPrepareMerge)  // bảng init
        {
            ObjectController.Instance.DoSomethingWithGroundIsSelected(levelGround, _positionPair);
        }
        else
        {
            if(!isSelected && ObjectController.Instance.isPrepareMerge) // đang có ô pending merge
            {
                ObjectController.Instance.SetInitGround();
            }
            else
            {
                if(isSelected)
                {
                    Debug.Log("Thực hiện merge");
                    ObjectController.Instance.MergeEgg(_positionPair);
                }
            }
        }
    }
}
