using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent((typeof(Image)))]
public class SimpleFrameAnimator : MonoBehaviour, ISpriteAnimator {
    public Sprite[] Frames;
    public float FrameTime = 1.0f;
    public bool RandomizeStartTime;

    private Image _renderer;

    private int _currentFrame;
    private float _currentFrameTime;

    public Sprite GetNextSprite() {
        _currentFrame++;
        if (_currentFrame >= Frames.Length) {
            _currentFrame = 0;
        }
        return Frames[_currentFrame];
    }

    public void SetSprite(Sprite sprite) {
        _renderer.sprite = sprite;
    }

    void Awake() {
        _renderer = GetComponent<Image>();
    }

    void Start() {
        _renderer.sprite = Frames[0];
        if (RandomizeStartTime) {
            _currentFrameTime = Random.value * FrameTime;
        }
    }

    void Update() {
        if (Frames.Length < 2) return;

        _currentFrameTime += Time.deltaTime;
        if (_currentFrameTime > FrameTime) {
            _currentFrameTime -= FrameTime;
            SetSprite(GetNextSprite());
        }
    }
}
