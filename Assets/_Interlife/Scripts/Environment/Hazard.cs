using UnityEngine;
using Interlife.Core;

namespace Interlife.Environment
{
    public class Hazard : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Player hit hazard!");
                LevelManager.Instance.RespawnPlayer(collision.transform);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("Player hit hazard!");
                LevelManager.Instance.RespawnPlayer(collision.transform);
            }
        }
    }
}
