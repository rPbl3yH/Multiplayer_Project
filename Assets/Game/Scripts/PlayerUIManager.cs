using System;
using UnityEngine;

namespace Game.Core
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instance;
        
        public ScoreView ScoreView;

        public void Awake()
        {
            Instance = this;
        }
    }
}