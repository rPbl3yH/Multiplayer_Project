using UnityEngine;

namespace Game.Core
{
    public class SkinComponent : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _renderers;

        public void SetMaterial(Material material)
        {
            foreach (var meshRenderer in _renderers)
            {
                meshRenderer.material = material;
            }
        }
    }
}