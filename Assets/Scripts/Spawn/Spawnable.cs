using UnityEngine;

namespace Spawn
{
    public interface ISpawnable
    {
        void SpawnObj(Transform spawningPoint);

        float Delay();
    }
}