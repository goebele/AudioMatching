using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateManager
{

    static int matchedTrack = -1;
    static int matches = 0;
    static int numberOfCopies = 3;
    public static bool animating = false;
    public class MatchEvent : UnityEvent<int> { }
    public static UnityEvent ResetEvent = new UnityEvent();
    public static MatchEvent MatchedTrackEvent = new MatchEvent();
    public static UnityEvent AnimatingEvent = new UnityEvent();
    public static UnityEvent AnimationFinishedEvent = new UnityEvent();

    public static void HandleCardClicked(int trackNumber)
    {
        if (matchedTrack == -1)
        {
            matchedTrack = trackNumber;
        }
        TrackCard.TrackCardClickedEvent.Invoke(trackNumber);
        matches++;
        if (matchedTrack != trackNumber)
        {
            HandleReset();
        }
        if (matches == numberOfCopies)
        {
            MatchedTrackEvent.Invoke(trackNumber);
            matchedTrack = -1;
        }
    }

    static void HandleReset()
    {
        
        matchedTrack = -1;
        matches = 0;
        ResetEvent.Invoke();
    }

    public StateManager()
    {
        AnimatingEvent.AddListener(setAnimatingFlag);
        AnimationFinishedEvent.AddListener(unsetAnimatingFlag);
    }

    static void setAnimatingFlag() {
        animating = true;
    }
    static void unsetAnimatingFlag()
    {
        animating = false;
    }

}
