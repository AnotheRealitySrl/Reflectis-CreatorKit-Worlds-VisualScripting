using Reflectis.CreatorKit.Worlds.Placeholders;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

using UnityEngine;

namespace Reflectis.CreatorKit.Worlds.CoreEditor
{
    public class CreatorKitUpgradeWindow : EditorWindow
    {
        private readonly List<KeyValuePair<string, string>> stringsToReplace = new()
        {
            new KeyValuePair<string, string>("Reflectis.PLG.Graphs", "Reflectis.SDK.Graphs"),
            new KeyValuePair<string, string>("Reflectis.PLG.Tasks", "Reflectis.SDK.Tasks"),
            new KeyValuePair<string, string>("Reflectis.PLG.TasksReflectis", "Reflectis.CreatorKit.Worlds.Tasks"),
            new KeyValuePair<string, string>("Reflectis.SDK.CreatorKit", "Reflectis.CreatorKit.Worlds.VisualScripting"),
            new KeyValuePair<string, string>("Reflectis.SDK.InteractionNew", "Reflectis.CreatorKit.Worlds.VisualScripting"),
            new KeyValuePair<string, string>("GenericInteract", "VisualScriptingInteract"),
        };

        private readonly List<string> extensions = new()
        {
            ".unity", ".prefab", ".asset"
        };

        [MenuItem("Reflectis/CK upgrade window")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(CreatorKitUpgradeWindow));
        }

        private void OnGUI()
        {
            GUIStyle style = new(EditorStyles.label)
            {
                richText = true,
            };
            if (GUILayout.Button("Migrate Reflectis version 2024.9.x -> 2025.1.0"))
            {
                try
                {
                    List<string> filePaths = new();
                    foreach (var extension in extensions)
                    {
                        filePaths.AddRange(Directory.GetFiles("Assets", $"*{extension}*", SearchOption.AllDirectories));
                    }

                    foreach (string file in filePaths)
                    {
                        // Leggi tutto il contenuto del file
                        string fileContent = File.ReadAllText(file);

                        foreach (KeyValuePair<string, string> stringToFind in stringsToReplace)
                        {
                            fileContent = fileContent.Replace(stringToFind.Key, stringToFind.Value);
                            Debug.Log($"Replaced {stringToFind.Key} -> {stringToFind.Value} in: {file}");
                        }
                        File.WriteAllText(file, fileContent);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            if (GUILayout.Button("Migrate Reflectis version 2025.1.x -> 2025.2.0"))
            {
                ReplaceInteractablePlaceholder();
            }
        }

        private void ReplaceInteractablePlaceholder()
        {
            string activeScenePath = "" + UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path;
            // Trova e sostituisci i componenti nei prefabs
            string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab");
            foreach (string prefabPathGuid in prefabPaths)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabPathGuid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (ReplaceComponentsInPrefab(prefab))
                {
                    PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
                }
            }


            // Trova tutte le scene nel progetto
            string[] scenePaths = AssetDatabase.FindAssets("t:Scene");
            foreach (string scenePathGuid in scenePaths)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(scenePathGuid);
                if (!scenePath.StartsWith("Packages/"))
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);

                    // Trova e sostituisci i componenti nella scena
                    if (ReplaceComponentsInScene())
                    {
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
                    }
                }
            }

            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(activeScenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);

