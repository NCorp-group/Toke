using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WeaponMovement : MonoBehaviour
{
    private Camera cam;
    private Vector2 mousePosition;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        PlayerPrefs.GetString("weapon", "default");
        rb = GetComponent<Rigidbody2D>();

    }

    public static int health;

    // Update is called once per frame
    void Update()
    {
        //Input.mousePosition
    }

    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Debug.Log("Before first Scene loaded");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoadRuntimeMethod()
    {
        Debug.Log("After first Scene loaded");
    }*/

    
    
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
