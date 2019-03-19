using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour {
    public Button Button;
    public GameObject Menu;

    public GameObject Help;
    public Button HelpButton;

    public AudioSource Music;

    public Slider VolumeSlider;
    public Text VolumeLabel;

    public Slider SfxVolumeSlider;
    public Text SfxVolumeLabel;

    void Start() {
        Button.onClick.AddListener(() => Menu.SetActive(!Menu.activeSelf));
        Messenger.AddHandler(Message.NewGame, () => Menu.SetActive(false));

        VolumeSlider.onValueChanged.AddListener(MusicVolumeSliderChanged);
        SfxVolumeSlider.onValueChanged.AddListener(SfxVolumeSliderChanged);
        if (PlayerPrefs.HasKey("MusicVolume")) VolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.HasKey("SfxVolume")) SfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxVolume");

        Music.Play();
    }

    void MusicVolumeSliderChanged(float value) {
        PlayerPrefs.SetFloat("MusicVolume", value);
        Music.volume = value;
        VolumeLabel.text = ((int)(value * 100)).ToString();
    }

    void SfxVolumeSliderChanged(float value) {
        PlayerPrefs.SetFloat("SfxVolume", value);
        Game.SfxVolume = value;
        SfxVolumeLabel.text = ((int)(value * 100)).ToString();
    }
}
