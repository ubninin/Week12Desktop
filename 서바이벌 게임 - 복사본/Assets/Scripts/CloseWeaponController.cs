using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{
    // 현재 장착
    [SerializeField] protected CloseWeapon currentCloseWeapon;

    protected bool isAttack = false;
    protected bool isSwing = false;
    protected RaycastHit hitInfo;

    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;


        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;


        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }


    // 미완성 ( 추상함수)
    protected abstract IEnumerator HitCoroutine();


    protected bool CheckObject()
    {


        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }



    // 추가 편집 가능(가상함수)
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
