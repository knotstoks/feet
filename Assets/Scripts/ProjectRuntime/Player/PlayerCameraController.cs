using UnityEngine;

namespace ProjectRuntime.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [field: SerializeField, Header("Scene References")]
        private Transform OrientationTransform { get; set; }

        [field: SerializeField, Header("Camera Settings")]
        private float SensitivityX { get; set; }

        [field: SerializeField]
        private float SensitivityY { get; set; }

        private float _xRotation;
        private float _yRotation;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * this.SensitivityX;
            var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * this.SensitivityY;

            this._yRotation += mouseX;
            this._xRotation -= mouseY;
            this._xRotation = Mathf.Clamp(this._xRotation, -90f, 90f);

            this.transform.rotation = Quaternion.Euler(this._xRotation, this._yRotation, 0);
            this.OrientationTransform.rotation = Quaternion.Euler(0f, this._yRotation, 0f);
        }
    }
}