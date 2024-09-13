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

    private void Update()
    {
        if (MetPotTrader && !MetCommander && InitiateEnding)
            Destroy(SecretEndingTrigger);
        else if (MetCommander && MetPotTrader && InitiateEnding) 
            Destroy(GoodEndingTrigger);

    }
}
