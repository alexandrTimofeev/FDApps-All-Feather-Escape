using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public string mainSceneName = "Game";
    public GameObject menuPanel;
    public GameObject optionsPanel;
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider masterVolumeSlider;
    public AudioManager audioManager;

    private void Start()
    {
        audioManager.StopAllMusic();
        audioManager.PlayMusic(audioManager.mainMenuMusic);
        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        // Initialize the volume sliders' values to the current volumes
        //float musicVolume, effectsVolume, masterVolume;
        //audioMixer.GetFloat("MusicVolume", out musicVolume);
        //audioMixer.GetFloat("EffectsVolume", out effectsVolume);
        //audioMixer.GetFloat("MasterVolume", out masterVolume);
        //if (musicVolumeSlider != null)
            //musicVolumeSlider.value = musicVolume;
        //if (effectsVolumeSlider != null)
            //effectsVolumeSlider.value = effectsVolume;
        //if (masterVolumeSlider != null)
            //masterVolumeSlider.value = masterVolume;
    }

    public void PlayGame()
    {
        audioManager.PlaySound(audioManager.buttonClick);

        int isWon = PlayerPrefs.GetInt("GameWon", 0);

        if (isWon == 1)
        {
            PlayerPrefs.SetInt("GameWon", 0); // сбрасываем
            SceneManager.LoadScene(mainSceneName); // перезагружаем сцену заново
        }
        else
        {
            SceneManager.LoadScene(mainSceneName); // просто загружаем
        }
    }

    public void ToggleOptions()
    {
        audioManager.PlaySound(audioManager.buttonClick);
        if (menuPanel != null && optionsPanel != null)
        {
            bool isOptionsActive = optionsPanel.activeSelf;
            menuPanel.SetActive(isOptionsActive);
            optionsPanel.SetActive(!isOptionsActive);
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioManager.SetMusicVolume(volume);
    }

    public void SetEffectsVolume(float volume)
    {
        audioManager.SetSoundsVolume(volume);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void ExitGame()
    {
        audioManager.PlaySound(audioManager.buttonClick);
        Application.Quit();
    }

    public void MenuButtonClicked()
    {
        audioManager.PlaySound(audioManager.buttonClick);
        if (menuPanel != null && optionsPanel != null)
        {
            optionsPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
    }
}
