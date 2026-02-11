using UnityEngine;
using Interlife.Core;

namespace Interlife.Environment
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private Color inactiveColor = Color.gray;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool isActive = false;

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            UpdateVisuals();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !isActive)
            {
                ActivateCheckpoint();
            }
        }

        private void ActivateCheckpoint()
        {
            isActive = true;
            LevelManager.Instance.SetCheckpoint(transform.position);
            UpdateVisuals();
            Debug.Log("Checkpoint Activated!");
        }

        private void UpdateVisuals()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = isActive ? activeColor : inactiveColor;
            }
        }
    }
}
