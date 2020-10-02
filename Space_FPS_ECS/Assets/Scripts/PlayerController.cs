using UnityEngine;

namespace SpaceFpsEcs
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 1.5f;
        public float rotationMultiplier = 10f;
        private Vector2 rotation = new Vector2(0, 0);

        public bool lockCursor = true;
        private bool cursorIsLocked = true;
    
        private void Update()
        {
            UpdateMovementAndRotation();
            UpdatePlayerFire();
            UpdateCursorLock();
        }

        private void UpdateMovementAndRotation()
        {
            rotation.y += Input.GetAxis("Mouse X");
            rotation.x += -Input.GetAxis("Mouse Y");
            transform.eulerAngles = rotation * (speed * rotationMultiplier);

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            transform.Translate(x * speed, 0, z * speed);
        }

        private void UpdatePlayerFire()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnManager.Instance.SpawnShipBullet();
            }
        }

        private void UpdateCursorLock()
        {
            // if the user set "lockCursor" we check & properly lock the cursor
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                cursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                cursorIsLocked = true;
            }

            if (cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}