using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ProjectRuntime.Managers
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { get; private set; }

        public static string StageIdToLoad = string.Empty; // Set outside the gameplay stage 

        public bool IsPaused => this._pauseType != PauseType.None;
        private PauseType _pauseType;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("There are 2 or more BattleManagers in the scene");
            }
        }

        private void Start()
        {
            this.Init().Forget();
        }

        public async UniTaskVoid Init()
        {
            // Spawn in the map and set it up

            // Teleport the player to the spawn position

            // Scene transition open and start the timer
        }

        public void CompleteLevel()
        {
            // Pause Game

        }

        #region Pause Logic
        public void PauseGame(PauseType type)
        {
            this._pauseType |= type;
            if (this.IsPaused)
            {
                Time.timeScale = 0f;
            }
        }

        public void ResumeGame(PauseType type)
        {
            this._pauseType &= ~type;
            if (!this.IsPaused)
            {
                Time.timeScale = 1f;
            }
        }

        [Flags]
        public enum PauseType
        {
            None = 0,
            PnlPause = 1 << 0,
        }
        #endregion
    }
}