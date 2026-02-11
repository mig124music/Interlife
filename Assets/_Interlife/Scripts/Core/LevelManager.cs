using UnityEngine;

namespace Interlife.Core
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Level Data")]
        [SerializeField] private string levelName;
        private int fragmentsCollected = 0;
        private Vector3 lastCheckpointPosition;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void SetCheckpoint(Vector3 position)
        {
            lastCheckpointPosition = position;
            Debug.Log("Checkpoint saved at: " + position);
        }

        public void CollectFragment()
        {
            fragmentsCollected++;
            Debug.Log($"Fragments collected: {fragmentsCollected}/3");
        }

        public void RespawnPlayer(Transform playerTransform)
        {
            playerTransform.position = lastCheckpointPosition;
            Debug.Log("Player respawned at last checkpoint.");
        }
    }
}
