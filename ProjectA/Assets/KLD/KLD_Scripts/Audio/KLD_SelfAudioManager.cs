using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_SelfAudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;

    Dictionary<string, Sound> soundsKey = new Dictionary<string, Sound>();

    [SerializeField] string[] soundsToLoop;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            AddSoundToDictionnary(sounds[i]);
        }

        for (int i = 0; i < soundsToLoop.Length; i++)
        {
            GetSound(soundsToLoop[i]).GetSource().loop = true;
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

    public void PlaySound(string _key, float _delay)
    {
        StartCoroutine(PlaySoundCoroutine(_key, _delay));
    }

    IEnumerator PlaySoundCoroutine(string _key, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        PlaySound(_key);
    }

    public Sound GetSound(string _key)
    {
        return soundsKey[_key];
    }
}
