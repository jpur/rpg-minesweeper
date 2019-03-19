using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : Entity {
    public Gradient LevelColor;
    public Text LevelLabel;

    public AudioClip[] Sounds;

    private const float MaxColorLevelDiff = 3; 
    private const float MinColorLevelDiff = -3;

    private SimpleFrameAnimator _sfa;

    void Awake() {
        _sfa = GetComponent<SimpleFrameAnimator>();
        gameObject.SetActive(false);
    }

    void Start() {
        LevelLabel.text = Level.ToString();
    }

    void UpdateColor() {
        int levelDiff = Level - Player.Instance.Level;
        LevelLabel.color = LevelColor.Evaluate((levelDiff - MinColorLevelDiff) / (MaxColorLevelDiff - MinColorLevelDiff));
    }

    public void OnReveal(bool playerClicked) {
        if (gameObject.activeSelf) return;
        Player.Instance.LeveledUp += UpdateColor;
        UpdateColor();
        gameObject.SetActive(true);
        if (playerClicked) Game.PlaySound(Sounds[Random.Range(0, Sounds.Length)]);
    }

    public void Remove() {
        Player.Instance.LeveledUp -= UpdateColor;
        Destroy(gameObject);
    }

    public void SetData(EnemyData data) {
        Level = data.Level;
        _sfa.Frames = data.Sprites;
    }
}
