using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool instance {
        get { return inst ?? (inst = FindObjectOfType<ObjectPool>()); }
    }

    private static ObjectPool inst;

    public PrePool[] prePools;

    private readonly Dictionary<string, Stack<GameObject>> pool = new Dictionary<string, Stack<GameObject>>();

    void Awake() {
        foreach (PrePool pp in prePools) {
            for (int i = 0; i < pp.count; i++) {
                GameObject obj = (GameObject)Instantiate(pp.obj);
                obj.name = pp.obj.name;
                Despawn(obj);
            }
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation) {
        string pName = prefab.name;
        if (pool.ContainsKey(pName) && pool[pName].Count > 0) {
            GameObject obj = Recycle(pName);
            obj.transform.parent = null;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }
        else {
            GameObject obj = (GameObject)Instantiate(prefab, position, rotation);
            obj.name = pName;
            return obj;
        }
    }

    public void Despawn(GameObject obj) {
        string pName = obj.name;
        if (pool.ContainsKey(pName)) pool[pName].Push(obj);
        else {
            Stack<GameObject> stack = new Stack<GameObject>();
            stack.Push(obj);
            pool.Add(pName, stack);
        }

        obj.transform.parent = transform;
        obj.SetActive(false);
    }

    GameObject Recycle(string pName) {
        return pool[pName].Pop();
    }
}

[System.Serializable]
public struct PrePool {
    public GameObject obj;
    public int count;
}
