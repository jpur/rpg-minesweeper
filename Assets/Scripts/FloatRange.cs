using UnityEngine;
using System.Collections;

[System.Serializable]
public struct FloatRange {
    public float Min;
    public float Max;

    public float GetRandom() {
        return Random.Range(Min, Max);
    }
}
