using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEgg", menuName = "EggInfor", order = 0)]
public class EggInfor : ScriptableObject
{
    [SerializeField][PreviewField(80)] 
    private Sprite avatar;

    [SerializeField][PreviewField(80)]
    private Sprite icon;

    [SerializeField] private string idName;
    [SerializeField] private Animator movement;

    public Sprite Avatar => avatar;
    public Sprite Icon => icon;

    public string IdName => idName;
}
