using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class UILevel : MonoBehaviour {
    public Player Player;
    public Text ValueLabel;

    public float TweenTime;

    private void Start() {
        Player.LeveledUp += UpdateLevel;
        UpdateLevel();
        Messenger.AddHandler(Message.NewGame, () => ValueLabel.text = "LV1");
    }

    private void UpdateLevel() {
        ValueLabel.text = "LV" + Player.Level;
    }
}
