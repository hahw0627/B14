using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();


    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        GameObject root = GameObject.Find("Sound");
        if (root == null)
        {
            root = new GameObject { name = "Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundName = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundName.Length - 1; i++)
            {
                var go = new GameObject { name = soundName[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
            _audioSources[(int)Define.Sound.Bgm].loop = true;

        }
    }

    public void Soundw()
    {
        
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            if (audioClip == null)
            {
                Debug.Log("missing");
                return;
            }
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioClip audioClip = GetOrAddAudioClip(path);
            if (audioClip == null)
            {
                Debug.Log("missing");
                return;
            }
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }
    public void Clear()
    {
        foreach (var audioSouce in _audioSources)
        {
            audioSouce.clip = null;
            audioSouce.Stop();
        }
        audioClips.Clear();
    }

    AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (audioClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = Resources.Load<AudioClip>(path);
            audioClips.Add(path, audioClip);
        }

        return audioClip;
    }
}
