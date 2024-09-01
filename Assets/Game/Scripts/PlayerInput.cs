using System.Collections.Generic;
using Game.Multiplayer;
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _player.Jump();
            }

            if (Input.GetMouseButton(0))
            {
                _player.Shoot();
            }
            
            SendMoveInfo();
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
}