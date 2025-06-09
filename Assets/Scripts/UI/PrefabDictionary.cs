using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PrefabDictionary", menuName = "Game/Prefab Dictionary")]
public class PrefabDictionary : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public GameItemData itemData;
        public GameObject prefab;
    }

    public List<Entry> entries;

    private Dictionary<string, GameObject> lookup;

    public void Initialize()
    {
        lookup = new Dictionary<string, GameObject>();
        foreach (var entry in entries)
        {
            if (entry.itemData != null && !lookup.ContainsKey(entry.itemData.itemName))
            {
                lookup.Add(entry.itemData.itemName, entry.prefab);
            }
        }
    }

    public GameObject GetPrefab(GameItemData data)
    {
        if (lookup == null)
            Initialize();

        if (lookup.TryGetValue(data.itemName, out var prefab))
        {
            return prefab;
        }

        Debug.LogWarning($"[PrefabDictionary] No se encontró prefab para: {data.name}");
        return null;
    }
    


    
}