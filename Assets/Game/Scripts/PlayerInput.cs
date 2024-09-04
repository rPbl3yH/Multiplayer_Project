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

        private void Update()
        {
            if (!_isEnable)
            {
                return;
            }
            
            var horizontalAxis = Input.GetAxisRaw("Horizontal");
            var verticalAxis = Input.GetAxisRaw("Vertical");

            var direction = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;
            
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            _player.SetInput(direction, mouseX);
            _player.Rotate(-mouseY * _mouseSensitivity);
            
            var isJump = Input.GetKeyDown(KeyCode.Space);
            var isShoot = Input.GetMouseButton(0);
            
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

        public void SendMoveInfo(float x, float z)
        {
            var moveData = new Dictionary<string, object>
            {
                {"pX", x},
                {"pY", 0f},
                {"pZ", z},
                {"vX", 0f},
                {"vY", 0f},
                {"vZ", 0f},
                {"rX", 0f},
                {"rY", 0f},
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