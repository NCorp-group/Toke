using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public Camera cam;
    private Vector2 mousePosition;
    public const float a = 0.1f;
    public const float b = 0.2f;

    public float mouseX;
    public float mouseY;
    public float slope;
    public float intercept;
    public float weaponX;
    public float weaponY;
    //private Vector2 ellipseParameters = new Vector2(1,2);

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        //Input.mousePosition
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        /*float*/ mouseX = mousePosition.x;
        /*float*/ mouseY = mousePosition.y;
        
        /*/*float#1# slope = -math.sqrt((- Mathf.Pow(a,2) + Mathf.Pow(mouseX,2)) * b
                      + Mathf.Pow(mouseY, 2) * Mathf.Pow(a,2) - mouseX * mouseY)
                      / (Mathf.Pow(a,2) * Mathf.Pow(mouseX,2));
        /*float#1# intercept = mouseY - slope * mouseX;

        /*float#1# weaponX = (math.sqrt(Mathf.Pow(a,2) * Mathf.Pow(b,2) 
                            * (Mathf.Pow(a,2) * Mathf.Pow(slope,2) + (Mathf.Pow(b,2) - Mathf.Pow(intercept,2)))) 
                            - Mathf.Pow(a,2) * intercept * slope)
                            / Mathf.Pow(a,2) * Mathf.Pow(slope,2) + Mathf.Pow(b,2);
        /*float#1# weaponY = (math.sqrt(Mathf.Pow(a, 2) * Mathf.Pow(b, 2) * Mathf.Pow(slope, 2)
                            * (Mathf.Pow(a, 2) * Mathf.Pow(mouseX,2) + (Mathf.Pow(b, 2) - Mathf.Pow(intercept, 2))))
                            + Mathf.Pow(b,2) * intercept)
                            / (Mathf.Pow(a,2) * Mathf.Pow(mouseX,2) + Mathf.Pow(b, 2));

        Vector2 newWeaponPos = new Vector2(weaponX, weaponY);
        rb.position = newWeaponPos;*/
        //rb.position.Set(weaponX, weaponY);
    }

    void FixedUpdate()
    {
        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}
