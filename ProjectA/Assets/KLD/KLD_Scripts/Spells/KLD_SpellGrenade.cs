using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSpellGrenade", menuName = "KLD/Spell/Grenade", order = 1)]
public class KLD_SpellGrenade : KLD_Spell
{
    [SerializeField] XL_SpellGrenadeAttributesSO grenadeAttributes;

    GameObject instGrenade;

    Vector3 grenadeVel = Vector3.zero;
    float[] startingVelocity;

    public override void ActivateSpell(Vector3 direction, Transform pos)
    {
        instGrenade = XL_Pooler.instance.PopPosition("BlastGrenade", pos.position + pos.forward + pos.up);
        startingVelocity = XL_Utilities.GetVelocity(0.5f, Mathf.Lerp(grenadeAttributes.minThrowingDistance, grenadeAttributes.throwingDistance, direction.magnitude), grenadeAttributes.travelTime);
        grenadeVel.x = startingVelocity[0] * (direction.x);
        grenadeVel.y = startingVelocity[1];
        grenadeVel.z = startingVelocity[0] * (direction.z);
        instGrenade.GetComponent<Rigidbody>().velocity = grenadeVel;
        //grenade.GetComponent<Rigidbody>().velocity = new Vector3(startingVelocity[0] * (throwingDirection.x), startingVelocity[1], startingVelocity[0] * (throwingDirection.z));
        instGrenade.GetComponent<XL_Grenade>().SetValue(grenadeAttributes.explosionDamage, grenadeAttributes.explosionRadius);
    }
}
