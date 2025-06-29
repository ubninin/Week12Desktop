using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    public string handName; //��Ŭ
    public float range;
    public int damage;
    public float workSpeed;
    public float attackDelay; //���� ������
    public float attackDelayA; //���� Ȱ��ȭ����
    public float attackDelayB; //���� ��Ȱ��ȭ ����
    [SerializeField]
    public Animator anim;

}
