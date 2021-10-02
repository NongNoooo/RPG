using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat //공통되는 부분을 중복으로 사용하지않기위해 stat스크립트를 상속받음
{
    [SerializeField]
    int _exp;
    [SerializeField]
    int _gold;
    [SerializeField]
    int _maxExp;

    public int Exp { get { return _exp; } set { _exp = value; } }
    public int Gold { get { return _gold; } set { _gold = value; } }
    public int MaxExp { get { return _maxExp; } set { _maxExp = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 50;
        _defense = 5;
        _moveSpeed = 5.0f;
        _exp = 0;
        _gold = 0;
        _maxExp = 10;

    }

    public void Update()
    {
        LevelUp();
        StatUp();
    }

    void statCheck()
    {
        _hp += Level * 100;
        _maxHp += _level * 100;
        _maxExp += _level * 10;
    }

    public int statPoint = 0;
    void LevelUp()
    {
        if(_maxExp <= _exp)
        {
            Debug.Log("레벨 업");
            _level++;
            statCheck();
            statPoint++;
        }
    }

    void StatUp()
    {
        if (Input.GetKey(KeyCode.H))
        {
            if(statPoint > 0)
            {
                _hp += 10;
                statPoint--;
            }
        }
    }
}
