using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBase : MonoBehaviour
{
    protected Animator animator;
    protected CharacterManager characterManager;

    public int GodID = 0;

    public int Power { get; private set; } = 0;
    public int Barrier { get; private set; } = 0;
     
    public int Positon { get; private set; } = -1;
    public bool bisSpawn { get; private set; } = false;

    public void Initialize()
    {
        if(animator==null) animator = GetComponentInChildren<Animator>();        
        if(characterManager ==null) characterManager = GameManager.GetManagerClass<CharacterManager>();
        characterManager.Gods.Add(this);
        animator.transform.localPosition = new Vector3(0,0,0);
        Positon = characterManager.currentHeroCardIndex;
    }

    public void SetPower(int power , int barrier)
    {
        this.Power = power;
        this.Barrier = barrier;
    }

    public void Spawn()
    {
        animator.SetTrigger("Spawn");
        animator.transform.localPosition = Vector3.zero;
        animator.transform.localRotation = Quaternion.Euler(0,0,0);
    }

    public void Idle()
    {
        animator.SetTrigger("Idle");
        animator.transform.localPosition = Vector3.zero;
        animator.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Debug.Log(GodID);
    }

    public void Death()
    {
        Power = 0;
        Positon = -1;
        animator.SetTrigger("Death");
    }

    public void Ready()
    {
        animator.SetTrigger("Ready");
        animator.transform.localPosition = Vector3.zero;
        animator.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void Attack(int damage , int type)
    {
        animator.SetTrigger("Attack");
        animator.transform.localPosition = Vector3.zero;
        animator.transform.localRotation = Quaternion.Euler(0, 0, 0);

    }

    public void Damagged(int damage)
    {
        Power -= DammageCalcul(damage);

        if (Power <= 0) Death();
        else animator.SetTrigger("Dammaged");
    }

    protected int DammageCalcul(int damage)
    {
        int temp = 0;
        Barrier -= damage;
        temp = Barrier < 0 ? 0 : Barrier;
        Barrier = 0;
        temp = Mathf.Abs(temp);

        return temp;
    }

    protected int BarrierDown(int damage)
    {       
        if(Barrier > 0)
        Barrier-= damage;
        int temp = Barrier < 0 ? 0 : Barrier;
        Barrier = Barrier < 0 ? 0 : Barrier;
        return temp;
    }

    private void OnEnable()
    {
        if (!bisSpawn) { Initialize(); transform.rotation = Quaternion.Euler(0, 90, 0);  Spawn();  bisSpawn = true; }
    }


}
