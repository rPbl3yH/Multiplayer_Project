using System;
using System.Collections.Generic;
using Game.Multiplayer;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Core
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private float _mouseSensitivity = 2f;
        private bool _isEnable;

        private bool _isEnableCursor;

        private void Awake()
        {
            _isEnableCursor = false;
            UpdateCursorState();
        }

        private void UpdateCursorState()
        {
            Cursor.lockState = _isEnableCursor ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isEnableCursor = !_isEnableCursor;
                UpdateCursorState();
            }
            
            if (!_isEnable)
            {
                return;
            }
            
            var horizontalAxis = Input.GetAxisRaw("Horizontal");
            var verticalAxis = Input.GetAxisRaw("Vertical");

            var direction = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;
            
            var mouseX = 0f;
            var mouseY = 0f;
            var isShoot = false;

            if (!_isEnableCursor)
            {
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
                isShoot = Input.GetMouseButton(0);
            }
            
            _player.SetInput(direction, mouseX);
            _player.Rotate(-mouseY * _mouseSensitivity);
            
            var isJump = Input.GetKeyDown(KeyCode.Space);
            
            if (isJump)
            {
                _player.Jump();
            }

            if (isShoot && _player.TryShoot(out var shootInfo))
            {
                SendShootInfo(ref shootInfo);
            }
            
            SendMoveInfo();
        }

        private void SendShootInfo(ref ShootData shootData)
        {
            shootData.clientId = MultiplayerManager.Instance.GetClientId();
            var jsonData = JsonConvert.SerializeObject(shootData);
            MultiplayerManager.Instance.SendMessage("shoot", jsonData);
        }

        public void SendMoveInfo()
        {
            _player.GetMoveInfo(out var position, out var velocity, out var eulerX, out var eulerY);
            
            var moveData = new Dictionary<string, object>
            {
                {"pX", position.x},
                {"pY", position.y},
                {"pZ", position.z},
                {"vX", velocity.x},
                {"vY", velocity.y},
                {"vZ", velocity.z},
                {"rX", eulerX},
                {"rY", eulerY},
            };
            
            MultiplayerManager.Instance.SendMessage("move", moveData);
        }

        public void SendMoveInfo(Vector3 position, Vector3 eulerAngles)
        {
            var moveData = new Dictionary<string, object>
            {
                {"pX", position.x},
                {"pY", position.y},
                {"pZ", position.z},
                {"vX", 0f},
                {"vY", 0f},
                {"vZ", 0f},
                {"rX", 0f},
                {"rY", eulerAngles.y},
            };
            
            MultiplayerManager.Instance.SendMessage("move", moveData);
        }

        public void Deactivate()
        {
            _isEnable = false;
        }

        public void Activate()
        {
            _isEnable = true;
        }
    }
}