using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExample : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("if it isn't the player i see there!");
        }

        Debug.Log("entered door region");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            Debug.Log("NO why you leaving!!!");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("this is a collision");
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("An object entered.");
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("An object is still inside of the trigger");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("An object left.");
    }
}
