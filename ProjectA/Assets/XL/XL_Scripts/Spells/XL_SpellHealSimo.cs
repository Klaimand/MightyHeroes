using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_SpellHealSimo : XL_Spells
{
    List<GameObject> players;
    Transform pos;
    [SerializeField] private XL_SpellHealSimoAttributesSO healAttributes;

    private void Start()
    {
        healAttributes.Initialize();
        players = XL_GameManager.instance.players;
    }

    public override void ActivateSpell(Vector3 direction, Transform pos)
    {
        this.pos = pos;
        StartCoroutine(HealCoroutine(healAttributes.duration));
    }

    IEnumerator HealCoroutine(float t) 
    {
        while (t > 0) 
        {
            players = XL_GameManager.instance.players;
            foreach (GameObject player in players) 
            {
                if ((player.transform.position - pos.position).magnitude < healAttributes.healingZoneRadius) player.GetComponent<XL_IDamageable>().TakeDamage(-healAttributes.healingAmount);
            }

            yield return new WaitForSeconds(1);
            t--;
        }
    }

}
