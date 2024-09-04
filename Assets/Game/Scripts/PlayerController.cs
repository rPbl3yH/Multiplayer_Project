using System.Collections;
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

        public void OnRestart(string jsonRestartData)
        {
            Deactivate();
            StartCoroutine(DelayCoroutine());
            
            _player.SetInput(Vector3.zero, 0);
            _player.ResetVelocity();
            
            var data = JsonConvert.DeserializeObject<RestartData>(jsonRestartData);
            transform.position = new Vector3(data.x, 0f, data.z);
            
            _playerInput.SendMoveInfo(data.x, data.z);
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