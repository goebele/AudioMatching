using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UIElements;



public class GameManager : MonoBehaviour
{

    static VisualElement root;
    public string trackFolder = "tracks";
    public string artFolder = "Cards";
    public int numberOfCopies = 3;
    int maxRows = 5;
    float spacing = 2f;

    public delegate int ModifyScore(int modifier, int score);

    public static int MultiplyScore(int modifier, int score)
    {
        return score * modifier;
    }

    public static int AddScore(int modifier, int score)
    {
        return score + modifier;
    }

    public static void UpdateScore(int newScore)
    {
        Label score = root.Q<Label>("Score");
        score.text = $"Score: {newScore}";
    }

    List<int> scoreOptions = new List<int> { 10, 15, 20};
    List<ModifyScore> modifierOptions = new List<ModifyScore> { MultiplyScore, AddScore };

    class ScoreModifier{
        int score;
        ModifyScore modifier;
    }

    // Start is called before the first frame update
    void Start()
    {
        List<AudioClip> sounds = LoadSounds(trackFolder);
        List<Sprite> art = LoadCardBacks(artFolder);
        List<GameObject> cards = CreateCardsFromSoundsAndArt(sounds, art);
        cards.Shuffle();
        arrayCardsInGrid(cards);
        StateManager stateManager = gameObject.AddComponent(typeof(StateManager)) as StateManager;
        root = GetComponent<UIDocument>().rootVisualElement;
        stateManager.Init(cards);
        
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

    List<Sprite> LoadCardBacks(string folderName)
    {
        string path = Application.dataPath + "/Resources/" + folderName;
        List<Sprite> cardBackList = new List<Sprite>();
        string[] cardBacknames = Directory.GetFiles(path);
        foreach (string cardBackName in cardBacknames)
        {
            Sprite cardBack = Resources.Load<Sprite>(folderName + "/" + Path.GetFileNameWithoutExtension(cardBackName));
            if (cardBack is not null)
            {
                cardBackList.Add(cardBack);
            }
        }
        return cardBackList;
    }

    List<GameObject> CreateCardsFromSoundsAndArt(List<AudioClip> sounds, List<Sprite> art)
    {
        int trackNumber = 0;
        List<GameObject> cardsByClip = new List<GameObject>();
        foreach (AudioClip sound in sounds)
        {
            int score = scoreOptions[(trackNumber + 1) % scoreOptions.Count];
            ModifyScore modifier = modifierOptions[(trackNumber + 1) % modifierOptions.Count];
            for (int i = 0; i < numberOfCopies; i++)
            {
                GameObject instance = Instantiate(Resources.Load<GameObject>("TrackCard"));
                AudioSource audioSource = instance.GetComponent<AudioSource>();
                SpriteRenderer spriteRenderer = instance.transform.Find("CardFront").GetComponent<SpriteRenderer>();
                TrackCard script = instance.GetComponent<TrackCard>();
                
                script.trackNumber = trackNumber;
                spriteRenderer.sprite = art[trackNumber];
                audioSource.clip = sound;
                audioSource.volume = 0f;
                audioSource.loop = true;
                audioSource.Play();
                cardsByClip.Add(instance);
                script.score = score;
                script.modifier = modifier;
                script.setScore();

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

