using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<K,V> {

    public List<K> keys;
    public List<V> values;

    public SerializableDictionary(){
        keys = new List<K>();
        values = new List<V>();
    }

    public V this[K key]
    {
        get { return get(key); }
        set { set(key, value); }
    }

    public int IndexOf(K key)
    {
        return keys.IndexOf(key);
    }
    public int IndexOf(V value)
    {
        return values.IndexOf(value);
    }

    public V get(K key)
    {
        int index = keys.IndexOf(key);
        return values[index];
    }

    public void set(K key, V value)
    {
        if (!keyExist(key))
        {
            keys.Add(key);
            values.Add(value);
        }
        else
        {
            int index = keys.IndexOf(key);
            values[index] = value;
        }
        
    }

    public bool keyExist(K key) {
        return keys.IndexOf(key) != -1;
    }


    public void add(Dictionary<K, V> dictionary)
    {
        foreach (var item in dictionary)
        {
            this[item.Key] = item.Value;
        }
    }
}


//Specifics

[System.Serializable]
public class SerializableStringStringDictionary : SerializableDictionary<string, string> {}

[System.Serializable]
public class SerializableStringVector3Dictionary : SerializableDictionary<string, Vector3> { }

[System.Serializable]
public class SerializableStringVector2Dictionary : SerializableDictionary<string, Vector2> { }

[System.Serializable]
public class SerializableStringGameObjectDictionary : SerializableDictionary<string, GameObject> { }