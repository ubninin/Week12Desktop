using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun currentGun;
    private float currentFireRate;
    private bool isReload = false;
    public bool isFineSightMode = false;

    [SerializeField] private Vector3 originPos;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        //TryReload();
        //TryFineSight();
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }

    }
    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }
    private void Shoot()
    {
        PlaySE(currentGun.fire_Sound);
        currentGun .muzzleFlash.Play();
        Debug.Log("shoot");

    }
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}