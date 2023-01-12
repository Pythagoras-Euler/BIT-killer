using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HashMap<TKey, TValue> : IDictionary<TKey, TValue>, IEnumerable
{
    Dictionary<TKey, TValue> HashMapData = new Dictionary<TKey, TValue>();
    public TValue this[TKey key]
    {
        get
        {
            return HashMapData[key];
        }

        set
        {
            if (HashMapData.ContainsKey(key))
            {
                HashMapData[key] = value;
            }
            else
            {
                HashMapData.Add(key, value);
            }
        }
    }

    public int Count
    {
        get
        {
            return HashMapData.Count;
        }
    }

    //如果 true 是只读的，则为 IDictionary；否则为 false。 在 Dictionary<TKey,TValue> 的默认实现中，此属性始终返回 false。
    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }

    public ICollection<TKey> Keys
    {
        get
        {
            return HashMapData.Keys;
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            return HashMapData.Values;
        }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        if (item.Key == null)
        {
            throw new ArgumentNullException("key can't be null.");
        }
        if (HashMapData.ContainsKey(item.Key))
        {
            HashMapData[item.Key] = item.Value;
        }
        else
        {
            HashMapData.Add(item.Key, item.Value);
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (key == null)
        {
            throw new ArgumentNullException("key can't be null.");
        }
        if (HashMapData.ContainsKey(key))
        {
            HashMapData[key] = value;
        }
        else
        {
            HashMapData.Add(key, value);
        }
    }

    public void Clear()
    {
        HashMapData.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return HashMapData.Contains(item);
    }

    public bool ContainsKey(TKey key)
    {
        return HashMapData.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (array == null)
        {
            return;
        }
        if (array.Length <= arrayIndex)
        {
            return;
        }
        int count = array.Length;
        for (int i = 0; i < count; i++)
        {
            if (HashMapData.ContainsKey(array[i].Key))
            {
                HashMapData[array[i].Key] = array[i].Value;
            }
            else
            {
                HashMapData.Add(array[i].Key, array[i].Value);
            }
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return HashMapData.GetEnumerator();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (HashMapData.ContainsKey(item.Key) && HashMapData.ContainsValue(item.Value))
        {
            return HashMapData.Remove(item.Key);
        }
        return false;
    }

    public bool Remove(TKey key)
    {
        return HashMapData.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        try
        {
            if (HashMapData.ContainsKey(key))
            {
                value = HashMapData[key];
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }
        catch
        {
            value = default(TValue);
            return false;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return HashMapData.GetEnumerator();
    }
}

