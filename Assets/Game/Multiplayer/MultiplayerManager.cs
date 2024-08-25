using Colyseus;
using UnityEngine;

namespace Game.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _enemy;
        
        private ColyseusRoom<State> _room;
        
        protected override void Awake()
        {
            base.Awake();
            Instance.InitializeClient();
            Connect();
        }

        private async void Connect()
        {
            _room = await Instance.client.JoinOrCreate<State>("state_handler");
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
            var position = new Vector3(serverPlayer.x, 0f, serverPlayer.y);
            Instantiate(_player, position, Quaternion.identity);
        }

        private void CreateEnemy(string key, Player serverPlayer)
        {
            var position = new Vector3(serverPlayer.x, 0f, serverPlayer.y);
            Instantiate(_enemy, position, Quaternion.identity);
        }

        private void RemoveEnemy(string key, Player value)
        {
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _room.Leave();
        }
    }
}
