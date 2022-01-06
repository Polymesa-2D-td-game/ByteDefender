using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class Buffable : MonoBehaviour
{
    [SerializeField] private GameObject buffEffect;
    [SerializeField] private float powerBuffModifier;
    [SerializeField] private float speedBuffModifier;
    [SerializeField] private float rangeBuffModifier;

    private Tower tower;
    private Spawner spawner;

    private float powerBuff = 0f;
    private float speedBuff = 0f;
    private float rangeBuff = 0f;

    //Awake is called when the GameObject activates
    private void Awake()
    {
        tower = GetComponent<Tower>();
        spawner = FindObjectOfType<Spawner>();
    }

    //Buff nearby towers
    public void Buff(float rangeBuff, float speedBuff, float powerBuff)
    {
        powerBuff = powerBuff * powerBuffModifier;
        speedBuff = speedBuff * speedBuffModifier;
        rangeBuff = rangeBuff * rangeBuffModifier;

        tower.CurrentPower = tower.Power + powerBuff;
        tower.CurrentSpeed = tower.Speed + speedBuff;
        tower.CurrentRange = tower.Range + rangeBuff;
    }

    //Play buff animation
    public void BuffAnimation()
    {
        buffEffect.GetComponent<ParticleSystem>().Play();
    }

    //Debuff all towers
    public void Debuff()
    {
        tower = GetComponent<Tower>();
        spawner = FindObjectOfType<Spawner>();
        tower.CurrentPower = tower.Power;
        tower.CurrentSpeed = tower.Speed;
        tower.CurrentRange = tower.Range;
    }
}
