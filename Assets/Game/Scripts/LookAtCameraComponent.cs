using UnityEngine;

namespace Game.Core
{
    public class LookAtCameraComponent : MonoBehaviour
    {
        private Transform _camera;

        private void Start()
        {
            _camera = Camera.main.transform;
        }

        public void Update()
        {
            transform.LookAt(_camera);
        }
    }
}