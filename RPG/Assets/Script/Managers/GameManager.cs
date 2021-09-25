using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject player;

    GameObject monster;

    private void Start()
    {
        player = Resources.Load<GameObject>("Prefabs/Character/Player");
        Instantiate(player);

        monster = Resources.Load<GameObject>("Prefabs/Character/Enemy1");
        Instantiate(monster);

    }

    private void Update()
    {
    }
}
