using UnityEngine;
using System.Collections;
using DG.Tweening;

public class UILevelUp : MonoBehaviour {
    public RectTransform Anchor;
    public AudioClip[] Sounds;

    private Vector3 _startPos;

    private CanvasGroup _group;

    void Awake() {
        _group = GetComponent<CanvasGroup>();
        _group.alpha = 0;
    }

    void Start() {
        _startPos = transform.position;

        Player.Instance.LeveledUp += Animate;
    }

    void Animate() {
        _group.alpha = 1;
        transform.position = _startPos;
        transform.DOMove(Anchor.position, 1.5f, true);
        DOTween.To(() => _group.alpha, a => _group.alpha = a, 0, 3);
        Game.PlaySound(Sounds[Random.Range(0, Sounds.Length)]);
    }
}
