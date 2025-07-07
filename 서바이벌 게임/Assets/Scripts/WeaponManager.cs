using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isChangeWeapon = false;
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    [SerializeField] private string currentWeaponType;

    [SerializeField] private float changeWeaponDelayTime;
    [SerializeField] private float changeWeaponEndDelayTime;

    [SerializeField] private Gun[] guns;
    [SerializeField] private Hand[] hands;

    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    [SerializeField] private GunController theGunController;
    [SerializeField] private HandController theHandController;
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }


    void Update()
    {
        
        if (!isChangeWeapon)
        {
            //Debug.Log("Update 실행됨");
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log('1');
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log('2');
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;

        if (currentWeaponAnim == null)
        {
            Debug.LogWarning("currentWeaponAnim이 null입니다! 무기 애니메이션 트리거 설정 실패");
        }
        else
        {
            currentWeaponAnim.SetTrigger("Weapon_Out");
        }

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);
        currentWeaponType = _type;
        isChangeWeapon = false;
    }


    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActive = false;
                break;
            case "HAND":
                HandController.isActive = false;
                break;
        }
    }
    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            theGunController.GunChange(gunDictionary[_name]);
        }
        else if (_type == "HAND")
        {
            theHandController.HandChange(handDictionary[_name]); ;
        }
    }
}
