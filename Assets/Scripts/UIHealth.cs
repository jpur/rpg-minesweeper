using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour {
    public PlayerHealth Health;
    public Image FillImage;
    public Text ValueLabel;

    public float TweenTime;

    void Start() {
        Health.HealthChanged += UpdateHealth;
        UpdateHealth();
    }

    void UpdateHealth() {
        DOTween.To(() => FillImage.fillAmount, f => FillImage.fillAmount = f, (float)Health.CurrentHealth / Health.MaxHealth, TweenTime);
        ValueLabel.text = Health.CurrentHealth + "/" + Health.MaxHealth;
    }
}
