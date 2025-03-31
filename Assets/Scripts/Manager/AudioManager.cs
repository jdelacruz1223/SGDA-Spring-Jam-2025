using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource _gameMusic;
    private bool _titleMusicPlaying = false;
    private bool _gameMusicPlaying = false;

    [Header("UI")]
    [SerializeField] private AudioSource _OpenSource;
    [SerializeField] private AudioSource _CloseSource;

    [Header("Shop")]
    [SerializeField] private AudioSource _BuySource;
    [SerializeField] private AudioSource _SellSource;

    [Header("Settings")]
    [SerializeField] private float uiAudioVolume = 0.8f;
    [SerializeField] private float uiAudioPitch = 1.0f;

    [Header("Gameplay")]
    [SerializeField] private AudioSource _PlantSource;
    [SerializeField] private AudioSource _HarvestSource;

    private Queue<AudioSource> audioQueue = new Queue<AudioSource>();
    private float lastPlayTime = 0f;
    private const float MIN_PLAY_INTERVAL = 0.05f;

    public static AudioManager GetInstance() { return me; }
    public static AudioManager me;

    void Awake()
    {
        if (me != null)
        {
            Destroy(gameObject);
            return;
        }

        me = this;

        // Initialize audio sources
        if (_OpenSource != null)
        {
            _OpenSource.playOnAwake = false;
            _OpenSource.volume = uiAudioVolume;
            _OpenSource.pitch = uiAudioPitch;
        }

        if (_CloseSource != null)
        {
            _CloseSource.playOnAwake = false;
            _CloseSource.volume = uiAudioVolume;
            _CloseSource.pitch = uiAudioPitch;
        }

        if (_PlantSource != null)
        {
            _PlantSource.playOnAwake = false;
        }
    }
    void Update()
    {
        if (audioQueue.Count > 0 && Time.realtimeSinceStartup - lastPlayTime >= MIN_PLAY_INTERVAL)
        {
            PlayQueuedAudio();
        }
    }

    public void PlayHarvestSound()
    {
        if (_HarvestSource == null) return;

        _HarvestSource.Stop();
        _HarvestSource.Play();
    }

    private void PlayQueuedAudio()
    {
        if (audioQueue.Count > 0)
        {
            AudioSource source = audioQueue.Dequeue();
            if (source != null)
            {
                source.Play();
                lastPlayTime = Time.realtimeSinceStartup;
            }
        }
    }

    public void PlayPanelEffect(bool status)
    {
        AudioSource sourceToPlay = status ? _OpenSource : _CloseSource;

        if (sourceToPlay == null) return;

        sourceToPlay.Stop();

        sourceToPlay.time = 0.05f;

        if (Time.realtimeSinceStartup - lastPlayTime < MIN_PLAY_INTERVAL)
        {
            audioQueue = new Queue<AudioSource>();
            audioQueue.Enqueue(sourceToPlay);
        }
        else
        {
            sourceToPlay.Play();
            lastPlayTime = Time.realtimeSinceStartup;
        }
    }

    public void PlayPlantSound()
    {
        if (_PlantSource == null) return;

        _PlantSource.Stop();
        _PlantSource.Play();
    }

    public void PlayBuySound()
    {
        if (_BuySource == null) return;

        _BuySource.Stop();
        _BuySource.Play();
    }

    public void PlaySellSound()
    {
        if (_SellSource == null) return;

        _SellSource.Stop();
        _SellSource.Play();
    }
}
