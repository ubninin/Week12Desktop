using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    //ㅅ
    [SerializeField] private float walkSpeed;
    [SerializeField] 
    private float runSpeed;
    private float applySpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpForce;

    private bool isRun = false;
    private bool isWalk = false;
    private bool isGround = true;
    private bool isCrouch = false;

    private Vector3 lastPos;
    //얼마나 앉을지
    [SerializeField] 
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider;
    [SerializeField] private float lookSensitivity;
    [SerializeField] 
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    [SerializeField] 
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;
    private Crosshair theCrosshair;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

        myRigid = GetComponent<Rigidbody>();
      
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        WeaponManager.isChangeWeapon = false;
        originPosY = theCamera.transform.localPosition.y;
        applySpeed = walkSpeed;
        applyCrouchPosY = originPosY;

    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        
        CameraRotation();
        CharacterRotation();
    }
    void FixedUpdate()
    {
        MoveCheck();
    }
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
            {
                break;
            }
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY,0f);
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y+0.1f);
        theCrosshair.JumpingAnimation(!isGround);
    }
    private void TryJump()
    {


        if(Input .GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (isCrouch)
        {
            Crouch();
        }
        myRigid.velocity = transform.up * jumpForce;
    }
    private void TryRun()
    {
        if(Input.GetKey (KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    private void Running()
    {
        if (isCrouch)
        {
            Crouch();
        }
        theGunController.CancelFineSight();
        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized *applySpeed;
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

    }
    private void MoveCheck()
    {
        if(!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos,transform.position)>=0.01f)
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }
            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }

    }
    private void CharacterRotation()
    {
        //좌우 캐릭터회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        //Debug.Log(myRigid.rotation.eulerAngles);
    }
    private void CameraRotation()
    {
        //상하 카메라회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

}
