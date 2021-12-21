using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenningarDrop : MonoBehaviour
{
    public int penningarsAvaliable = 0;
    // Start is called before the first frame update
    void Start()
    {
        var tokeStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        //var itemStats = GetComponent<ItemStats>();

        float penningarDropFloat = Random.Range(0f, 3f);
        float luckedPenningarDropFloat = penningarDropFloat * tokeStats.luckMultiplier;
        //Debug.Log($"PeningarLuck: Random float number between 0 and 3 is { peningarDropFloat }. Adding luck yields: {luckedPeningarDropFloat}");
        int penningarAmount = (int) Mathf.Ceil(luckedPenningarDropFloat);
        //Debug.Log($"Bag has: {peningarAmount} peningars");
        //itemStats.peningar = peningarAmount;
        penningarsAvaliable = penningarAmount;
    }
}
