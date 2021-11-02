using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QSkill : MonoBehaviour
{
    Vector3 mousePos;

    GameObject player;

    SkillController scon;

    QSkill qskillScp;

    Vector3 dir;

    public Vector3 spreadDir;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        qskillScp = gameObject.GetComponent<QSkill>();

        scon = player.GetComponent<SkillController>();

        mousePos = scon.mousePos;

        dir = mousePos - transform.position;
    }


    void Update()
    {
        Fire();
    }

    void Fire()
    {
        transform.position += dir * 3 * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public GameObject firstHit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Stat stat = other.GetComponent<Stat>();

            PlayerStat ps = player.GetComponent<PlayerStat>();

            stat.OnSkilled(ps, 1);

            Debug.Log(other.gameObject.name);
            firstHit = other.gameObject;

            spread();
            //qskillScp.enabled = false;
            Destroy(gameObject);
        }
    }

    //public Collider[] colls;

    void spread()
    {
        Debug.Log("1단 발사");

        int mask = (1 << 9);
        Collider[] colls = Physics.OverlapSphere(transform.position, 5.0f, mask);

        Collider[] colllls = Physics.OverlapBox(transform.position, new Vector3(5, 5, 5));

        foreach (Collider coll in colls)
        {
            if(coll.gameObject != firstHit)
            {
                GameObject target = coll.gameObject;
                //GameObject q = Resources.Load<GameObject>("Prefabs/SpreadQskill");

                GameObject qspread = Instantiate(Resources.Load<GameObject>("Prefabs/SpreadQskill"), transform.position, transform.rotation);

                spreadQskill sq = qspread.GetComponent<spreadQskill>();

                sq.target = target;
            }
        }
    }
}
