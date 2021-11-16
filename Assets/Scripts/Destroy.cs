using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void DestroyYourself()
    {
        Destroy(gameObject);
    }
}
