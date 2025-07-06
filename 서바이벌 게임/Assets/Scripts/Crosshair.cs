using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private float gunAccuracy;
    [SerializeField]
    private GameObject go_CrosshairHUD;
    [SerializeField] private GunController theGunController;

    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("w", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("r", _flag);
    }
    public void CrouchingAnimation(bool _flag)
    {
        animator.SetBool("c", _flag);
    }
    public void FineSightAnimation(bool _flag)
    {
        animator.SetBool("fs", _flag);
    }
    public void FireAnimation()
    {
        if (animator.GetBool("w")) animator.SetTrigger("wf");
        else if (animator.GetBool("c")) animator.SetTrigger("cf");
        else animator.SetTrigger("if");
    }
    public float GetAccuracy()
    {
        if (animator.GetBool("w")) gunAccuracy = 0.06f;
        else if (animator.GetBool("c")) gunAccuracy = 0.015f;
        else if (theGunController.GetFineSightMode()) gunAccuracy = 0.001f;

        else gunAccuracy = 0.035f;
        return gunAccuracy;
    }
    void Update()
    {
        
    }
}
