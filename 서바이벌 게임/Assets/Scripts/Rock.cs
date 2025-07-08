using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField]private int hp; 

    [SerializeField]private float destroyTime; 

    [SerializeField]private SphereCollider col; 

    [SerializeField]
    private GameObject go_rock; // 일반 바위
    [SerializeField]
    private GameObject go_debris; // 깨진 바위
    [SerializeField]
    private GameObject go_effect_prefabs; // 채굴 이펙트

    //사운드 이름
    [SerializeField]private string strike_Sound;
    [SerializeField]private string destroy_Sound;


    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);

        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);
        hp--;

        if (hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);
        col.enabled = false;
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }

}
