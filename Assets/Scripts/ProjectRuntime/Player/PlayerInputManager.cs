using Cysharp.Threading.Tasks;
using ProjectRuntime.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    public PlayerInput PlayerInput { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There are 2 or more PlayerInputManagers in the scene");
        }

        this.PlayerInput = new PlayerInput();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        this.Init();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private async void Init()
    {
        // Init the Player Weapon System

        // Init the Player Movement
        await UniTask.WaitUntil(() => PlayerMovement.Instance != null);
        if (!this) return;
        PlayerMovement.Instance.Init(this.PlayerInput);

    }

    public void TogglePlayerInput(bool toggle)
    {
        if (toggle)
        {
            this.PlayerInput.CharacterControls.Enable();
        }
        else
        {
            this.PlayerInput.CharacterControls.Disable();
        }
    }
}
