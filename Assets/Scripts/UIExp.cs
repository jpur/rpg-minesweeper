using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class UIExp : MonoBehaviour {
    public Player Player;
    public Image FillImage;
    public Text ValueLabel;

    public float TweenTime;

    void Start() {
        Player.ExpChanged += UpdateExpChanged;
        UpdateExpChanged();
    }

    void UpdateExpChanged() {
        DOTween.To(() => FillImage.fillAmount, f => FillImage.fillAmount = f, (float)Player.Experience / Player.ExperienceToLevel, TweenTime);
        ValueLabel.text = Player.Experience + "/" + Player.ExperienceToLevel;
    }
}
