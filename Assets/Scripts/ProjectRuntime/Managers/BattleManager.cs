using System;
using BroccoliBunnyStudios.Managers;
using BroccoliBunnyStudios.Panel;
using BroccoliBunnyStudios.Pools;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ProjectRuntime.Player;
using UnityEngine;

namespace ProjectRuntime.Managers
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { get; private set; }

        [field: SerializeField, Header("Cheats")]
        private bool WillSkipStageSetup { get; set; }

        [field: SerializeField]
        private string EditorIdToLoad { get; set; }

        public static string WorldIdToLoad = string.Empty; // Set outside the gameplay stage 

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
            if (this.WillSkipStageSetup)
            {
                PanelManager.Instance.FadeFromBlack(0f).Forget();
                PlayerInputManager.Instance.TogglePlayerInput(true);
            }
            else
            {
                this.Init().Forget();
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public async UniTaskVoid Init()
        {
            // Spawn in the map and set it up
            if (string.IsNullOrEmpty(WorldIdToLoad))
            {
                WorldIdToLoad = EditorIdToLoad;
            }
            var dWorld = DWorld.GetDataById(WorldIdToLoad).Value;
            var worldPrefab = ResourceLoader.InstantiateAsync(dWorld.PrefabName);

            await UniTask.WaitUntil(() => LevelManager.Instance != null);

            // Teleport the player to the spawn position
            await UniTask.WaitUntil(() => PlayerMovement.Instance != null);
            PlayerMovement.Instance.transform.position = LevelManager.Instance.PlayerSpawnTransform.position;

            // Scene transition open and start the timer
            await PanelManager.Instance.FadeFromBlack();
            if (!this) return;

            await UniTask.WaitUntil(() => PlayerInputManager.Instance != null);
            PlayerInputManager.Instance.TogglePlayerInput(true);
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