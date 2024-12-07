using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Dropdown resolutionDropdown;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider deathVolumeSlider;
    public Slider enemyVolumeSlider;
    public Toggle vsyncToggle;
    public Toggle fullscreenToggle;
    public GameObject optionsPanel;
    public GameObject keybindingPanel;
    public TMP_Text moveLeftText;
    public TMP_Text moveRightText;
    public TMP_Text jumpText;

    private Resolution[] resolutions;

    void Start()
    {
        // Populate resolution options
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        var resolutionOptions = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Load saved settings
        LoadSettings();

        // Update keybinding texts
        UpdateKeyBindingTexts();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }



    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("DeathVolume", deathVolumeSlider.value);
        PlayerPrefs.SetFloat("EnemyVolume", enemyVolumeSlider.value);
        PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Resolution"))
        {
            int savedResolutionIndex = PlayerPrefs.GetInt("Resolution");
            resolutionDropdown.value = savedResolutionIndex;
            SetResolution(savedResolutionIndex);
        }

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedMasterVolume = PlayerPrefs.GetFloat("MasterVolume");
            masterVolumeSlider.value = savedMasterVolume;
            SetMasterVolume(savedMasterVolume);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume");
            sfxVolumeSlider.value = savedSFXVolume;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            musicVolumeSlider.value = savedMusicVolume;
        }

        if (PlayerPrefs.HasKey("DeathVolume"))
        {
            float savedDeathVolume = PlayerPrefs.GetFloat("DeathVolume");
            deathVolumeSlider.value = savedDeathVolume;
        }

        if (PlayerPrefs.HasKey("EnemyVolume"))
        {
            float savedEnemyVolume = PlayerPrefs.GetFloat("EnemyVolume");
            enemyVolumeSlider.value = savedEnemyVolume;
        }

        if (PlayerPrefs.HasKey("VSync"))
        {
            bool savedVSync = PlayerPrefs.GetInt("VSync") == 1;
            vsyncToggle.isOn = savedVSync;
            SetVSync(savedVSync);
        }

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1;
            fullscreenToggle.isOn = savedFullscreen;
            SetFullscreen(savedFullscreen);
        }
    }
    // Opens keybinding panel and starts the keybinding process
    public void openKeybindings()
    {
        keybindingPanel.SetActive(true);
        UpdateKeyBindingTexts();
    }

    // Update the keybinding texts with the current key bindings
    public void UpdateKeyBindingTexts()
    {
        moveLeftText.text = KeybindingManager.Instance.GetPrimaryKey("MoveLeft") + " / " + KeybindingManager.Instance.GetSecondaryKey("MoveLeft");
        moveRightText.text = KeybindingManager.Instance.GetPrimaryKey("MoveRight") + " / " + KeybindingManager.Instance.GetSecondaryKey("MoveRight");
        jumpText.text = KeybindingManager.Instance.GetPrimaryKey("Jump") + " / " + KeybindingManager.Instance.GetSecondaryKey("Jump");
    }

    // Closes the keybinding panel
    public void closeKeybinds()
    {
        if (keybindingPanel != null)
        {
            keybindingPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("keybindingPanel reference not set!");
        }
    }

    public void closeOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("OptionsPanel reference not set!");
        }
    }
}
