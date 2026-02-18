using UnityEngine;

namespace Interlife.Core
{
    /// <summary>
    /// Manager local de cada nivel para gestionar progreso y respawn.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Level Data")]
        [SerializeField] private string levelName = "Umbral_01";
        
        private int fragmentsCollected = 0;
        private Vector3 currentCheckpointPos;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        /// <summary>
        /// Actualiza la posición de reaparición del jugador.
        /// </summary>
        public void SetCheckpoint(Vector3 newPos)
        {
            currentCheckpointPos = newPos;
            Debug.Log($"<color=green>Checkpoint Saved: {newPos}</color>");
        }

        /// <summary>
        /// Suma un fragmento al contador del nivel.
        /// </summary>
        public void CollectFragment()
        {
            fragmentsCollected++;
            Debug.Log($"<color=yellow>Fragments: {fragmentsCollected}/3</color>");
        }

        /// <summary>
        /// Devuelve al jugador al último checkpoint.
        /// </summary>
        public void RespawnPlayer(Transform player)
        {
            player.position = currentCheckpointPos;
            // Aquí activaríamos el efecto visual de disolución más adelante
            Debug.Log("<color=red>Player Respawned</color>");
        }
    }
}
