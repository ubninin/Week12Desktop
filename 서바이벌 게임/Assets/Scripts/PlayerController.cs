using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float crouchPosY;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float cameraRotationLimit;
    [SerializeField] private Camera theCamera;

    private float applySpeed;
    private float originPosY;
    private float applyCrouchPosY;
    private float currentCameraRotationX = 0;

    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;
    private bool pauseCameraRotation = false;

    private Vector3 lastPos;

    private CapsuleCollider capsuleCollider;
    private Rigidbody myRigid;
    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();

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
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();
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

            if (count > 15)
                break;

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
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
            Jump();
    }

    void Jump()
    {
        if (isCrouch)
            Crouch();

        theStatusController.DecreaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;
    }

    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift)) Running();
        if (Input.GetKeyUp(KeyCode.LeftShift)) RunningCancel();
    }

    void Running()
    {
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();
        isRun = true;

        theCrosshair.RunningAnimation(isRun);
        theStatusController.DecreaseStamina(10);
        applySpeed = runSpeed;
    }

    void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized * applySpeed;
        myRigid.MovePosition(transform.position + move * Time.deltaTime);
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
        float y = Input.GetAxisRaw("Mouse X") * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(0f, y, 0f));
    }

    void CameraRotation()
    {
        if (!pauseCameraRotation)
        {
            float x = Input.GetAxisRaw("Mouse Y") * lookSensitivity;
            currentCameraRotationX -= x;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    public IEnumerator TreeLookCoroutine(Vector3 target)
    {
        pauseCameraRotation = true;

        Quaternion direction = Quaternion.LookRotation(target - theCamera.transform.position);
        float destinationX = direction.eulerAngles.x;

        while (Mathf.Abs(destinationX - currentCameraRotationX) >= 0.5f)
        {
            Vector3 euler = Quaternion.Lerp(theCamera.transform.localRotation, direction, 0.3f).eulerAngles;
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
