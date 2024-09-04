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
        [SerializeField] private EnemyGun _enemyGun;

        private List<float> _receivedTimeIntervals = new(4) {0, 0, 0, 0};
        private float _lastTime;

        private global::Player _serverPlayer;

        public void Construct(string sessionId, global::Player serverPlayer)
        {
            _serverPlayer = serverPlayer;
            _enemy.Construct(sessionId);
            _enemy.SetSpeed(_serverPlayer.speed);
            _enemy.SetMaxHealth(_serverPlayer.maxHp);

            _serverPlayer.OnChange += OnStateChanged;
        }

        public void Destroy()
        {
            _serverPlayer.OnChange -= OnStateChanged;
            Destroy(gameObject);
        }

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
            var velocity = _enemy.Velocity;
            
            foreach (DataChange dataChange in changes)
            {
                switch (dataChange.Field)
                {
                    case "loss":
                        var playerLose = (ushort)dataChange.Value;
                        PlayerUIManager.Instance.ScoreView.SetEnemyScore(playerLose);
                        break;
                    case "hp":
                        if ((short)dataChange.Value > (short)dataChange.PreviousValue)
                        {
                            _enemy.SetMaxHealth((short)dataChange.Value);
                        }
                        break;
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
                    case "rX":
                        _enemy.SetRotateX((float)dataChange.Value);
                        break;
                    case "rY":
                        _enemy.SetRotateY((float)dataChange.Value);
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

        public void Shoot(in ShootData shootData)
        {
            var shootPosition = new Vector3(shootData.pX, shootData.pY, shootData.pZ);
            var shootVelocity = new Vector3(shootData.dX, shootData.dY, shootData.dZ);
            
            _enemyGun.Shoot(shootPosition, shootVelocity);
        }
    }
}