using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    // 기존 위치.
    private Vector3 originPos;

    // 현재 위치.
    private Vector3 currentPos;

    // sway 한계.
    [SerializeField]
    private Vector3 limitPos;

    // 정조준 sway 한계.
    [SerializeField]
    private Vector3 fineSightLimitPos;


    // 부드러운 움직임 정도.
    [SerializeField]
    private Vector3 smoothSway;


    // 필요한 컴포넌트/
    [SerializeField]
    private GunController theGunController;


    // Use this for initialization
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
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
