using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    public string handName; //너클
    public float range;
    public int damage;
    public float workSpeed;
    public float attackDelay; //공격 딜레이
    public float attackDelayA; //공격 활성화시점
    public float attackDelayB; //공격 비활성화 시점
    [SerializeField]
    public Animator anim;

}
