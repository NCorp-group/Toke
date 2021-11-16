using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CurvedProjectile : MonoBehaviour
{
    public Projectile projectile;
    public GameObject visualIndicator;

    //public Vector2 P0;

    public Transform target;
    public Transform controlPoint;
    
    private Vector2 P1;
    
    private Vector2 P2;
    // TODO: use charge
    public float distance = 5f;

    [Header("The time in seconds, the projectile will be guided by the Bezier Curve")]
    public float t = 5f;
    
    public float speed = 1f;
    public float acceleration = 0f;

    public float offset = 1f;
    public float duration = 3f;
    
    
    public int granularity = 10;
    
    private List<Vector2> controlPoints = new List<Vector2>();

    private Vector2 P0;

    private float spawnTime;
    
    // Start is called before the first frame update
    void Start()
    {
        P0 = transform.position;
        if (controlPoint != null)
        {
            P1 = controlPoint.position;
        }

        if (target != null)
        {
            P2 = target.position;
        }

        spawnTime = Time.time;
    }

    public void Setup(Vector2 controlPoint, Vector2 target)
    {
        P1 = controlPoint;
        P2 = target;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        var right_btn_clicked = Input.GetMouseButtonDown(1);
        if (right_btn_clicked)
        {
            SetControlPoint();
        }
        
        var left_btn_clicked = Input.GetMouseButtonDown(0);
        if (left_btn_clicked)
        {
            Fire();
        }
        */
        
        Move();
    }

    private void Move()
    {
        var t = (Time.time - spawnTime - offset) / duration;
        if (t <= 1.0f)
        {
            transform.position = Bezier(t, P0, P1, P2);
        }
    }
    
    private void Fire()
    {
        var P0  = new Vector2(transform.position.x, transform.position.y);
        var direction = transform.eulerAngles.normalized;
        var P1 = controlPoints[0];
        
        var P2= GetMousePos();

        // https://en.wikipedia.org/wiki/B%C3%A9zier_curve
        var B = new Func<int, Vector2>((t) => (1 - t) * ((1 - t) * P0 + t * P1) + t*((1 - t)*P1 + t*P2));
        var derivative = new Func<int, Vector2>(t => 
            2 * (1 - t) * (P1 - P0) + 2 * t * (P2 - P1)
        );
        
        //Vector2.L
        
        // give B as a setup function to the projectile
        var p = Instantiate(projectile, transform.position, Quaternion.Euler((P1 - P0).normalized));
        // p.Setup(B, derivative, speed, acceleration);
        
    }

    private Vector2 Bezier(float t, Vector2 P0, Vector2 P1, Vector2 P2)
    {
        var Q0 = Vector2.Lerp(P0, P1, t);
        var Q1 = Vector2.Lerp(P1, P2, t);
        return Vector2.Lerp(Q0, Q1, t);
    }

    private void SetControlPoint()
    {
        var mousePos = GetMousePos();
        Instantiate(visualIndicator, mousePos, Quaternion.identity);
        controlPoints.Add(mousePos);
    }


    private Vector2 GetMousePos()
    {
        var mousePos = Input.mousePosition;   
        mousePos.z = Camera.main.nearClipPlane;
        var Worldpos=Camera.main.ScreenToWorldPoint(mousePos);
        var Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);

        return Worldpos2D;
    }
}
