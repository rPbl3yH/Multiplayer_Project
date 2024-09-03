using System.Collections.Generic;
using Colyseus;
using Game.Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private Core.Player _player;
        [SerializeField] private EnemyController _enemy;
        
        private ColyseusRoom<State> _room;
        
        private Dictionary<string, EnemyController> _enemies = new();

        protected override void Awake()
        {
            base.Awake();
            Instance.InitializeClient();
            Connect();
        }

        private async void Connect()
        {
            var options = new Dictionary<string, object>()
            {
                {"speed", _player.Speed},
                {"maxHp", _player.MaxHealth}
            };
            _room = await Instance.client.JoinOrCreate<State>("state_handler", options);
            _room.OnStateChange += OnRoomStateChanged;
            
            _room.OnMessage<string>("Shoot", ShootHandler);
        }

        private void ShootHandler(string shotInfoJson)
        {
            var shootInfo = JsonConvert.DeserializeObject<ShootInfo>(shotInfoJson);
            var clientKey = shootInfo.clientId;

            if (_enemies.TryGetValue(clientKey, out var enemyController))
            {
                enemyController.Shoot(shootInfo);
            }
            else
            {
                Debug.LogError("Handle not exist enemy ");
            }

        }

        private void OnRoomStateChanged(State state, bool isFirstState)
        {
            if (!isFirstState)
            {
                return;
            }

            state.players.ForEach(HandleServerPlayer);

            _room.State.players.OnAdd += CreateEnemy;
            _room.State.players.OnRemove += RemoveEnemy;
        }

        private void HandleServerPlayer(string key, Player serverPlayer)
        {
            if (key == _room.SessionId)
            {
                CreatePlayer(serverPlayer);
            }
            else
            {
                CreateEnemy(key, serverPlayer);
            }
        }

        private void CreatePlayer(Player serverPlayer)
        {
            var position = new Vector3(serverPlayer.pX, serverPlayer.pY, serverPlayer.pZ);
            var player = Instantiate(_player, position, Quaternion.identity);

            serverPlayer.OnChange += player.OnChange;
        }

        private void CreateEnemy(string key, Player serverPlayer)
        {
            var position = new Vector3(serverPlayer.pX, serverPlayer.pY, serverPlayer.pZ);
            var enemyController = Instantiate(_enemy, position, Quaternion.identity);

            enemyController.Construct(key, serverPlayer);

            _enemies.Add(key, enemyController);
        }

        private void RemoveEnemy(string key, Player value)
        {
            if (!_enemies.TryGetValue(key, out var enemyController)) 
                return;
            
            enemyController.Destroy();
            _enemies.Remove(key);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _room.Leave();
        }

        public void SendMessage(string keyEvent, Dictionary<string, object> data)
        {
            _room.Send(keyEvent, data);
        }

        public void SendMessage(string keyEvent, string data)
        {
            _room.Send(keyEvent, data);
        }

        public string GetClientId()
        {
            return _room.SessionId;
        }
    }
}
