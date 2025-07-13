using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    private float applySpeed;

    [SerializeField] private float jumpForce;

    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    private Vector3 lastPos;

    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider;

    [SerializeField] private float lookSensitivity;
    [SerializeField] private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    [SerializeField] private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = Object.FindFirstObjectByType<GunController>();
        theCrosshair = Object.FindFirstObjectByType<Crosshair>();
        theStatusController = Object.FindFirstObjectByType<StatusController>();

        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        MoveCheck();
        CameraRotation();
        CharacterRotation();
    }

    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) Crouch();
    }

    void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);
        applySpeed = isCrouch ? crouchSpeed : walkSpeed;
        applyCrouchPosY = isCrouch ? crouchPosY : originPosY;
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
            if (count > 15) break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrosshair.JumpingAnimation(!isGround);
    }

    void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
            Jump();
    }

    void Jump()
    {
        if (isCrouch) Crouch();
        theStatusController.DecreaseStamina(100);
        myRigid.linearVelocity = transform.up * jumpForce; // linearVelocity -> velocity·Î º¯°æ
    }

    void TryRun()
    {
        bool sp = theStatusController.GetCurrentSP() > 0;

        if (Input.GetKey(KeyCode.LeftShift) && sp) Running();
        if (Input.GetKeyUp(KeyCode.LeftShift) || !sp) RunningCancel();
    }

    void Running()
    {
        if (isCrouch) Crouch();
        theGunController.CancelFineSight();
        isRun = true;
        theCrosshair.RunningAnimation(true);
        theStatusController.DecreaseStamina(10);
        applySpeed = runSpeed;
    }

    void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(false);
        applySpeed = walkSpeed;
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 dir = (transform.right * x + transform.forward * z).normalized;
        myRigid.MovePosition(transform.position + dir * applySpeed * Time.deltaTime);
    }

    void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            isWalk = Vector3.Distance(lastPos, transform.position) >= 0.01f;
            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    void CharacterRotation()
    {
        float y = Input.GetAxisRaw("Mouse X");
        Vector3 rotY = new Vector3(0f, y, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(rotY));
    }

    void CameraRotation()
    {
        if (pauseCameraRotation) return;
        float x = Input.GetAxisRaw("Mouse Y") * lookSensitivity;
        currentCameraRotationX -= x;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private bool pauseCameraRotation = false;

    public IEnumerator TreeLookCoroutine(Vector3 target)
    {
        pauseCameraRotation = true;
        Quaternion dir = Quaternion.LookRotation(target - theCamera.transform.position);
        float destX = dir.eulerAngles.x;

        while (Mathf.Abs(destX - currentCameraRotationX) >= 0.5f)
        {
            Vector3 euler = Quaternion.Lerp(theCamera.transform.localRotation, dir, 0.3f).eulerAngles;
            theCamera.transform.localRotation = Quaternion.Euler(euler.x, 0f, 0f);
            currentCameraRotationX = theCamera.transform.localEulerAngles.x;
            yield return null;
        }

        pauseCameraRotation = false;
    }

    public bool GetRun() => isRun;
    public bool GetWalk() => isWalk;
    public bool GetCrouch() => isCrouch;
    public bool GetIsGround() => isGround;
}
