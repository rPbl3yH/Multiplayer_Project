using System;
using System.Collections.Generic;
using System.Linq;
using Colyseus.Schema;
using UnityEngine;

namespace Game.Core
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;

        private List<float> _receivedTimeIntervals = new(4) {0, 0, 0, 0};
        private float _lastTime;

        private void SaveReceivedTime()
        {
            var interval = Time.time - _lastTime;
            _lastTime = Time.time;
            
            _receivedTimeIntervals.Add(interval);
            _receivedTimeIntervals.RemoveAt(0);
        }
        
        public void OnStateChanged(List<DataChange> changes)
        {
            SaveReceivedTime();
            
            var position = _enemy.TargetPosition;
            var velocity = Vector3.zero;
            
            foreach (DataChange dataChange in changes)
            {
                switch (dataChange.Field)
                {
                    case "pX":
                        position.x = (float)dataChange.Value;
                        break;
                    case "pY":
                        position.y = (float)dataChange.Value;
                        break;
                    case "pZ":
                        position.z = (float)dataChange.Value;
                        break;
                    case "vX":
                        velocity.x = (float)dataChange.Value;
                        break;
                    case "vY":
                        velocity.y = (float)dataChange.Value;
                        break;
                    case "vZ":
                        velocity.z = (float)dataChange.Value;
                        break;
                    default:
                        Debug.Log($"No handler for data {dataChange.Field}");
                        break;
                }
            }

            var averageInterval = GetAverage(_receivedTimeIntervals);
            _enemy.SetMovementData(position, velocity, averageInterval);
        }

        private float GetAverage(IReadOnlyCollection<float> list)
        {
            if (list.Count == 0)
            {
                return 0;
            }
            
            var allValue = list.Sum();

            return allValue / list.Count;
        }
    }
}