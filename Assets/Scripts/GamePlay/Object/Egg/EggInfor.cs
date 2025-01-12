using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEgg", menuName = "EggInfor", order = 0)]
public class EggInfor : ScriptableObject
{
    [SerializeField][PreviewField(80)] 
    private Sprite imageGamePlay;

    [SerializeField][PreviewField(80)]
    private Sprite achievement;

    [SerializeField] private string level;

    public Sprite ImageGamePlay => imageGamePlay;
    public Sprite ImageAchivement => achievement;

    public string Level => level;
    private void OnValidate()
    {
        level = name;
    }
}
