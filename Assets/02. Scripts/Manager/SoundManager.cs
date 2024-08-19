using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioMixerGroup mixerGroupA; //bgm
    [SerializeField]
    private AudioMixerGroup mixerGroupB; //effect

    [SerializeField]
    private AudioMixer audioMixer;

    private readonly AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    private readonly Dictionary<string, AudioClip> _audioClips = new();

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public AudioSource GetAudioSource(Define.Sound type)
    {
        if (type == Define.Sound.Bgm)
            return _audioSources[(int)Define.Sound.Bgm];
        else if (type == Define.Sound.Effect)
            return _audioSources[(int)Define.Sound.Effect];

        return null;
    }

    public void PauseBGM()
    {
        _audioSources[(int)Define.Sound.Bgm].Pause();
    }
    public void UnPauseBGM()
    {
        _audioSources[(int)Define.Sound.Bgm].UnPause();
    }
    public void SetVolume(Define.Sound type, float volume)
    {

        if (volume < 0.1)
        {
            audioMixer.SetFloat(type.ToString(), -80f); //º¼·ýÀ» ¿ÏÀüÈ÷ ²û
        }
        else
        { 
            audioMixer.SetFloat(type.ToString(), Mathf.Log10(volume) * 20);
        }

    }

    private void Init()
    {
        var root = GameObject.Find("Sound");
        if (root != null) return;
        root = new GameObject { name = "Sound" };
        DontDestroyOnLoad(root);

        var soundName = System.Enum.GetNames(typeof(Define.Sound));
        for (var i = 0; i < soundName.Length - 1; i++)
        {
            var go = new GameObject { name = soundName[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;

        }
        _audioSources[(int)Define.Sound.Bgm].outputAudioMixerGroup = mixerGroupA;
        _audioSources[(int)Define.Sound.Effect].outputAudioMixerGroup = mixerGroupB;
        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        if (type == Define.Sound.Bgm)
        {
            var audioClip = Resources.Load<AudioClip>(path);
            if (audioClip is null)
            {
                Debug.Log("missing");
                return;
            }

            var audioSource = _audioSources[(int)Define.Sound.Bgm];

            if (audioSource.isPlaying)
                audioSource.Stop();


            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            var audioClip = GetOrAddAudioClip(path);
            if (audioClip == null)
            {
                Debug.Log("missing");
                return;
            }

            var audioSource = _audioSources[(int)Define.Sound.Effect];

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void StopBGM()
    {
        _audioSources[(int)Define.Sound.Bgm].Stop();
    }

    public void PlaySkillSound(AudioClip skillSound)
    {
        if (skillSound is null) return;

        var audioSource = _audioSources[(int)Define.Sound.Effect];
        audioSource.PlayOneShot(skillSound);
    }

    public void Clear()
    {
        foreach (var audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClips.Clear();
    }

    private AudioClip GetOrAddAudioClip(string path)
    {
        if (_audioClips.TryGetValue(path, out var audioClip)) return audioClip;
        audioClip = Resources.Load<AudioClip>(path);
        _audioClips.Add(path, audioClip);

        return audioClip;
    }

}