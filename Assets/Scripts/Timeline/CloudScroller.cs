using System.Collections;
using UnityEngine;

namespace Timeline
{
    public class CloudController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float spawnX;
        [SerializeField] private float spawnDelay;

        private Renderer _renderer;
        private float _timer = 0;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            transform.position = new Vector3(spawnX, transform.position.y, transform.position.z);
        }

        private void Update()
        {
            transform.Translate(Vector3.left * (speed * Time.deltaTime), Space.World);
    
            if (_renderer.isVisible) return;
        
            _timer += Time.deltaTime;
            if (!(_timer >= spawnDelay)) return;
        
            _timer = 0;
            transform.position = new Vector3(spawnX, transform.position.y, transform.position.z);
        }
    }
}
