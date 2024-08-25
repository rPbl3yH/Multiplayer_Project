using System.Collections.Generic;
using Game.Multiplayer;
using UnityEngine;

namespace Game.Core
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Player _player;
        
        private void Update()
        {
            var horizontalAxis = Input.GetAxisRaw("Horizontal");
            var verticalAxis = Input.GetAxisRaw("Vertical");

            var direction = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;
            
            _player.SetupMoveDirection(direction);
            
            SendMoveInfo();
        }

        public void SendMoveInfo()
        {
            var vector = _player.GetMoveInfo();
            
            var moveData = new Dictionary<string, object>
            {
                {"x", vector.x},
                {"y", vector.z}
            };
            
            MultiplayerManager.Instance.SendMessage("move", moveData);
        }
    }
}