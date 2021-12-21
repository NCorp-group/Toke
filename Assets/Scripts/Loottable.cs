using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    private void RuneDrop()
    {
        //Debug.Log("RuneDrop");
        // 1:Athletics 2:Determination 3:Focus 4:Healing 5:Luck 6:Strength
        List<ItemStats> runes = new List<ItemStats>
        {
            Resources.Load<ItemStats>($"runes/Athletics Rune"),
            Resources.Load<ItemStats>($"runes/Determination Rune"),
            Resources.Load<ItemStats>($"runes/Focus Rune"),
            Resources.Load<ItemStats>($"runes/Athletics Rune"),
            Resources.Load<ItemStats>($"runes/Healing Rune"),
            Resources.Load<ItemStats>($"runes/Strength Rune")
        };

        int runeType = Random.Range(0, runes.Count);

        Instantiate(runes[runeType], transform.position, Quaternion.identity);
    }

    private void PeningarDrop()
    {
        var peningarBag = Resources.Load<PenningarDrop>($"items/Penningar Bag");
        Instantiate(peningarBag, transform.position, Quaternion.identity);
    }

    public List<int> lootTable;// = { 10, 90 }; // rune tickets, nothing
    public int totalTickets = 0;
    public int randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        lootTable = new List<int>() { 10, 40, 50 }; // Rune, peningar, nothing
        // Sum up all tickets
        foreach(int tickets in lootTable){
            //Debug.Log($"Ticket entry with {tickets} tickets");
            totalTickets += tickets;
        }
    }

    public void OnDestroy()
    {
        // Generate random number between 0 and totalTickets
        randomNumber = Random.Range(0, totalTickets);
        //Debug.Log($"Random number between 0 and {totalTickets} is: {randomNumber}");

        int n = 0;
        foreach (int tickets in lootTable)
        {
            //Debug.Log($"Tickets: {tickets}, Totaltickets: {totalTickets}");
            totalTickets -= tickets;
            //Debug.Log($"After subtraction, totaltickets left: {totalTickets}");
            if (randomNumber > totalTickets)
            {
                //Debug.Log($"Luck hit in loop number {n}");
                break;
            }
            n++;
        }

        if (n == 0)
        {
            RuneDrop();
        }
        else if (n == 1)
        {
            PeningarDrop();
        }
    }

    /*public void DropLoot()
    {
        // Generate random number between 0 and totalTickets
        randomNumber = Random.Range(0, totalTickets);
        Debug.Log($"Random number between 0 and {totalTickets} is: {randomNumber}");

        int n = 0;
        foreach (int tickets in lootTable)
        {
            Debug.Log($"Tickets: {tickets}, Totaltickets: {totalTickets}");
            totalTickets -= tickets;
            //Debug.Log($"After subtraction, totaltickets left: {totalTickets}");
            if (randomNumber > totalTickets)
            {
                Debug.Log($"Luck hit in loop number {n}");
                break;
            }
            n++;
        }

        if (n == 0)
        {
            RuneDrop();
        }
        else if (n == 1)
        {
            PeningarDrop();
        }
    }*/
}