            EditorUtility.DisplayDialog("Success", "Components replaced successfully!", "OK");
        }

        private bool ReplaceComponentsInScene()
        {
            GameObject[] gameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            bool modified = false;
            foreach (GameObject gameObject in gameObjects)
            {
                var isModified = ReplaceComponentRecursive(gameObject);
                modified = modified || isModified;
            }
            return modified;
        }

        private bool ReplaceComponentsInPrefab(GameObject prefab)
        {
            var replaced = ReplaceComponentRecursive(prefab);

            return replaced;
        }

        private bool ReplaceComponentRecursive(GameObject gameObject)
        {

            InteractablePlaceholderObsolete[] interactables = gameObject.GetComponents<InteractablePlaceholderObsolete>();
            bool modified = false;
            foreach (var interactable in interactables)
            {
                if (interactable != null)
                {
                    ReplaceInteractablePlaceholder(interactable);
                }
                modified = true;
            }
            foreach (Transform child in gameObject.transform)
            {
                var isModified = ReplaceComponentRecursive(child.gameObject);
                modified = modified || isModified;
            }
            return modified;
        }

        private void ReplaceInteractablePlaceholder(InteractablePlaceholderObsolete interactable)
        {
            Debug.Log($"Replacing component in {interactable.gameObject.name} in scene {interactable.gameObject.scene.name}", interactable.gameObject);

            UnityEditor.Undo.RecordObject(interactable.gameObject, "Replace Component");

            InteractionPlaceholder interactionPlaceholder = interactable.gameObject.AddComponent<InteractionPlaceholder>();

            interactionPlaceholder.LockHoverDuringInteraction = interactable.LockHoverDuringInteraction;
            interactionPlaceholder.InteractionColliders = interactable.InteractionColliders;
            EditorUtility.SetDirty(interactionPlaceholder.gameObject);

            if (interactable.InteractionModes.HasFlag(Core.Interaction.IInteractable.EInteractableType.ContextualMenuInteractable))
            {
                ContextualMenuPlaceholder contextualMenuPlaceholder = interactable.gameObject.AddComponent<ContextualMenuPlaceholder>();
                contextualMenuPlaceholder.ContextualMenuOptions = interactable.ContextualMenuOptions;
                EditorUtility.SetDirty(contextualMenuPlaceholder.gameObject);
            }

            if (interactable.InteractionModes.HasFlag(Core.Interaction.IInteractable.EInteractableType.Manipulable))
            {
                ManipulablePlaceholder manipulablePlaceholder = interactable.gameObject.AddComponent<ManipulablePlaceholder>();
                manipulablePlaceholder.ManipulationMode = interactable.ManipulationMode;
                manipulablePlaceholder.DynamicAttach = interactable.DynamicAttach;
                manipulablePlaceholder.AdjustRotationOnRelease = interactable.AdjustRotationOnRelease;
                manipulablePlaceholder.MouseLookAtCamera = interactable.MouseLookAtCamera;
                manipulablePlaceholder.RealignAxisX = interactable.RealignAxisX;
                manipulablePlaceholder.RealignAxisY = interactable.RealignAxisY;
                manipulablePlaceholder.RealignAxisZ = interactable.RealignAxisZ;
                manipulablePlaceholder.RealignDurationTimeInSeconds = interactable.RealignDurationTimeInSeconds;
                manipulablePlaceholder.VrInteraction = interactable.VRInteraction;
                manipulablePlaceholder.AttachTransform = interactable.AttachTransform;
                EditorUtility.SetDirty(manipulablePlaceholder.gameObject);
            }

            if (interactable.InteractionModes.HasFlag(Core.Interaction.IInteractable.EInteractableType.VisualScriptingInteractable))
            {
                VisualScriptingInteractablePlaceholder vsPlaceholder = interactable.gameObject.AddComponent<VisualScriptingInteractablePlaceholder>();
                vsPlaceholder.DesktopAllowedStates = interactable.DesktopAllowedStates;
                vsPlaceholder.VRAllowedStates = interactable.VRAllowedStates;
                vsPlaceholder.InteractionsScriptMachine = interactable.InteractionsScriptMachine;
                vsPlaceholder.VrVisualScriptingInteraction = interactable.VrVisualScriptingInteraction;
                EditorUtility.SetDirty(vsPlaceholder.gameObject);
            }

            EditorUtility.SetDirty(interactionPlaceholder.gameObject);

            DestroyImmediate(interactable, true);
        }
    }
}
