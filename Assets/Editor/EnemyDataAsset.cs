using UnityEngine;
using System.Collections;
using UnityEditor;

public class EnemyDataAsset {
    [MenuItem("Assets/Create/YourClass")]
    public static void CreateAsset() {
        ScriptableObjectUtility.CreateAsset<EnemyData>();
    }
}
