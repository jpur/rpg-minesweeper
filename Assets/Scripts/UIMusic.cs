using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMusic : MonoBehaviour, IPointerClickHandler {
    public Sprite EnabledSprite;
    public Sprite DisabledSprite;

    private Image _image;

    private bool _enabled;

    void Awake() {
        _image = GetComponent<Image>();
        if (PlayerPrefs.HasKey("Mute")) {
            SetEnabled(PlayerPrefs.GetInt("Mute") == 1);
        } else {
            SetEnabled(true);
        }
    }

    public void SetEnabled(bool state) {
        _enabled = state;
        AudioListener.volume = state ? 0.3f : 0;
        _image.sprite = _enabled ? EnabledSprite : DisabledSprite;
        PlayerPrefs.SetInt("Mute", state ? 1 : 0);
    }

    public void OnPointerClick(PointerEventData eventData) {
        SetEnabled(!_enabled);
    }
}
