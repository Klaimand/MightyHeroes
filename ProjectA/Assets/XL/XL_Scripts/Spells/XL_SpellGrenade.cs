using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_SpellGrenade : MonoBehaviour, XL_ISpells
{
    [SerializeField] private XL_SpellGrenadeAttributes grenadeAttributes;
    [SerializeField] private float throwingRange;
    [SerializeField] private float travelTime;
    [SerializeField] private int explosionDamage;
    [SerializeField] private float explosionRadius;
    private GameObject grenade;
    private float[] startingVelocity;
    private void Awake()
    {
        grenadeAttributes.Initialize();
    }

    public void ActivateSpell(Vector3 throwingDirection)
    {
        Debug.Log("throwing direction " + throwingDirection);
        grenade = XL_Pooler.instance.PopPosition("BlastGrenade", transform.position + transform.forward);
        startingVelocity = XL_Utilities.GetVelocity(0.5f, throwingRange - 1, travelTime);
        Debug.Log("Velocity : "+startingVelocity[0]+", "+startingVelocity[1]);
        grenade.GetComponent<Rigidbody>().velocity = new Vector3(startingVelocity[0] * (throwingDirection.x), startingVelocity[1], startingVelocity[0] * (throwingDirection.z));
        grenade.GetComponent<XL_Grenade>().SetValue(explosionDamage, explosionRadius);
    }
}
