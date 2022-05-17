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

    Transform impactZoneTransform;
    bool isAiming = false;
    Vector3[] impactPoints;
    Vector3 impactPos;
    Vector3 impactZoneWorldDir;

    public override void Initialize(int characterLevel)
    {
        grenadeAttributes.level = characterLevel;
        grenadeAttributes.Initialize();
    }

    public override void ActivateSpell(Vector3 direction, Transform pos, int characterLevel)
    {
        instGrenade = XL_Pooler.instance.PopPosition("BlastGrenade", pos.position + pos.forward + pos.up);
        startingVelocity = XL_Utilities.GetVelocity(0.5f, Mathf.Lerp(grenadeAttributes.minThrowingDistance, grenadeAttributes.throwingDistance, direction.magnitude), grenadeAttributes.travelTime);
        grenadeVel.x = startingVelocity[0] * (direction.x);
        grenadeVel.y = startingVelocity[1];
        grenadeVel.z = startingVelocity[0] * (direction.z);
        instGrenade.GetComponent<Rigidbody>().velocity = grenadeVel;
        //grenade.GetComponent<Rigidbody>().velocity = new Vector3(startingVelocity[0] * (throwingDirection.x), startingVelocity[1], startingVelocity[0] * (throwingDirection.z));
        instGrenade.GetComponent<XL_Grenade>().SetValue(grenadeAttributes.explosionDamage, grenadeAttributes.explosionRadius);

        DepopImpactZone();
    }

    public override void OnUltJoystickDown(Vector2 _joyDirection, Transform _player)
    {

        if (!isAiming)
        {
            isAiming = true;
            impactZoneTransform = XL_Pooler.instance.Pop("BlastGrenadeZone").transform;
        }

        impactZoneWorldDir.x = _joyDirection.x;
        impactZoneWorldDir.y = 0f;
        impactZoneWorldDir.z = _joyDirection.y;

        impactZoneWorldDir = Quaternion.Euler(0f, 45f, 0f) * impactZoneWorldDir;

        //impactPoints = XL_Utilities.GenerateCurve(2, grenadeAttributes.throwingDistance - 1);

        impactPos = _player.position + impactZoneWorldDir.normalized +
        impactZoneWorldDir * Mathf.Lerp(grenadeAttributes.minThrowingDistance,
        grenadeAttributes.throwingDistance, impactZoneWorldDir.magnitude);

        impactZoneTransform.position = impactPos;
    }

    public override void OnSpellLaunch()
    {
        DepopImpactZone();
    }

    void DepopImpactZone()
    {
        isAiming = false;
        XL_Pooler.instance.DePop("BlastGrenadeZone", impactZoneTransform.gameObject);
    }
}
