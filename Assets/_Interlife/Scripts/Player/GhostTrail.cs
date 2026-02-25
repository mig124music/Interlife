using UnityEngine;
using System.Collections;

namespace Interlife.Player
{
    public class GhostTrail : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float ghostDelay = 0.05f;
        [SerializeField] private float ghostLifetime = 0.3f;
        [SerializeField] private Color ghostColor = new Color(0.5f, 0f, 1f, 0.5f);
        
        private SpriteRenderer sr;
        private bool isSpawning = false;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void StartTrail()
        {
            if (isSpawning) return;
            isSpawning = true;
            StartCoroutine(SpawnGhostsRoutine());
        }

        public void StopTrail()
        {
            isSpawning = false;
        }

        private IEnumerator SpawnGhostsRoutine()
        {
            while (isSpawning)
            {
                GameObject ghost = new GameObject("ShadowGhost");
                ghost.transform.position = transform.position;
                ghost.transform.rotation = transform.rotation;
                ghost.transform.localScale = transform.localScale;

                SpriteRenderer ghostSR = ghost.AddComponent<SpriteRenderer>();
                ghostSR.sprite = sr.sprite;
                ghostSR.color = ghostColor;
                ghostSR.flipX = sr.flipX;
                ghostSR.sortingOrder = sr.sortingOrder - 1;

                Destroy(ghost, ghostLifetime);
                yield return new WaitForSeconds(ghostDelay);
            }
        }
    }
}
