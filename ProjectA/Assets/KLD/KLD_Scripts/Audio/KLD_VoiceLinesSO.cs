using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newVoiceLinesSO", menuName = "KLD/Audio/VoiceLinesSO", order = 0)]
public class KLD_VoiceLinesSO : ScriptableObject
{
    public string prefix = "Sayuri_";

    public Sound kill;

    public Sound reload;

    public Sound activeAbility;

    public Sound captureZone;

    public Sound intelPickup;

    public Sound takeDamage;

    public Sound healing;

    public Sound death;

    public Sound victory;

    public Sound pickCharacter;

    public Sound[] GetCharacterSounds()
    {
        Sound[] sounds = new Sound[10];

        sounds[0] = kill;
        sounds[1] = reload;
        sounds[2] = activeAbility;
        sounds[3] = captureZone;
        sounds[4] = intelPickup;
        sounds[5] = takeDamage;
        sounds[6] = healing;
        sounds[7] = death;
        sounds[8] = victory;
        sounds[9] = pickCharacter;

        return sounds;
    }

    [Header("Priorities")]
    public int[] priorities;
}