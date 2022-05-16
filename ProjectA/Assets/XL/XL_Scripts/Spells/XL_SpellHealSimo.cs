using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_SpellHealSimo : XL_Spells
{
    List<GameObject> players;
    //Transform pos;
    Transform simo;
    GameObject curHeal;
    [SerializeField] private XL_SpellHealSimoAttributesSO healAttributes;

    void Start()
    {
        healAttributes.Initialize();
        players = XL_GameManager.instance.players;
    }

    void Update()
    {
        //transform.position = XL_GameManager.instance.players[0].transform.position;
    }

    public override void ActivateSpell(Vector3 direction, Transform pos)
    {
        if (curHeal != null) return;
        simo = pos;
        curHeal = XL_Pooler.instance.PopPosition("SimoHeal", simo.position, simo);
        StartCoroutine(HealCoroutine(healAttributes.duration));
    }

    IEnumerator HealCoroutine(float t)
    {
        while (t > 0)
        {
            players = XL_GameManager.instance.players;
            foreach (GameObject player in players)
            {
                if ((player.transform.position - simo.position).magnitude < healAttributes.healingZoneRadius) player.GetComponent<XL_IDamageable>().TakeDamage(-healAttributes.healingAmount);
            }

            yield return new WaitForSeconds(1);
            t--;
        }
        XL_Pooler.instance.DePop("SimoHeal", curHeal);
        curHeal = null;
    }

}
