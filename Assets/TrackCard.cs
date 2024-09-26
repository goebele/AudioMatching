using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackCard : MonoBehaviour
{
    Dictionary<int, Color> testingColors = new Dictionary<int, Color>()
    {
        {1, Color.blue },
        {2, Color.cyan },
        {3, Color.green },
        {4, Color.magenta },
        {5, Color.red },
        {6, Color.yellow },
        {7, Color.grey },
        {8, Color.black }
    };

    public class IntEvent : UnityEvent<int> { };
    public static IntEvent TrackCardClickedEvent = new IntEvent();
    public int trackNumber;
    public bool matched = false;


    // Start is called before the first frame update
    void Start()
    {
        TrackCardClickedEvent.AddListener(OnTrackSelected);
        GameManager.ResetEvent.AddListener(OnTrackReset);
        GameManager.MatchedTrackEvent.AddListener(OnTrackMatched);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = testingColors[this.trackNumber];
        TrackCardClickedEvent.Invoke(this.trackNumber);
    }

    void OnTrackSelected(int track)
    {
        if (track == this.trackNumber) {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = .5f;
        }
    }

    void OnTrackMatched(int track)
    {
        if(this.trackNumber == track)
        {
            this.matched = true;
        }
    }

    void OnTrackReset()
    {
        if (this.matched == false) {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.color = Color.white;
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = .1f;
        }
    }

}
