using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFactory : MonoBehaviour
{
    public int monsterCount = 0;

    int keepMonsterCount = 5;

    Vector3 spawnPos = Vector3.zero;

    float spawnRadius = 15.0f;

    float spawnTime = 3.0f;


    void Start()
    {
        
    }

    void Update()
    {
        AddMonster();
    }

    void AddMonster()
    {
        if(monsterCount < keepMonsterCount)
        {
            StartCoroutine("MonsterGenerate");
        }
    }

    IEnumerator MonsterGenerate() // 코루틴 이용 
    {
        monsterCount++;

        yield return new WaitForSeconds(Random.Range(0, spawnTime));

        GameObject _monster = Resources.Load<GameObject>("Prefabs/Character/Enemy1");
        GameObject monster = Instantiate(_monster);

        NavMeshAgent nma = monster.GetComponent<NavMeshAgent>();

        Vector3 rndPos;

        while (true)
        {
            Vector3 rndDir = Random.insideUnitSphere * Random.Range(0, spawnRadius); //insideunitSphere를 이용하면 원의 범위내에서 랜덤한 좌표를 뽑아오게됨
            rndDir.y = 0; // 땅 아래 생성되지 않게
            rndPos = spawnPos + rndDir;

            NavMeshPath path = new NavMeshPath();
            if(nma.CalculatePath(rndPos, path)) //rndPos가 갈수 있는 길인지 아닌지 체크
            {
                break;//갈수 있는 길일경우 반복문 탈출
            }
        }

        monster.transform.position = rndPos;
    }
}
