using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadWSkill : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        transform.Translate(Vector3.forward * 7 * Time.deltaTime);

        Destroy(gameObject, 10.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Map"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Stat stat = other.GetComponent<Stat>();

            PlayerStat ps = player.GetComponent<PlayerStat>();

            stat.OnSkilled(ps, 2);
            Destroy(gameObject);
        }
    }
}
