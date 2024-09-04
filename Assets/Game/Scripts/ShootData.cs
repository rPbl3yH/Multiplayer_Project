using System;

namespace Game.Core
{
    [Serializable]
    public struct ShootData
    {
        public string clientId;
        public float pX;
        public float pY;
        public float pZ;

        public float dX;
        public float dY;
        public float dZ;
    }
}