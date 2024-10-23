using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateManager: MonoBehaviour
{

    static int matchedTrack = -1;
    static int matches = 0;
    static int numberOfCopies = 3;
    static int score = 0;
    public static bool animating = false;
    List<TrackCard> totalCards = new List<TrackCard>();
    List<TrackCard> flippedCards = new List<TrackCard>();

    public void Init(List<GameObject> cards)
    {
        foreach(GameObject cardObject in cards)
        {
            TrackCard card = getCardScript(cardObject);
            card.clickedCallback = HandleCardClicked;
            totalCards.Add(card);
        }
        GameManager.UpdateScore(0);
    }

    private TrackCard getCardScript(GameObject card)
    {
        return (TrackCard)card.GetComponent<TrackCard>();
    }

    private bool isCardInGroupStillAnimating(List<TrackCard> cards)
    {
        foreach(TrackCard card in cards)
        {
            if (card.animating == true)
            {
                return true;
            }
        }
        return false;
    }

    public void HandleCardClicked(TrackCard card)
    {
        if (card.flippedUp)
        {
            return;
        }
        card.FlipCard();
        if (matchedTrack == -1)
        {
            matchedTrack = card.trackNumber;
        }
        flippedCards.Add(card);
        matches++;
        if (matchedTrack != card.trackNumber)
        {
            List<TrackCard> wrongMatches = new List<TrackCard>(flippedCards);
            StartCoroutine(HandleReset(wrongMatches));
            flippedCards.Clear();
        }
        if (matches == numberOfCopies)
        {
            matchedTrack = -1;
            foreach(TrackCard matchedCard in flippedCards)
            {
                card.PlayTrack();
            }
            flippedCards.Clear();
            score = card.modifier(card.score, score);
            GameManager.UpdateScore(score);

        }
    }

    IEnumerator HandleReset(List<TrackCard> wrongMatches)
    {

        matchedTrack = -1;
        matches = 0;
        while (isCardInGroupStillAnimating(wrongMatches))
        {
            yield return null;
        }
        foreach(TrackCard card in wrongMatches)
        {
            card.FlipCard();
        }

        yield break;
    }
   
    void handleMatch()
    {
        foreach(TrackCard card in flippedCards)
        {
            card.matched = true;
        }
    }


}
