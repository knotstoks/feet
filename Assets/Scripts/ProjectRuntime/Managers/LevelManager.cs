using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRuntime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [field: SerializeField, Header("Scene References")]
        public Transform PlayerSpawnTransform { get; private set; }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("There are 2 or more LevelManagers in the scene");
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}