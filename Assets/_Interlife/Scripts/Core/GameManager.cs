using UnityEngine;

namespace Interlife.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState
        {
            Menu,
            Playing,
            Paused,
            GameOver,
            LevelCompleted
        }

        public GameState CurrentState { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void UpdateState(GameState newState)
        {
            CurrentState = newState;
            Debug.Log($"Game State changed to: {newState}");
            
            // Eventos globales para cambios de estado pueden ser añadidos aquí
        }
    }
}
