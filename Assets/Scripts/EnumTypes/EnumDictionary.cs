using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnumTypes
{
    
    [Serializable]
    public class EnumDictionary<TEnum, TValue> : ISerializationCallbackReceiver where TEnum : Enum
    {
        [Serializable]
        public struct Pair
        {
            public TEnum key;
            public TValue value;
        }

        [SerializeField]
        private List<Pair> pairs = new();

        private Dictionary<TEnum, TValue> dictionary;

        private void EnsureDictionary()
        {
            if (dictionary != null) return;

            dictionary = new Dictionary<TEnum, TValue>();
            foreach (var pair in pairs)
            {
                dictionary[pair.key] = pair.value;
            }
        }

        public TValue this[TEnum key]
        {
            get
            {
                EnsureDictionary();
                return dictionary[key];
            }
            set
            {
                EnsureDictionary();
                dictionary[key] = value;

                for (int i = 0; i < pairs.Count; i++)
                {
                    if (EqualityComparer<TEnum>.Default.Equals(pairs[i].key, key))
                    {
                        pairs[i] = new Pair { key = key, value = value };
                        return;
                    }
                }

                pairs.Add(new Pair { key = key, value = value });
            }
        }

        public bool ContainsKey(TEnum key)
        {
            EnsureDictionary();
            return dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TEnum key, out TValue value)
        {
            EnsureDictionary();
            return dictionary.TryGetValue(key, out value);
        }

        public IEnumerable<TEnum> Keys
        {
            get
            {
                EnsureDictionary();
                return dictionary.Keys;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                EnsureDictionary();
                return dictionary.Values;
            }
        }

        public int Count
        {
            get
            {
                EnsureDictionary();
                return dictionary.Count;
            }
        }

        public void Remove(TEnum key)
        {
            EnsureDictionary();
            if (dictionary.Remove(key))
            {
                pairs.RemoveAll(p => EqualityComparer<TEnum>.Default.Equals(p.key, key));
            }
        }

        public void Clear()
        {
            pairs.Clear();
            dictionary = null;
        }

        public Dictionary<TEnum, TValue> ToDictionary()
        {
            EnsureDictionary();
            return new Dictionary<TEnum, TValue>(dictionary);
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            dictionary = null;
        }

        public List<Pair> Pairs => pairs;
    }
}