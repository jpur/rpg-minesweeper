using UnityEngine;
using System.Collections;

public class DestroyAudioWhenDone : MonoBehaviour {
    private AudioSource _source;

    void Awake() {
        _source = GetComponent<AudioSource>();
    }

    void Update() {
        if (!_source.isPlaying) {
            Destroy(gameObject);
        }
    }
}
