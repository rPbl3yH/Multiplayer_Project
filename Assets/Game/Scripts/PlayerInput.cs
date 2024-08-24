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
        }
    }
}