using TMPro;
using UnityEngine;

namespace Game.Core
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        private int _playerScore;
        private int _enemyScore;
        
        public void SetEnemyScore(int score)
        {
            _enemyScore = score;
            UpdateText();
        }

        public void SetPlayerScore(int score)
        {
            _playerScore = score;
            UpdateText();
        }

        private void UpdateText()
        {
            _scoreText.SetText($"{_playerScore} : {_enemyScore}");
        }
    }
}