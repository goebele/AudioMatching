using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public class MatchEvent: UnityEvent<int> { }
    public static UnityEvent ResetEvent = new UnityEvent();
    public static MatchEvent MatchedTrackEvent = new MatchEvent();
    public string trackFolder = "tracks";
    public int numberOfCopies = 3;
    int maxRows = 5;
    float spacing = 2f;

    int matchedTrack = 0;
    int matches = 0;

    // Start is called before the first frame update
    void Start()
    {
        List<AudioClip> sounds = LoadSounds(trackFolder);
        List<GameObject> cards = CreateCardsFromSounds(sounds);
        cards.Shuffle();
        arrayCardsInGrid(cards);
        TrackCard.TrackCardClickedEvent.AddListener(HandleCardClicked);
    }


    void HandleCardClicked(int trackNumber)
    {
        if (matchedTrack == 0)
        {
            matchedTrack = trackNumber;
        }
        if (matchedTrack != trackNumber)
        {

            Invoke("HandleReset", 1f);
        }
        matches++;
        if (matches == numberOfCopies)
        {
            MatchedTrackEvent.Invoke(trackNumber);
        }
    }

    void HandleReset()
    {
        ResetEvent.Invoke();
        matchedTrack = 0;
        matches = 0;
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

                TrackCard script = instance.GetComponent<TrackCard>();
                script.trackNumber = trackNumber;
                audioSource.clip = sound;
                audioSource.volume = .1f;
                audioSource.loop = true;
                audioSource.Play();
                cardsByClip.Add(instance);

            }
            trackNumber++;
        }
        
        return cardsByClip;
    }

    void arrayCardsInGrid(List<GameObject> cards)
    {
        int rows = ((cards.Count-1) / maxRows) + 1;
        int cols = cards.Count/rows;
        int extra = cards.Count % (cols+1);
        int cardCount = 0;
        for(int i = 0; i < rows; i++)
        {
            int topOffset = (rows - 1)- i*2;
            int modifiedCols = cols;
            if (i < extra)
            {
                modifiedCols += 1;
            }
            float leftOffset = modifiedCols *-1;
            for (int j = 0; j<modifiedCols; j++)
            {

                Vector3 newPosition = new Vector3(leftOffset + j * (spacing+.5f), topOffset, 0f);
                cards[cardCount].transform.position = newPosition;
                cardCount++;
            }
        }

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

