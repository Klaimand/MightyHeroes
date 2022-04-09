using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KLD_PlayerShoot : MonoBehaviour
{
    //private references
    KLD_PlayerAim playerAim;

    [Header("Public References")]
    [SerializeField] Transform canon;

    [Header("Weapon"), Space(10)]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [SerializeField] KLD_WeaponSO weapon;

    [Header("Shooting Parameters"), Space(10)]
    [SerializeField] float zombieVerticalOffset = 1.5f;
    [SerializeField] LayerMask layerMask;

    //private local fields
    Vector3 impactPosition = Vector3.zero;
    Vector3 shootDirection = Vector3.zero;
    Vector3 selectedZombiePos = Vector3.zero;

    [Header("Debug"), Space(10)]
    [SerializeField] float debugShootDelay = 0.3f;
    float curDebugShootDelay = 0f;
    [SerializeField] Color raysColor;

    void Awake()
    {
        playerAim = GetComponent<KLD_PlayerAim>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerAim.isShooting && curDebugShootDelay > debugShootDelay)
        {
            if (playerAim.GetSelectedZombie() != null)
            {
                selectedZombiePos = playerAim.GetSelectedZombie().transform.position;

                selectedZombiePos.y += zombieVerticalOffset;

                shootDirection = selectedZombiePos - canon.position;
            }
            else
            {
                shootDirection = canon.forward;
            }

            //Debug.DrawRay(canon.position, shootDirection, raysColor, debugShootDelay);

            impactPosition = weapon.bullet.Shoot(weapon, canon.position, shootDirection, layerMask);
            Debug.DrawLine(canon.position, impactPosition, raysColor, debugShootDelay);
            curDebugShootDelay = 0f;
        }

        curDebugShootDelay += Time.deltaTime;
    }
}
