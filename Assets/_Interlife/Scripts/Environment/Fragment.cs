using UnityEngine;
using Interlife.Core;

namespace Interlife.Environment
{
    public class Fragment : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject collectEffect;

        public void Interact()
        {
            LevelManager.Instance.CollectFragment();
            
            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Interact();
            }
        }
    }
}
