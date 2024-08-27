using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemeSpawner : MonoBehaviour
{
    public static MemeSpawner Instance;

    public Transform memeContainer;

    public Sprite[] memeSprites;

    [SerializedDictionary("Meme name", "sprite")]
    public SerializedDictionary<string, Sprite> MemesDict = new();

    private void Awake()
    {
        Instance = this;

        CreateDict();
    }

    void CreateDict()
    {
        MemesDict.Clear();
        foreach (Sprite sprite in memeSprites)
        {
            MemesDict.Add(sprite.name, sprite);
        }
    }

    public Sprite GetRandomAvatar()
    {
        int ranIndex = Random.Range(0, MemesDict.Count);

        return MemesDict.ElementAt(ranIndex).Value;
    }

    public Sprite GetMemeSpriteByName(string str)
    {
        if (MemesDict.TryGetValue(str, out Sprite sprite))
        {
            return sprite;
        }

        return null;
    }

    public void DisableMemeSpawner()
    {
        memeContainer.gameObject.SetActive(false);
    }

    public void ReactivateMemeSpawner()
    {
        memeContainer.gameObject.SetActive(true);

    }
}
