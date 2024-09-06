using UnityEngine;

namespace Game.Core
{
    public class SkinService : MonoBehaviour
    {
        public int Length => _skins.Length;
        
        [SerializeField] private Material[] _skins;

        public void GetSkin(int index, out Material material)
        {
            material = null;
            
            if (index >= _skins.Length)
            {
                return;
            }

            material = _skins[index];
        }
    }
}