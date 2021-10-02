using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level; //상속받은 PlayerStat에서 사용하기 위해서 protected를 붙여줌 
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defense;
    [SerializeField]
    protected float _moveSpeed;


    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }


    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100; 
        _attack = 30;
        _defense = 5;
        _moveSpeed = 5.0f;
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        if(Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    public virtual void OnDead(Stat attacker)
    {
        PlayerStat playerStat = attacker.GetComponent<PlayerStat>();
        if(playerStat != null)
        {
            playerStat.Exp += 10;

            Debug.Log("경험치 10 획득");
        }

        Destroy(gameObject);
        Debug.Log(gameObject.name + " 사망");
    }
}
