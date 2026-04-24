using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip collectibleClip;
    [SerializeField] private float collectibleVolume = 1f;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private float gameOverVolume = 1f;
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private float buttonClickVolume = 1f;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Volumes")]
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private float musicVolume = 0.5f;

    [Header("SFX Pool")]
    [SerializeField] private int sfxPoolSize = 5;

    private List<AudioSource> sfxSources = new List<AudioSource>();
    private int currentSfxIndex;

    private AudioSource musicSource; // Will add audio source dynamically
    private bool hasGameOverPlayed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeAudio();
    }

    private void OnEnable()
    {
        GameEvents.OnCollectiblePicked += PlayCollectible;
        GameEvents.OnGameOver += PlayGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnCollectiblePicked -= PlayCollectible;
        GameEvents.OnGameOver -= PlayGameOver;
    }

    private void InitializeAudio()
    {
        // Music Source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        //musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        // SFX Pool
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxSources.Add(source);
        }

        PlayMusic();
    }

    private AudioSource GetNextSFXSource()
    {
        AudioSource source = sfxSources[currentSfxIndex];
        currentSfxIndex = (currentSfxIndex + 1) % sfxSources.Count;
        return source;
    }

    private void PlaySFX(AudioClip clip, float volume)
    {
        if (clip == null) return;

        AudioSource source = GetNextSFXSource();
        source.pitch = Random.Range(0.95f, 1.05f);
        float finalVolume = volume *sfxVolume;
        source.PlayOneShot(clip, finalVolume);
    }

    // ===== PUBLIC / EVENT METHODS =====

    public void PlayCollectible(int _)
    {
        PlaySFX(collectibleClip, collectibleVolume);
    }

    public void PlayGameOver()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();

        PlaySFX(gameOverClip, gameOverVolume);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickClip, buttonClickVolume);
    }

    public void PlayMusic()
    {
        if (backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }
}