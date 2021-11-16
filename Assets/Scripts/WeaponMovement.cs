using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public Camera cam;
    private Vector2 mousePosition;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        //Input.mousePosition
    }

    void FixedUpdate()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        // srb.SetRotation()
        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        // rb.rotation = angle;
        //Debug.Log($"angle is {angle}");
        // srb.angularVelocity = angle;
        rb.MoveRotation(angle);
        //rb.SetRotation(angle);
    }
}
