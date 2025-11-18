using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    [field: SerializeField, Header("Scene References")]
    private Transform KickCastTransform { get; set; }

    [field: SerializeField, Header("Kick Settings")]
    private float BoxcastWidth { get; set; }

    [field: SerializeField]
    private float KickCooldown { get; set; }

    private PlayerInput _playerInput;
    private LayerMask _destructionLayerMask;

    // Timers
    private float _kickCooldownTimer;

    private void Awake()
    {
        this._destructionLayerMask = LayerMask.GetMask("Destructable");
    }

    public void Init(PlayerInput playerInput)
    {
        this._playerInput = playerInput;

        this._playerInput.CharacterControls.Kick.performed += context => this.OnKick(context);
    }

    private void Update()
    {
        if (this._kickCooldownTimer > 0)
        {
            this._kickCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnKick(InputAction.CallbackContext _)
    {
        if (this._kickCooldownTimer > 0)
        {
            return;
        }

        this._kickCooldownTimer = this.KickCooldown;

        var cast = Physics.OverlapBox(this.KickCastTransform.position, 0.5f * this.BoxcastWidth * Vector3.one, this.transform.rotation, this._destructionLayerMask);
        if (cast.Length > 0)
        {
            foreach (var collider in cast)
            {
                Debug.Log(collider.gameObject.name);
            }
        }

        // TODO: Some animation to kick feet
    }
}
