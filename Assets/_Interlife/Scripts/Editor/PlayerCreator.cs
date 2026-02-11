using UnityEngine;
using UnityEditor;
using Interlife.Player;

namespace Interlife.Editor
{
    public class PlayerCreator : EditorWindow
    {
        [MenuItem("Interlife/Create Player (Void)")]
        public static void CreateVoidPlayer()
        {
            // 1. Crear el GameObject principal
            GameObject voidPlayer = new GameObject("Void");
            voidPlayer.tag = "Player";
            
            // 2. Configurar Rigidbody2D
            Rigidbody2D rb = voidPlayer.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            // 3. Configurar BoxCollider2D
            BoxCollider2D collider = voidPlayer.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(0.8f, 1.8f); // Tamaño aproximado humanoide

            // 4. Crear el hijo GroundCheck
            GameObject groundCheck = new GameObject("GroundCheck");
            groundCheck.transform.SetParent(voidPlayer.transform);
            groundCheck.transform.localPosition = new Vector3(0, -0.9f, 0);

            // 5. Añadir y configurar VoidController
            VoidController controller = voidPlayer.AddComponent<VoidController>();
            
            // Usamos SerializedObject para asignar los campos privados Serialized
            SerializedObject so = new SerializedObject(controller);
            
            // Buscar el InputReader en el proyecto automáticamente
            string[] guids = AssetDatabase.FindAssets("t:InputReader");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                InputReader reader = AssetDatabase.LoadAssetAtPath<InputReader>(path);
                so.FindProperty("inputReader").objectReferenceValue = reader;
            }
            else
            {
                Debug.LogWarning("No se encontró ningún asset de tipo InputReader. Deberás asignarlo manualmente.");
            }

            so.FindProperty("groundCheckTransform").objectReferenceValue = groundCheck.transform;
            so.ApplyModifiedProperties();

            // 6. Escena y Feedback
            Selection.activeGameObject = voidPlayer;
            Undo.RegisterCreatedObjectUndo(voidPlayer, "Create Void Player");
            
            Debug.Log("<color=green>🤖 Robot: Void Player creado y configurado con éxito.</color>");
        }
    }
}
