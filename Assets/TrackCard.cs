using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class TrackCard : MonoBehaviour
{
    public int trackNumber;
    public bool matched = false;
    public bool flippedUp = false;
    public bool animating = false;
    public Action<TrackCard> clickedCallback;
    public GameManager.ModifyScore modifier;
    public int score;

    public void setScore()
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        string mod = "";
        if(this.modifier == GameManager.MultiplyScore)
        {
            mod = "x";
        } else
        {
            mod = "+";
        }
        string scorelabel = $"{mod}{score}";
        foreach(TMP_Text text in texts)
        {
            text.text = scorelabel;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        clickedCallback(this);
    }



    public void PlayTrack()
    {
          AudioSource audioSource = GetComponent<AudioSource>();
          audioSource.volume = .5f;
    }



    void OnTrackMatched(int track)
    {
        if (this.trackNumber == track)
        {
            this.matched = true;
        }
    }

    void OnTrackReset()
    {
        if (this.matched == false) {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0f;
        }

    }

    public void FlipCard()
    {
        StartCoroutine(Flip());
    }

    IEnumerator Flip(float durationSeconds = .3f)
    {
        if (this.flippedUp)
        {
            this.flippedUp = false;
        }
        else
        {
            this.flippedUp = true;
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
