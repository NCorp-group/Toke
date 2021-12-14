using UnityEngine;

public class FollowObject : MonoBehaviour
{
  
    public float followSpeed = 2f;
    private Transform target;
    public float z = -50f;
    public float offsetY = -1;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {   // https://www.youtube.com/watch?v=FXqwunFQuao
        Vector3 newPos = new Vector3(target.position.x, target.position.y + offsetY, z);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed);
    }
}
