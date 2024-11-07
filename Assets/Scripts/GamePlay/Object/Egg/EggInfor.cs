using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEgg", menuName = "EggInfor", order = 0)]
public class EggInfor : ScriptableObject
{
    [SerializeField][PreviewField(80)] 
    private Sprite avatar;
    [SerializeField] private string idName;
    [SerializeField] private Animator movement;

    public Sprite Avatar => avatar;
    public string IdName => idName;
    public Animator Movement => movement;
}
