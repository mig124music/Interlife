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

            // 3b. Configurar SpriteRenderer (Para que sea visible)
            SpriteRenderer sr = voidPlayer.AddComponent<SpriteRenderer>();
            sr.color = new Color(0.1f, 0.1f, 0.1f, 1f); // Un color oscuro tipo "Void"
            
            // Intentar asignar un sprite por defecto de Unity
            sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            sr.sortingOrder = 10; // Asegurar que esté por delante del fondo

            // 4. Crear el hijo GroundCheck
            GameObject groundCheck = new GameObject("GroundCheck");
            groundCheck.transform.SetParent(voidPlayer.transform);
            groundCheck.transform.localPosition = new Vector3(0, -0.9f, 0);

            // 4b. Añadir GhostTrail
            GhostTrail gt = voidPlayer.AddComponent<GhostTrail>();

            // 4c. Crear WallChecks
            GameObject wallCheckL = new GameObject("WallCheck_L");
            wallCheckL.transform.SetParent(voidPlayer.transform);
            wallCheckL.transform.localPosition = new Vector3(-0.5f, 0, 0);

            GameObject wallCheckR = new GameObject("WallCheck_R");
            wallCheckR.transform.SetParent(voidPlayer.transform);
            wallCheckR.transform.localPosition = new Vector3(0.5f, 0, 0);

            // 5. Añadir y configurar VoidController
            VoidController controller = voidPlayer.AddComponent<VoidController>();
            
            // Usamos SerializedObject para asignar los campos privados Serialized
            SerializedObject so = new SerializedObject(controller);
            
            // Buscar el InputReader en el proyecto automáticamente
            string[] guids = AssetDatabase.FindAssets("t:InputReader");
            InputReader reader = null;

            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                reader = AssetDatabase.LoadAssetAtPath<InputReader>(path);
            }
            else
            {
                // Auto-crear el asset si no existe
                reader = ScriptableObject.CreateInstance<InputReader>();
                if (!AssetDatabase.IsValidFolder("Assets/_Interlife/Settings"))
                {
                    if (!AssetDatabase.IsValidFolder("Assets/_Interlife")) AssetDatabase.CreateFolder("Assets", "_Interlife");
                    AssetDatabase.CreateFolder("Assets/_Interlife", "Settings");
                }
                AssetDatabase.CreateAsset(reader, "Assets/_Interlife/Settings/InputReader.asset");
                AssetDatabase.SaveAssets();
                Debug.Log("<color=yellow>🤖 Robot: No se encontró InputReader, se ha creado uno nuevo en Assets/_Interlife/Settings/</color>");
            }

            if (reader != null)
            {
                so.FindProperty("inputReader").objectReferenceValue = reader;
            }

            so.FindProperty("spriteRenderer").objectReferenceValue = sr;
            so.FindProperty("ghostTrail").objectReferenceValue = gt;
            so.FindProperty("groundCheckTransform").objectReferenceValue = groundCheck.transform;
            so.FindProperty("wallCheckLeft").objectReferenceValue = wallCheckL.transform;
            so.FindProperty("wallCheckRight").objectReferenceValue = wallCheckR.transform;
            
            // Configurar capa de suelo por defecto a Everything o Default para asegurar que el test funciona
            // En un proyecto real, el usuario asignaría la capa "Ground"
            so.FindProperty("groundLayer").intValue = -1; // -1 es "Everything"

            so.ApplyModifiedProperties();

            // 6. Escena y Feedback
            Selection.activeGameObject = voidPlayer;
            Undo.RegisterCreatedObjectUndo(voidPlayer, "Create Void Player");
            
            Debug.Log("<color=green>🤖 Robot: Void Player creado y configurado con éxito.</color>");
        }

        [MenuItem("Interlife/Fix Selected Player")]
        public static void FixSelectedPlayer()
        {
            GameObject selected = Selection.activeGameObject;
            if (selected == null || !selected.CompareTag("Player"))
            {
                Debug.LogWarning("🤖 Robot: Por favor, selecciona un objeto con la etiqueta 'Player'.");
                return;
            }

            VoidController controller = selected.GetComponent<VoidController>();
            if (controller == null)
            {
                Debug.LogWarning("🤖 Robot: El objeto seleccionado no tiene un VoidController.");
                return;
            }

            Undo.RecordObject(selected, "Fix Player");
            
            // 1. Asegurar SpriteRenderer
            SpriteRenderer sr = selected.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                sr = selected.AddComponent<SpriteRenderer>();
                sr.color = new Color(0.1f, 0.1f, 0.1f, 1f);
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
                sr.sortingOrder = 10;
                Debug.Log("🤖 Robot: SpriteRenderer añadido.");
            }

            // 1b. Asegurar GhostTrail
            GhostTrail gt = selected.GetComponent<GhostTrail>();
            if (gt == null)
            {
                gt = selected.AddComponent<GhostTrail>();
                Debug.Log("🤖 Robot: GhostTrail añadido.");
            }

            // 2. Asegurar InputReader
            SerializedObject so = new SerializedObject(controller);
            SerializedProperty inputProp = so.FindProperty("inputReader");
            SerializedProperty spriteProp = so.FindProperty("spriteRenderer");
            SerializedProperty ghostProp = so.FindProperty("ghostTrail");

            if (inputProp.objectReferenceValue == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:InputReader");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    inputProp.objectReferenceValue = AssetDatabase.LoadAssetAtPath<InputReader>(path);
                    Debug.Log("🤖 Robot: InputReader asignado.");
                }
            }

            if (spriteProp.objectReferenceValue == null)
            {
                spriteProp.objectReferenceValue = sr;
            }

            if (ghostProp.objectReferenceValue == null)
            {
                ghostProp.objectReferenceValue = gt;
            }

            // 1c. Asegurar WallChecks
            SerializedProperty wallPropL = so.FindProperty("wallCheckLeft");
            SerializedProperty wallPropR = so.FindProperty("wallCheckRight");

            if (wallPropL.objectReferenceValue == null)
            {
                GameObject wl = new GameObject("WallCheck_L");
                wl.transform.SetParent(selected.transform);
                wl.transform.localPosition = new Vector3(-0.5f, 0, 0);
                wallPropL.objectReferenceValue = wl.transform;
            }

            if (wallPropR.objectReferenceValue == null)
            {
                GameObject wr = new GameObject("WallCheck_R");
                wr.transform.SetParent(selected.transform);
                wr.transform.localPosition = new Vector3(0.5f, 0, 0);
                wallPropR.objectReferenceValue = wr.transform;
            }

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(controller);
            AssetDatabase.SaveAssets();

            Debug.Log("<color=green>🤖 Robot: Jugador reparado con éxito.</color>");
        }
    }
}
