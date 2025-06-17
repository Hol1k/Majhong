using UnityEngine;

namespace Game.Field.Scripts
{
    public class BackgroundSizeAdapter : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        private void Start()
        {
            transform.localScale = new Vector3(camera.pixelWidth / 1024f, camera.pixelHeight / 1024f, 1);
        }
    }
}
