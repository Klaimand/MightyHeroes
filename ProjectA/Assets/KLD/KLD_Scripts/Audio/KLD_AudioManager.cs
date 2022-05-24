using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;
    public AudioMixerGroup group;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [SerializeField] bool canBeOverriden = false;

    [FoldoutGroup("Random")]
    [Range(0f, 0.5f)]
    public float randomVolume = 0f;
    [FoldoutGroup("Random")]
    [Range(0f, 0.5f)]
    public float randomPitch = 0f;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        if (clips.Length == 1)
        {
            source.clip = clips[0];
        }
        else
        {
            source.clip = clips[Random.Range(0, clips.Length)];
        }
        source.outputAudioMixerGroup = group;
    }

    public AudioSource GetSource()
    {
        return source;
    }

    public void Play()
    {
        if (!source.isPlaying || (source.isPlaying && canBeOverriden))
        {
            if (clips.Length > 1)
            {
                source.clip = clips[Random.Range(0, clips.Length)];
            }
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
            source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
            source.Play();
        }
    }

    public void Stop()
    {
        source.Stop();
    }

}


public class KLD_AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    [SerializeField] KLD_VoiceLinesSO[] voiceLines;

    [SerializeField] Sound[] sounds;

    Dictionary<string, Sound> soundsKey = new Dictionary<string, Sound>();

    [SerializeField]
    string[] soundsToPlayOnStart;

    public static KLD_AudioManager Instance = null;

    #region Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    Sound[] soundsToAdd;

    private void Start()
    {

        for (int i = 0; i < sounds.Length; i++)
        {
            AddSoundToDictionnary(sounds[i], "");
        }

        for (int i = 0; i < voiceLines.Length; i++)
        {
            soundsToAdd = voiceLines[i].GetCharacterSounds();
            for (int y = 0; y < soundsToAdd.Length; y++)
            {
                AddSoundToDictionnary(soundsToAdd[y], voiceLines[i].prefix);
            }
        }

        //GetSound("musique1").GetSource().loop = true;
        //GetSound("musique2").GetSource().loop = true;

        foreach (string sound in soundsToPlayOnStart)
        {
            PlaySound(sound);
        }
    }

    public void AddSoundToDictionnary(Sound _sound, string _prefix)
    {
        GameObject _go = new GameObject("Sound_" + _prefix + _sound.name);
        _go.transform.parent = transform;
        _sound.SetSource(_go.AddComponent<AudioSource>());
        soundsKey.Add(_prefix + _sound.name, _sound);
    }

    public void PlaySound(string _key)
    {
        soundsKey[_key].Play();
    }

    public void PlayCharacterSound(string _key, int _index)
    {
        int _priority = GetCurCharacterPriority(_index);

        if (!isVoicing || (isVoicing && _priority > curVoicePriority))
        {
            if (isVoicing)
            {
                GetSound(lastVoiceLineKey).Stop();
                if (curVoiceLineCoroutine != null)
                {
                    StopCoroutine(curVoiceLineCoroutine);
                }
            }
            curVoicePriority = _priority;

            lastVoiceLineKey = KLD_CharacterInfos.instance.GetCharacterSoundPrefix() + _key;
            soundsKey[lastVoiceLineKey].Play();

            isVoicing = true;
            curVoiceLineCoroutine = StartCoroutine(PlayVoiceLineCoroutine(
                GetSound(lastVoiceLineKey).GetSource().clip.length
            ));
        }
    }

    bool isVoicing = false;
    int curVoicePriority = 0;
    string lastVoiceLineKey;
    Coroutine curVoiceLineCoroutine;

    IEnumerator PlayVoiceLineCoroutine(float _lenght)
    {
        yield return new WaitForSeconds(_lenght);
        isVoicing = false;
    }

    public int GetCurCharacterPriority(int _index)
    {
        return voiceLines[KLD_CharacterInfos.instance.GetCharacterSoundIndex()].priorities[_index];
    }

    #region Obsolete
    /*
    bool foundsmthng = false;
    foreach (Sound sound in sounds)
    {
        if (sound.name == _name)
        {
            sound.Play();
            foundsmthng = true;
            //return;
        }
    }
    if (!foundsmthng)
        Debug.LogWarning("No found sound '" + _name + "'");
        */
    #endregion

    public Sound GetSound(string _key)
    {
        return soundsKey[_key];
    }

    public void FadeOutInst(AudioSource _source, float time)
    {
        StartCoroutine(FadeOut(_source, time));
    }

    IEnumerator FadeOut(AudioSource _source, float time)
    {
        float curTime = 0f;
        float startVolume = _source.volume;

        while (curTime < time)
        {
            _source.volume = Mathf.Lerp(startVolume, 0f, curTime / time);
            curTime += Time.deltaTime;
            yield return null;
        }
        _source.volume = 0f;

        _source.Stop();
    }

    public void SetReverb(bool value)
    {
        mixer.SetFloat("ReverbMix", value ? 0f : -80f);
    }
}