using System;
using BroccoliBunnyStudios.Managers;
using BroccoliBunnyStudios.Panel;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectRuntime.Player;
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

            PanelManager.Instance.FadeToBlackAsync(0f).Forget(); //temp for now
        }

        private void Start()
        {
            this.Init().Forget();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public async UniTaskVoid Init()
        {
            // Spawn in the map and set it up

            await UniTask.WaitUntil(() => LevelManager.Instance != null);

            // Teleport the player to the spawn position
            PlayerMovement.Instance.transform.position = LevelManager.Instance.PlayerSpawnTransform.position;

            // Scene transition open and start the timer
            await PanelManager.Instance.FadeFromBlack();
            if (!this) return;

            PlayerMovement.Instance.TogglePlayerMovement(true);
        }

        public void CompleteLevel()
        {
            // TODO: Open PnlPostGame
            SceneManager.Instance.LoadSceneAsync("ScGame").Forget();
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