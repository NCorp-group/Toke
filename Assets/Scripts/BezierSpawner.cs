using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierSpawner : MonoBehaviour
{
    public Transform controlPoint;

    private Transform target;

    public CurvedProjectile projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var p = Instantiate(projectile, transform.position, Quaternion.identity);
            p.Setup(controlPoint.position, GetMousePos());
        }
        
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
