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
        
        private void Update()
        {
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

        private void SendShootInfo(ref ShootInfo shootInfo)
        {
            shootInfo.clientId = MultiplayerManager.Instance.GetClientId();
            var jsonData = JsonConvert.SerializeObject(shootInfo);
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
    }

    public struct ShootInfo
    {
        public string clientId;
        public float pX;
        public float pY;
        public float pZ;

        public float dX;
        public float dY;
        public float dZ;
    }
}