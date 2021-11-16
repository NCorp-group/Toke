using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float followSpeed = 2f;
    public Transform target;
    public float z = -50f;
    public float offsetY = -1;

    // Update is called once per frame
    void Update()
    {   // https://www.youtube.com/watch?v=FXqwunFQuao
        Vector3 newPos = new Vector3(target.position.x, target.position.y + offsetY, z);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed);
    }
}
