using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string trackFolder = "tracks";
    public int numberOfCopies = 3;
    // Start is called before the first frame update
    void Start()
    {
        List<AudioClip> sounds = LoadSounds(trackFolder);
        List<GameObject> cardsByClip = CreateCardsFromSounds(sounds);
        cardsByClip.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    List<AudioClip> LoadSounds(string folderName)
    {
        string path = Application.dataPath + "/Resources/" + folderName;
        List<AudioClip> trackList = new List<AudioClip>();
        string[] trackNames = Directory.GetFiles(path);
        foreach(string trackName in trackNames)
        {
            AudioClip clip = Resources.Load<AudioClip>(folderName + "/" + Path.GetFileNameWithoutExtension(trackName));
            if (clip is not null)
            {
                trackList.Add(clip);
            }
        }
        return trackList;
    }

    List<GameObject> CreateCardsFromSounds(List<AudioClip> sounds)
    {
        int trackNumber = 1;
        List<GameObject> cardsByClip = new List<GameObject>();
        foreach (AudioClip sound in sounds)
        {
            for (int i = 0; i < numberOfCopies; i++)
            {
                GameObject instance = Instantiate(Resources.Load<GameObject>("TrackCard"));
                AudioSource audioSource = instance.GetComponent<AudioSource>();
                audioSource.clip = sound;
                cardsByClip.Add(instance);
            }
            trackNumber++;
        }
        
        return cardsByClip;
    }

}

public static class RandomList
{
    //Fisher-Yates implementation stolen from stackoverflow https://stackoverflow.com/questions/273313/randomize-a-listt
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

