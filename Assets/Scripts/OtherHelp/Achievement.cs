using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    public GameObject Ground {  get { return ground; } set { ground = value; } }
    [SerializeField] private Image egg;
    public Image Egg { get { return  egg; } set { egg = value; } }
    [SerializeField] private GameObject hideGround;
    public GameObject HideGround { get { return hideGround; } set { hideGround = value; } }

}
