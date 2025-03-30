using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class RarityRandomList<T>
{
    [System.Serializable]
    public struct Pair
    {
        public T item;
        public float rarity;

        public Pair(T item, float rarity)
        {
            this.item = item;
            this.rarity = rarity;
        }
    }

    public List<Pair> list = new List<Pair>();

    public int Count => list.Count;

    public void Add(T item, float rarity)
    {
        list.Add(new Pair(item, rarity));
    }

    public T GetRandom()
    {
        var totalRarity = list.Sum(p => p.rarity);

        var value = Random.value * totalRarity;

        float sumRarity= 0;
        foreach (var p in list)
        {
            sumRarity += p.rarity;

            if (sumRarity >= value)
            {
                return p.item;
            }
        }
        return default(T);
    }
}