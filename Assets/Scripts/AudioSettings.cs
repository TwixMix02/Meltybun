using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider deathVolumeSlider;
    [SerializeField] Slider enemyVolumeSlider;

    string masterVolume = "MasterVolume";
    string sfxVolume = "SFXVolume";
    string musicVolume = "MusicVolume";

    string deathVolume = "DeathVolume";
    string enemyVolume = "EnemyVolume";

    void Start(){
        masterVolumeSlider.value = PlayerPrefs.GetFloat(masterVolume,0f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(sfxVolume, 0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolume, 0f);
        deathVolumeSlider.value = PlayerPrefs.GetFloat(deathVolume, 0f);
        enemyVolumeSlider.value = PlayerPrefs.GetFloat(enemyVolume, 0f);
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
        SetDeathVolume();
        SetEnemyVolume();
    }


    public void SetMasterVolume(){
        SetVolume(masterVolume, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(masterVolume, masterVolumeSlider.value);
    }
    public void SetSFXVolume()
    {
        SetVolume(sfxVolume, sfxVolumeSlider.value);
        PlayerPrefs.SetFloat(sfxVolume, sfxVolumeSlider.value);
    }
    public void SetMusicVolume()
    {
        SetVolume(musicVolume, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(musicVolume, musicVolumeSlider.value);
    }
    public void SetDeathVolume()
    {
        SetVolume(deathVolume, deathVolumeSlider.value);
        PlayerPrefs.SetFloat(deathVolume, deathVolumeSlider.value);
    }
    public void SetEnemyVolume()
    {
        SetVolume(enemyVolume, enemyVolumeSlider.value);
        PlayerPrefs.SetFloat(enemyVolume, enemyVolumeSlider.value);
    }

    void SetVolume(string groupName, float value){
        float adjustedVolume = Mathf.Log10(value) * 20;
        if(value == 0){
            adjustedVolume = -80;
        }
        audioMixer.SetFloat(groupName, adjustedVolume);
    }
}