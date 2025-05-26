using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private LayerMask allowedBuildLayers;
        [SerializeField] private Vector2Int size = Vector2Int.one;
        public Vector2Int Size => size;
        public LayerMask AllowedBuildLayers => allowedBuildLayers;
        
        private void OnDrawGizmos()
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if ((x + y) % 2 == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
                    else Gizmos.color = new Color(1f, 0.68f, 0f, 0.3f);

                    Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
                }
            }
        }
    }
}