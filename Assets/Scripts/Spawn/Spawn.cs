using UnityEngine;

namespace Spawn
{
    public class Spawn : MonoBehaviour, ISpawnable
    {
        public GameObject spawn;
        public float angle = 0f;
        public float delay = 0f;
        public float speed = 1f;
        public Vector2 velocity = Vector2.zero;

        public void SpawnObj(Transform spawningPoint)
        {
            var obj = Instantiate(spawn, spawningPoint.position, Quaternion.Euler(0, 0, angle));
            obj.GetComponent<Rigidbody2D>()?.AddForce(velocity * speed, ForceMode2D.Impulse);

            var hack = Resources.Load("") as GameObject;
            
            // remember to clean up!
            // Destroy(gameObject);
        }

        float ISpawnable.Delay()
        {
            return delay;
        }
    }
}
