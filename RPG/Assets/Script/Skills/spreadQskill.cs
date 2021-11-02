using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spreadQskill : MonoBehaviour
{
    Vector3 fPos;

    public GameObject target;

    GameObject firstHit;

    GameObject player;

    GameObject qspread;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        qspread = Resources.Load<GameObject>("Prefabs/SpreadQskill2");

    }


    void Update()
    {
        Move();

        if (target = null)
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        Vector3 dir = target.transform.position - transform.position;

        transform.position += dir * 2 * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
        {
            Stat stat = other.GetComponent<Stat>();
            PlayerStat ps = player.GetComponent<PlayerStat>();

            firstHit = other.gameObject;

            stat.OnSpreadQ(ps);
            if (gameObject.tag != "Q_Skill_Spread2")
            {
                Spread();
            }
            Destroy(gameObject);
        }
    }

    int i = 0;

    void Spread()
    {
        Debug.Log("2단발사");
        int mask = (1 << 9);
        Collider[] colls = Physics.OverlapSphere(transform.position, 5.0f, mask);

        foreach (Collider coll in colls)
        {
            if (coll.gameObject != firstHit)
            {
                if(i < 2)
                {
                    GameObject target = coll.gameObject;

                    if(target != null)
                    {
                        Instantiate(qspread, transform.position, transform.rotation);

                        spreadQskill sq = qspread.GetComponent<spreadQskill>();

                        sq.target = target;

                        i++;
                    }
                }
            }
        }
    }
}
