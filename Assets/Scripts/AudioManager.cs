using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider; // Slider for master volume
    [SerializeField] private Slider musicVolumeSlider;  // Slider for music volume
    [SerializeField] private Slider soundsVolumeSlider; // Slider for sounds volume

    [SerializeField] private AudioSource musicSource;    // Assign your music audio source here
    [SerializeField] private AudioSource birdsSource;    // Assign your birds audio source here
    [SerializeField] private AudioSource soundsSource;   // Assign your general sounds audio source here

    private const string MasterVolumeKey = "masterVolume";
    private const string MusicVolumeKey = "musicVolume";
    private const string SoundsVolumeKey = "soundsVolume";

    public AudioClip mainMenuMusic;
    public AudioClip backgroundMusic;
    public AudioClip buttonClick;
    public AudioClip birdsSing;
    public AudioClip cicadas;
    public AudioClip lose;
    public AudioClip win;
    public AudioClip winApplause;
    public AudioClip egg;
    public AudioClip digging;
    public AudioClip fenceCreak;
    public AudioClip walkGrass;
    public AudioClip walkGround;
    public AudioClip walkWood;

    private float musicVolume;

    void Start()
    {
        LoadAndApplyVolumeSettings();
        PlayMusic(backgroundMusic);
        PlayBirds(birdsSing);
    }

    private void Awake()
    {
        LoadAndApplyVolumeSettings();
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        //DontDestroyOnLoad(gameObject); // Make this object persistent across scenes
    }

    private void LoadAndApplyVolumeSettings()
    {
        // Load volume settings from PlayerPrefs
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.1f);
        float soundsVolume = PlayerPrefs.GetFloat(SoundsVolumeKey, 0.75f);
        float masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);

        // Apply volume settings to AudioSources and AudioListener
        musicSource.volume = musicVolume;
        soundsSource.volume = soundsVolume;
        AudioListener.volume = masterVolume;
    }



    private void Update()
    {

        SwitchTime();
    }
    void SwitchTime()
    {
        switch (TimerClock.Hours())
        {
            case 4:
                PlayBirds(birdsSing);
                break;
            case 7:
                if (musicSource.volume == 0)
                    StartCoroutine(RaiseAudioSource(musicSource, TimerClock.dayLength * 60 / 24 - 1, musicVolume));
                break;
            case 22:
                PlayBirds(cicadas);
                StartCoroutine(FadeOutAudioSource(musicSource, TimerClock.dayLength * 60 / 24 - 2));
                break;
        }
    }
    IEnumerator FadeOutAudioSource(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float volume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volume, 0, currentTime / duration);
            yield return null;
        }
    }
    IEnumerator RaiseAudioSource(AudioSource audioSource, float duration, float resultVolume)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, resultVolume, currentTime / duration);
            yield return null;
        }
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume); // Save volume
    }

    // Call this method from your birds volume slider
    public void SetBirdsVolume(float volume)
    {
        birdsSource.volume = volume;
        PlayerPrefs.SetFloat("BirdsVolume", volume); // Save volume
    }

    // Call this method from your sounds volume slider
    public void SetSoundsVolume(float volume)
    {
        soundsSource.volume = volume;
        PlayerPrefs.SetFloat("SoundsVolume", volume); // Save volume
    }
    public void PlaySound(AudioClip sound)
    {
        soundsSource.PlayOneShot(sound);
    }
    public void PlayMusic(AudioClip sound)
    {
        musicSource.Stop();
        musicSource.clip = sound;
        musicSource.Play();
    }
    public void PlayBirds(AudioClip sound)
    {
        birdsSource.Stop();
        birdsSource.clip = sound;
        birdsSource.Play();
    }
    public void StopAllMusic()
    {
        musicSource.Stop();
        soundsSource.Stop();
        birdsSource.Stop();
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadAndApplyVolumeSettings(); // Reapply volume settings every time a scene is loaded
    }

    public void ChangeMasterVolume()
    {
        AudioListener.volume = masterVolumeSlider.value;
        SaveVolumeSetting(MasterVolumeKey, masterVolumeSlider.value);
    }

    public void ChangeMusicVolume()
    {
        musicSource.volume = musicVolumeSlider.value;
        SaveVolumeSetting(MusicVolumeKey, musicVolumeSlider.value);
    }

    public void ChangeSoundsVolume()
    {
        // Assuming you want the same volume for birds and general sounds
        birdsSource.volume = soundsVolumeSlider.value;
        soundsSource.volume = soundsVolumeSlider.value;
        SaveVolumeSetting(SoundsVolumeKey, soundsVolumeSlider.value);
    }

    private void LoadVolumeSetting(string key, Slider slider, float defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetFloat(key, defaultValue);
        }
        slider.value = PlayerPrefs.GetFloat(key);
    }

    private void SaveVolumeSetting(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    private void ApplyVolumeSettings()
    {
        // Update the AudioListener and AudioSources with the saved values
        ChangeMasterVolume();
        ChangeMusicVolume();
        ChangeSoundsVolume();
    }
}
