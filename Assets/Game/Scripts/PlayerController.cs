using System.Collections;
using Game.Multiplayer;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _restartDelay = 3f;

        private void Awake()
        {
            _player.Construct(this);
            Activate();
        }

        public void OnRestart(int spawnIndex)
        {
            Deactivate();
            StartCoroutine(DelayCoroutine());
            
            _player.SetInput(Vector3.zero, 0);
            _player.ResetVelocity();
            
            MultiplayerManager.Instance.SpawnPointsService.GetPoint(spawnIndex, out var position, out var rotation);

            transform.position = position;
            rotation.x = 0f;
            rotation.z = 0f;
            transform.rotation = Quaternion.Euler(rotation);
            
            _playerInput.SendMoveInfo(position, rotation);
        }

        private IEnumerator DelayCoroutine()
        {
            yield return new WaitForSecondsRealtime(_restartDelay);
            Activate();
        }

        private void Activate()
        {
            _playerInput.Activate();
        }

        private void Deactivate()
        {
            _playerInput.Deactivate();
        }
    }
}