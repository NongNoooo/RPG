using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSkill : MonoBehaviour
{
    Vector3 mousePos;

    GameObject player;

    SkillController scon;

    Vector3 dir;

    GameObject wSpread;

    void Start()
    {
        wSpread = Resources.Load<GameObject>("Prefabs/W_Skill_Spread");

        player = GameObject.FindGameObjectWithTag("Player");

        scon = player.GetComponent<SkillController>();

        mousePos = scon.mousePos;

        dir = mousePos - transform.position;

        StartCoroutine("Spread");
    }

    void Update()
    {
        Fire();
        Destroy(gameObject, 4.0f);
    }

    void Fire()
    {
        transform.position += dir * 0.2f * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.Rotate(transform.up * 500 * Time.deltaTime);
    }

    IEnumerator Spread()
    {
        Debug.Log("스킬 발동");
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            Debug.Log("스킬 오브젝트 생성");
            Instantiate(wSpread, transform.position, transform.rotation);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
