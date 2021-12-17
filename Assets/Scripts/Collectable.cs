using UnityEngine;
using UnityEngine.Assertions;

public class Collectable : MonoBehaviour
{
    public enum Variant
    {
        PROJECTILE,
        MODIFIER
    };

    public Variant variant;
    public GameObject item;

    private bool _collected = false;
    void Start()
    {
        Assert.IsNotNull(item);
    }

    public GameObject Collect()
    {
        _collected = true;
        return item;
    }

    public void Update()
    {
        //if (_collected) Destroy(gameObject);
    }
}
