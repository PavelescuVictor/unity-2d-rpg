using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.Audio;

public class OptionMenu : GameMenu
{

    public Button closeMenu;
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown antiAliasingDropdown;
    public Dropdown vSyncDropdown;
    public Slider musicVolumeSlider;
    public Button applySettings;

    public AudioSource musicSource;
    public AudioMixer audioMixer;
    public Resolution[] resolutions;
    public GameSettings gameSettings;

    void OnEnable()
    {
        gameSettings = new GameSettings();
        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
        if (File.Exists(Application.persistentDataPath + "/gamesettings.json") == true)
        {
            LoadSettings();
        }
    }

    public void FullScreenToggle()
    {
        gameSettings.fullScreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void ResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void TextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
    }

    public void AntiAliasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antiAliasing = (int)Mathf.Pow(2f, antiAliasingDropdown.value);
    }

    public void VSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    public void MusicVolumeChange()
    {
        //musicSource.volume = gameSettings.musicVolume = musicVolumeSlider.value;
        float volume = gameSettings.musicVolume = musicVolumeSlider.value;
        audioMixer.SetFloat("MainMixerMusicVolume", volume);
    }

    public void ApplySettings()
    {
        SaveSettings();
    }
    
    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        //musicVolumeSlider.value = gameSettings.musicVolume;
        audioMixer.SetFloat("MainMixerMusicVolume", gameSettings.musicVolume);
        antiAliasingDropdown.value = gameSettings.antiAliasing;
        vSyncDropdown.value = gameSettings.vSync;
        textureQualityDropdown.value = gameSettings.textureQuality;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        fullscreenToggle.isOn = gameSettings.fullScreen;
        Screen.fullScreen = gameSettings.fullScreen;
        resolutionDropdown.RefreshShownValue();
    }
}
