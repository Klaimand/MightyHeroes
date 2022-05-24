using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_SelfAudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;

    Dictionary<string, Sound> soundsKey = new Dictionary<string, Sound>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            AddSoundToDictionnary(sounds[i]);
        }
    }

    public void AddSoundToDictionnary(Sound _sound)
    {
        GameObject _go = new GameObject("Sound_" + _sound.name);
        _go.transform.parent = transform;
        _sound.SetSource(_go.AddComponent<AudioSource>());
        soundsKey.Add(_sound.name, _sound);
    }

    public void PlaySound(string _key)
    {
        soundsKey[_key].Play();
    }
}
