using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    [SerializeField] private int hp;
    private int currentHp;

    [SerializeField] private int sp;
    private int currentSp;

    [SerializeField] private int spIncreaseSpeed;
    [SerializeField] private int spRechargeTime;
    private int currentSpRechargeTime;
    private bool spUsed;

    [SerializeField] private int dp;
    private int currentDp;

    [SerializeField] private int hungry;
    private int currentHungry;
    [SerializeField] private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    [SerializeField] private int thirsty;
    private int currentThirsty;
    [SerializeField] private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    [SerializeField] private int satisfy;
    private int currentSatisfy;

    [SerializeField] private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GaugeUpdate();
    }

    void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }

    void SPRecover()
    {
        if (!spUsed && currentSp < sp)
            currentSp += spIncreaseSpeed;
    }

    void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("배고픔 0");
    }

    void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("목마름 0");
    }

    void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void IncreaseHP(int _count)
    {
        currentHp = Mathf.Min(currentHp + _count, hp);
    }

    public void DecreaseHP(int _count)
    {
        if (currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }

        currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("hp 0");
    }

    public void IncreaseDP(int _count)
    {
        currentDp = Mathf.Min(currentDp + _count, dp);
    }

    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("dp 0");
    }

    public void IncreaseHungry(int _count)
    {
        currentHungry = Mathf.Min(currentHungry + _count, hungry);
    }

    public void DecreaseHungry(int _count)
    {
        currentHungry = Mathf.Max(currentHungry - _count, 0);
    }

    public void IncreaseThirsty(int _count)
    {
        currentThirsty = Mathf.Min(currentThirsty + _count, thirsty);
    }

    public void DecreaseThirsty(int _count)
    {
        currentThirsty = Mathf.Max(currentThirsty - _count, 0);
    }

    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;
        currentSp = Mathf.Max(currentSp - _count, 0);
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }
}
