using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private GameObject _introCanvas;
    
    private readonly AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    private readonly Dictionary<string, AudioClip> _audioClips = new();

    protected override void Awake()
    {
        base.Awake();
        Init();
        if (!_introCanvas.activeSelf)
        {
            Instance.Play("MainBackground", type: Define.Sound.Bgm, volume: 0.6f);
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

        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float volume = 1.0f, float pitch = 1.0f)
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

            audioSource.volume = volume;
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
            audioSource.volume = volume;
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