using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] AudioMixer soundEffectsMixer;
    [SerializeField] AudioMixer musicMixer;

    [SerializeField] Slider soundEffectsSlider;
    [SerializeField] Slider musicSlider;

    private void Start()
    {
        //Set the sliders values to the last saved values
        musicSlider.value = PlayerPrefs.GetFloat("musicVol");
        soundEffectsSlider.value = PlayerPrefs.GetFloat("effectsVol");
        SetVolume(musicMixer, musicSlider);
        SetVolume(soundEffectsMixer, soundEffectsSlider);
    }
    public void ChangeMusicVolume()
    {
        SetVolume(musicMixer, musicSlider);
        PlayerPrefs.SetFloat("musicVol", musicSlider.value);
    }

    public void ChangeSoundEffectsVolume()
    {
        SetVolume(soundEffectsMixer, soundEffectsSlider);
        PlayerPrefs.SetFloat("effectsVol", soundEffectsSlider.value);
    }

    private void SetVolume(AudioMixer mixer, Slider slider)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(slider.value) * 20f);
    }
}
