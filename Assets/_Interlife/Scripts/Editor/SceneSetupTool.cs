using UnityEngine;
using UnityEditor;
using Interlife.Core;
using Interlife.Environment;
using Interlife.Player;

namespace Interlife.Editor
{
    public class SceneSetupTool : EditorWindow
    {
        [MenuItem("Interlife/Setup Test Scene")]
        public static void SetupTestScene()
        {
            // 1. Crear Managers
            GameObject managers = new GameObject("--- MANAGERS ---");
            managers.AddComponent<LevelManager>();

            // 2. Crear Entorno (Suelo)
            GameObject environment = new GameObject("--- ENVIRONMENT ---");
            
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Floor";
            ground.transform.SetParent(environment.transform);
            ground.transform.localScale = new Vector3(20, 1, 1);
            ground.transform.position = new Vector3(0, -2, 0);
            DestroyImmediate(ground.GetComponent<BoxCollider>()); // Quitar 3D
            ground.AddComponent<BoxCollider2D>();
            ground.GetComponent<MeshRenderer>().material.color = Color.gray;

            // 3. Crear Jugador (usando el creator existente)
            PlayerCreator.CreateVoidPlayer();
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) player.transform.position = new Vector3(0, 0, 0);

            // 4. Crear Elementos Interactivos
            GameObject interactives = new GameObject("--- INTERACTIVES ---");

            // Lava (Hazard)
            GameObject hazard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hazard.name = "Hazard (Lava)";
            hazard.transform.SetParent(interactives.transform);
            hazard.transform.localScale = new Vector3(5, 0.5f, 1);
            hazard.transform.position = new Vector3(8, -1.75f, 0);
            DestroyImmediate(hazard.GetComponent<BoxCollider>());
            hazard.AddComponent<BoxCollider2D>().isTrigger = true;
            hazard.AddComponent<Hazard>();
            hazard.GetComponent<MeshRenderer>().material.color = Color.red;

            // Checkpoint
            GameObject checkpointObj = new GameObject("Checkpoint");
            checkpointObj.transform.SetParent(interactives.transform);
            checkpointObj.transform.position = new Vector3(5, -1, 0);
            checkpointObj.AddComponent<BoxCollider2D>().isTrigger = true;
            Checkpoint cp = checkpointObj.AddComponent<Checkpoint>();
            
            // Fragmento
            GameObject fragmentObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            fragmentObj.name = "Fragment";
            fragmentObj.transform.SetParent(interactives.transform);
            fragmentObj.transform.position = new Vector3(10, 0, 0);
            fragmentObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            DestroyImmediate(fragmentObj.GetComponent<SphereCollider>());
            fragmentObj.AddComponent<CircleCollider2D>().isTrigger = true;
            fragmentObj.AddComponent<Fragment>();
            fragmentObj.GetComponent<MeshRenderer>().material.color = Color.yellow;

            // 5. Configurar Cámara
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                mainCam.transform.position = new Vector3(0, 1, -10);
                mainCam.orthographic = true;
                mainCam.orthographicSize = 7;
            }

            Debug.Log("<color=cyan>✨ Robot: Escena de prueba montada con éxito.</color>");
        }
    }
}
