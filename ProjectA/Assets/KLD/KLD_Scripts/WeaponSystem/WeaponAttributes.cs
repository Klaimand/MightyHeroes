using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WeaponAttributes
{
    [LabelText("Level Price")]
    public int experienceToReach = 0;

    public int bulletDamage = 20;

    public int rpm = 450;

    [Header("Magazine & Reloading")]
    [LabelText("$reloadSpeedLabel")]
    public float reloadSpeed = 1.5f;

    public int magazineSize = 30;

    [Header("Shot Parameters")]
    [Range(1, 10)]
    public int bulletsPerShot = 1;

    [HideIf("bulletsPerShot", 1)]
    public float timeBetweenBullets = 0.07f;

    [ReadOnly, HideIf("bulletsPerShot", 1)]
    public bool isBuckshot = false;



    [LabelText("$spreadLabel"), Range(0, 90)]
    public int spread = 25;

    public float range = 15f;

    [Space(10)]
    public int activePointsPerKill = 5;

    [HideInInspector] public string displayedName = "";
    [HideInInspector] public bool isBPBReloading = false;
    string spreadLabel = "spread";
    string reloadSpeedLabel = "";
    public void OnValidate()
    {
        ProtectNonNegativeValues();

        isBuckshot = bulletsPerShot > 1 && timeBetweenBullets == 0;

        spreadLabel = isBuckshot ? "spread (buckshot)" : "spread";

        reloadSpeedLabel = isBPBReloading ? "Reload Speed (1 bullet)" : "Reload Speed";
    }

    void ProtectNonNegativeValues()
    {
        if (experienceToReach < 0) experienceToReach = 0;
        if (bulletDamage < 0) bulletDamage = 0;
        if (rpm < 0) rpm = 0;
        if (magazineSize < 0) magazineSize = 0;
        if (activePointsPerKill < 0) activePointsPerKill = 0;

        if (timeBetweenBullets < 0f) timeBetweenBullets = 0f;
        if (reloadSpeed < 0f) reloadSpeed = 0f;
        if (range < 0f) range = 0f;
    }
}