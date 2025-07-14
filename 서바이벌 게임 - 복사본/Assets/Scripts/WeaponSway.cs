using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    private Vector3 originPos;

    private Vector3 currentPos;

    [SerializeField]
    private Vector3 limitPos;

    [SerializeField]
    private Vector3 fineSightLimitPos;


    [SerializeField]
    private Vector3 smoothSway;


    [SerializeField]
    private GunController theGunController;


    void Start()
    {
        originPos = this.transform.localPosition;
    }


    void Update()
    {
        if (!Inventory.inventoryActivated)
            TrySway();
    }


    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        else
            BackToOriginPos();
    }

    private void Swaying()
    {
        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");

        Vector3 targetOffset;

        if (!theGunController.isFineSightMode)
        {
            targetOffset = new Vector3(
                Mathf.Clamp(-moveX, -limitPos.x, limitPos.x),
                Mathf.Clamp(-moveY, -limitPos.y, limitPos.y),
                0f
            );
        }
        else
        {
            targetOffset = new Vector3(
                Mathf.Clamp(-moveX, -fineSightLimitPos.x, fineSightLimitPos.x),
                Mathf.Clamp(-moveY, -fineSightLimitPos.y, fineSightLimitPos.y),
                0f
            );
        }

        currentPos = Vector3.Lerp(currentPos, originPos + targetOffset, Time.deltaTime * smoothSway.x);
        transform.localPosition = currentPos;
    }


    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
