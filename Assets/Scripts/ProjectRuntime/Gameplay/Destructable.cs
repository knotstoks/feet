using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRuntime.Gameplay
{
    public class Destructable : MonoBehaviour
    {
        [field: SerializeField, Header("Scene References")]
        private GameObject OriginalObject { get; set; }

        [field: SerializeField]
        private GameObject BrokenObjectParent { get; set; }

        [field: SerializeField]
        private List<Rigidbody> BrokenShards { get; set; }

        private bool _isBroken = false;

        public void OnBreak(Vector3 forceDirection)
        {
            if (this._isBroken)
            {
                return;
            }

            this._isBroken = true;
            this.OriginalObject.SetActive(false);
            this.BrokenObjectParent.SetActive(true);
            var dir = forceDirection.normalized;
            foreach (var shard in BrokenShards)
            {
                shard.AddForce(this.GetRandomizedForceDirection(dir), ForceMode.Impulse);
            }
        }

        private Vector3 GetRandomizedForceDirection(Vector3 dir)
        {
            dir = dir.normalized;
            dir.x += Random.Range(-2f, 2f);
            dir.y += Random.Range(-0.5f, 0.5f);
            dir.z += Random.Range(-1f, 1f);

            return dir.normalized;
        }
    }
}