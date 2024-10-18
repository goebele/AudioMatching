using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackCard : MonoBehaviour
{

    public class IntEvent : UnityEvent<int> { };
    public static IntEvent TrackCardClickedEvent = new IntEvent();
    public int trackNumber;
    public bool matched = false;
    public bool flipped = false;
    public bool animating = false; 


    // Start is called before the first frame update
    void Start()
    {
        TrackCardClickedEvent.AddListener(OnTrackSelected);
        StateManager.ResetEvent.AddListener(OnTrackReset);
        StateManager.MatchedTrackEvent.AddListener(OnTrackMatched);
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    void OnMouseDown()
    {
        
        this.flipped = true;
        StateManager.HandleCardClicked(trackNumber);
        StartCoroutine(Flip());

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
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0f;
            if(this.flipped == true)
            {
                this.flipped = false;
                StartCoroutine(Flip());
            }
        }
        
    }

    IEnumerator Flip(float durationSeconds = .5f)
    {
        while (StateManager.animating == true || this.animating == true)
        {
            yield return null;
        }
        this.animating = true;


        float timeElapsed = 0f;
        while(timeElapsed < durationSeconds)
        {
            
            float rotation = 180 * Time.deltaTime/durationSeconds;
            gameObject.transform.Rotate(new Vector3(0, rotation, 0));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        this.animating = false;
        yield break;
    }

}
