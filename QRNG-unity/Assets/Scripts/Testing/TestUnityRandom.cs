using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnityRandom : MonoBehaviour
{

    public int[] coinFlipResults;
    public int[] diceRollResults;
    public int[] cardPickResults;

    // Start is called before the first frame update
    void Start()
    {
        coinFlipResults = new int[8192];
        diceRollResults = new int[8192];
        cardPickResults = new int[8192];
    }
}
