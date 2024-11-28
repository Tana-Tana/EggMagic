using Assets.Scripts.Common;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [Header("----------Egg And Ground----------")]
    [SerializeField] private Egg thisEgg;
    public Egg ThisEgg {  get { return thisEgg; } set { thisEgg = value; } }

    private Pair<int, int> _positionPair;
    public Pair<int, int> PositionPair { get { return _positionPair; } set { _positionPair = value; } }

    public bool isSelected = false;

    private void Update()
    {
        if(thisEgg!= null)
        {
            thisEgg.transform.SetParent(transform);
        }
    }

    public void SetCenterEgg()
    {
        if(thisEgg != null)
        {
            thisEgg.transform.localPosition = Vector3.zero;
        }
    }

    public async void OnClick()
    {
        if(!GamePlayController.Instance.CheckEndGame)
        {
            if (ObjectController.Instance.isFinishMerge)
            {
                if (!isSelected && !ObjectController.Instance.isPrepareMerge)  // bảng init
                {
                    ObjectController.Instance.DoSomethingWithGroundIsSelected(thisEgg.IdName, _positionPair);
                }
                else
                {
                    if (!isSelected && ObjectController.Instance.isPrepareMerge) // đang có ô pending merge
                    {
                        await ObjectController.Instance.SetInitGround();
                    }
                    else
                    {
                        if (isSelected)
                        {
                            Debug.Log("Thực hiện merge");
                            ObjectController.Instance.MergeEgg(_positionPair);
                        }
                    }
                }
            }
        }
    }

}
