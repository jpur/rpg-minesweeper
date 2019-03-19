using UnityEngine;
using System.Collections;

public interface ISpriteAnimator {
    Sprite GetNextSprite();
    void SetSprite(Sprite sprite);
}
