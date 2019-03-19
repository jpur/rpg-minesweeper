using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerClickHandler {
    public bool IsOccupied { get { return Occupant != null; } }
    public bool IsRevealed { get; private set; }
    public IVector2 Position { get; set; }

    public AudioClip[] Sounds;

    public Enemy Occupant
    {
        get { return _occupant;}
        set
        {
            if (_occupant != null) {
                Destroy(_occupant.gameObject);
            }
            _occupant = value;
            if (_occupant != null) {
                _occupant.transform.SetParent(transform, false);
            }
        }
    }

    public Text Label;

    public Color RevealedColor;
    public Color UnrevealedColor;

    private RectTransform _trans;
    private Enemy _occupant;
    private Image _image; 

    void Awake() {
        _trans = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _image.color = UnrevealedColor;
    }

    public void SetSize(Vector2 size) {
        _trans.sizeDelta = size;
    }

    public void Reveal(bool playerClicked) {
        if (IsRevealed) return;
        _image.color = RevealedColor;
        Player.Instance.OnReveal(this, playerClicked);
        IsRevealed = true;
        if (playerClicked) Game.PlaySound(Sounds[Random.Range(0, Sounds.Length)]);
    }

    public void SetNearbyCount(int count) {
        Label.text = count.ToString();
        Label.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Reveal(true);
    }

    public void Remove() {
        if (Occupant != null) {
            Occupant.Remove();
        }
        Destroy(gameObject);
    }
}
