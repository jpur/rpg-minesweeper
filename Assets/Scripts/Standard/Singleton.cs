using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance {
        get { return _inst ?? (_inst = FindObjectOfType<T>()); }
    }

    private static T _inst;
}
