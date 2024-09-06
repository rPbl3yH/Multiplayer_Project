using UnityEngine;

namespace Game.Core
{
    public class SpawnPointsService : MonoBehaviour
    {
        public int Length => _spawnPoints.Length;
        
        [SerializeField] private Transform[] _spawnPoints;

        public void GetPoint(int index, out Vector3 position, out Vector3 eulerAngles)
        {
            position = Vector3.zero;
            eulerAngles = Vector3.zero;
            
            if (index >= _spawnPoints.Length)
            {
                return;
            }

            position = _spawnPoints[index].position;
            eulerAngles = _spawnPoints[index].eulerAngles;
        }
    }
}