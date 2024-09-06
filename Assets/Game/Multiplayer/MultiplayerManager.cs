using System.Collections.Generic;
using Colyseus;
using Game.Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        public SpawnPointsService SpawnPointsService;

        [SerializeField] private Core.Player _player;
        [SerializeField] private EnemyController _enemy;
        [SerializeField] private SkinService _skinService;

        private ColyseusRoom<State> _room;
        
        private readonly Dictionary<string, EnemyController> _enemies = new();

        protected override void Awake()
        {
            base.Awake();
            Instance.InitializeClient();
            Connect();
        }

        private async void Connect()
        {
            SpawnPointsService.GetPoint(Random.Range(0, SpawnPointsService.Length), 
                out var position, out var rotation);
            
            var options = new Dictionary<string, object>()
            {
                {"skins", _skinService.Length},
                {"pointsLength", SpawnPointsService.Length},
                {"speed", _player.Speed},
                {"maxHp", _player.MaxHealth},
                {"pX", position.x},
                {"pY", position.y},
                {"pZ", position.z},
                {"rY", rotation.y},
            };
            _room = await Instance.client.JoinOrCreate<State>("state_handler", options);
            _room.OnStateChange += OnRoomStateChanged;
            
            _room.OnMessage<string>("Shoot", ShootHandler);
        }

        private void ShootHandler(string shotInfoJson)
        {
            var shootData = JsonConvert.DeserializeObject<ShootData>(shotInfoJson);
            var clientKey = shootData.clientId;

            if (_enemies.TryGetValue(clientKey, out var enemyController))
            {
                enemyController.Shoot(shootData);
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
            var rotation = Quaternion.Euler(0, serverPlayer.rY, 0);
            var player = Instantiate(_player, position, rotation);

            SetupSkin(serverPlayer.skinIndex, player);

            serverPlayer.OnChange += player.OnChange;

            _room.OnMessage<int>("Restart", player.PlayerController.OnRestart);
        }

        private void CreateEnemy(string key, Player serverPlayer)
        {
            var position = new Vector3(serverPlayer.pX, serverPlayer.pY, serverPlayer.pZ);
            var rotation = Quaternion.Euler(serverPlayer.rX, serverPlayer.rY, 0);
            var enemyController = Instantiate(_enemy, position, rotation);

            enemyController.Construct(key, serverPlayer);
            SetupSkin(serverPlayer.skinIndex, enemyController.Enemy);

            _enemies.Add(key, enemyController);
        }

        private void SetupSkin(int index, Character entity)
        {
            _skinService.GetSkin(index, out var skinMaterial);
            entity.SkinComponent.SetMaterial(skinMaterial);
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
