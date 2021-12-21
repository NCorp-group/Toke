using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeningarDrop : MonoBehaviour
{
    public int peningarsAvaliable = 0;
    // Start is called before the first frame update
    void Start()
    {
        var tokeStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        //var itemStats = GetComponent<ItemStats>();

        float peningarDropFloat = Random.Range(0f, 3f);
        float luckedPeningarDropFloat = peningarDropFloat * tokeStats.luckMultiplier;
        Debug.Log($"PeningarLuck: Random float number between 0 and 3 is { peningarDropFloat }. Adding luck yields: {luckedPeningarDropFloat}");
        int peningarAmount = (int) Mathf.Ceil(luckedPeningarDropFloat);
        Debug.Log($"Bag has: {peningarAmount} peningars");
        //itemStats.peningar = peningarAmount;
        peningarsAvaliable = peningarAmount;
    }
}
