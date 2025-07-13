using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{

    // 활성화 여부
    public static bool isActivate = false;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                Rock rock = hitInfo.collider.GetComponentInParent<Rock>();
                if (rock != null)
                {
                    rock.Mining();
                }
                else
                {
                    Debug.LogWarning("Rock 컴포넌트를 찾지 못했습니다: " + hitInfo.transform.name);
                }
                isSwing = false;
                Debug.Log("히트 오브젝트: " + hitInfo.transform.name + " / 태그: " + hitInfo.transform.tag);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
