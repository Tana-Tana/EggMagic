using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEgg", menuName = "EggInfors", order = 0)]
public class EggInfors : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string id;
    [SerializeField] private Animator thisAnimator;

    public Sprite Icon { get; set; }
    public string Id { get; set; }
    public Animator ThisAnimator { get; set; }

    private void OnValidate()
    {
        this.id = name;
    }
}
