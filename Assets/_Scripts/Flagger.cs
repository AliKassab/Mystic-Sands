using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flagger : MonoBehaviour
{
    public bool MetPotTrader = false;
    public bool MetCommander = false;
    public bool InitiateEnding = false;

    [SerializeField] GameObject GoodEndingTrigger;
    [SerializeField] GameObject SecretEndingTrigger;

    [Header("Ending screens")]
    [SerializeField] string goodEndingMessage = "You saved the heritage of the city";
    [SerializeField] string secretEndingMessage = "You uncovered the city's hidden secret";
    [SerializeField] float endingDuration = 4f;

    private bool _ended;

    private void Update()
    {
        if (_ended || !InitiateEnding) return;

        // Good ending (the "win"): show the message, then return to the Main Menu.
        if (MetPotTrader && !MetCommander)
            PlayEnding(goodEndingMessage);
        // Secret ending: also wraps up and returns to the menu.
        else if (MetPotTrader && MetCommander)
            PlayEnding(secretEndingMessage);
    }

    private void PlayEnding(string message)
    {
        _ended = true;
        EndingSequence.Play(message, endingDuration);
    }
}
