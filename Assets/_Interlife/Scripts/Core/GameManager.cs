using UnityEngine;

namespace Interlife.Core
{
    /// <summary>
    /// Manager global para controlar el flujo de estados del juego (PC/Mobile).
    /// </summary>
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

        [Header("Settings")]
        public GameState CurrentState { get; private set; } = GameState.Playing;

        private void Awake()
        {
            // Singleton profesional
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            Debug.Log("<color=cyan>GameManager Initialized</color>");
        }

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
            
            // Aquí puedes activar eventos de UI, pausar el tiempo, etc.
            switch (newState)
            {
                case GameState.Paused:
                    Time.timeScale = 0;
                    break;
                case GameState.Playing:
                    Time.timeScale = 1;
                    break;
            }

            Debug.Log($"<color=yellow>Game State changed to: {newState}</color>");
        }
    }
}
