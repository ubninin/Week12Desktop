using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{

    // Ȱ��ȭ ����
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
                    Debug.LogWarning("Rock ������Ʈ�� ã�� ���߽��ϴ�: " + hitInfo.transform.name);
                }
                isSwing = false;
                Debug.Log("��Ʈ ������Ʈ: " + hitInfo.transform.name + " / �±�: " + hitInfo.transform.tag);
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
