using System.Collections.Generic;
using Colyseus;
using Game.Core;
using UnityEngine;

namespace Game.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private Core.Player _player;
        [SerializeField] private EnemyController _enemy;
        
        private ColyseusRoom<State> _room;
        
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
            };
            _room = await Instance.client.JoinOrCreate<State>("state_handler", options);
            _room.OnStateChange += OnRoomStateChanged;
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
            Instantiate(_player, position, Quaternion.identity);
        }

        private void CreateEnemy(string key, Player serverPlayer)
        {
            var position = new Vector3(serverPlayer.pX, serverPlayer.pY, serverPlayer.pZ);
            var enemyController = Instantiate(_enemy, position, Quaternion.identity);

            enemyController.Construct(serverPlayer);
        }

        private void RemoveEnemy(string key, Player value)
        {
            //TODO:
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
    }
}
