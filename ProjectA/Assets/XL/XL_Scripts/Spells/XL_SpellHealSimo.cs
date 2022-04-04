using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_SpellHealSimo : MonoBehaviour, XL_ISpells
{
    List<GameObject> players;
    [SerializeField] private XL_SpellHealSimoAttributes healAttributes;

    private void Start()
    {
        healAttributes.Initialize();
        players = XL_GameManager.instance.players;
    }

    public void ActivateSpell(Vector3 direction)
    {
        StartCoroutine(HealCoroutine(healAttributes.duration));
    }

    IEnumerator HealCoroutine(float t) 
    {
        while (t > 0) 
        {
            players = XL_GameManager.instance.players;
            foreach (GameObject player in players) 
            {
                if ((player.transform.position - player.transform.position).magnitude < healAttributes.healingZoneRadius) player.GetComponent<XL_IDamageable>().TakeDamage(-healAttributes.healingAmount);
            }

            yield return new WaitForSeconds(1);
            t--;
        }
    }

}
