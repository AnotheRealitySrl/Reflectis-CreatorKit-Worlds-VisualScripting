using Reflectis.CreatorKit.Worlds.Placeholders;
using Reflectis.CreatorKit.Worlds.Tasks;
using Reflectis.SDK.Core.Utilities;
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

            foreach (string prefabPathGuid in prefabPaths)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabPathGuid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (FixDetector(prefab))
                {
                    PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
                }
            }

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

            foreach (string scenePathGuid in scenePaths)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(scenePathGuid);
                if (!scenePath.StartsWith("Packages/"))
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);

                    if (FixDetectorsInScene())
                    {
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
                    }
                }
            }

            foreach (string prefabPathGuid in prefabPaths)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabPathGuid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (DestroyOldComponentInPrefab(prefab))
                {
                    PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
                }
            }

            foreach (string scenePathGuid in scenePaths)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(scenePathGuid);
                if (!scenePath.StartsWith("Packages/"))
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);

                    // Trova e sostituisci i componenti nella scena
                    if (DestroyOldComponentInScene())
                    {
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
                    }
                }
            }

            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(activeScenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);

            EditorUtility.DisplayDialog("Success", "Components replaced successfully!", "OK");
        }

        private bool FixDetector(GameObject prefab)
        {
            var grabs = prefab.GetComponentsInChildren<ManipulableGrabberDetector>(true);
            var hovers = prefab.GetComponentsInChildren<VisualScriptingInteractableHoverDetector>(true);
            foreach (var grab in grabs)
            {
                if (grab.interactablePlaceholder != null)
                {
                    grab.manipulablePlaceholder = grab.interactablePlaceholder.GetComponentInChildren<ManipulablePlaceholder>(true);
                    EditorUtility.SetDirty(grab);
                    EditorUtility.SetDirty(grab.gameObject);
                }
            }
            foreach (var hover in hovers)
            {
                if (hover.interactablePlaceholder != null)
                {
                    hover.visualscriptingPlaceholder = hover.interactablePlaceholder.GetComponentInChildren<VisualScriptingInteractablePlaceholder>(true);
                    EditorUtility.SetDirty(hover);
                    EditorUtility.SetDirty(hover.gameObject);
                }
            }
            return grabs.Length > 0 || hovers.Length > 0;
        }

        private bool FixDetectorsInScene()
        {
            GameObject[] gameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            bool change = false;
            foreach (GameObject gameObject in gameObjects)
            {
                if (FixDetector(gameObject))
                {
                    change = true;
                    Debug.LogError("Found detectors in " + gameObject.name + " in scene " + gameObject.scene.name, gameObject);
                }
            }
            return change;
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

            InteractionPlaceholder interactionPlaceholder = interactable.gameObject.GetOrAddComponent<InteractionPlaceholder>();

            interactionPlaceholder.LockHoverDuringInteraction = interactable.LockHoverDuringInteraction;
            interactionPlaceholder.InteractionColliders = interactable.InteractionColliders;
            interactionPlaceholder.IsNetworked = interactable.IsNetworked;

            EditorUtility.SetDirty(interactionPlaceholder);

            if (interactable.InteractionModes.HasFlag(Core.Interaction.IInteractable.EInteractableType.ContextualMenuInteractable))
            {
                ContextualMenuPlaceholder contextualMenuPlaceholder = interactable.gameObject.GetOrAddComponent<ContextualMenuPlaceholder>();
                contextualMenuPlaceholder.ContextualMenuOptions = interactable.ContextualMenuOptions;
                EditorUtility.SetDirty(contextualMenuPlaceholder);
            }
            else
            {
                if (interactionPlaceholder.TryGetComponent<ContextualMenuPlaceholder>(out var cmp))
                {
                    DestroyImmediate(cmp, true);
                }
            }

            if (interactable.InteractionModes.HasFlag(Core.Interaction.IInteractable.EInteractableType.Manipulable))
            {
                ManipulablePlaceholder manipulablePlaceholder = interactable.gameObject.GetOrAddComponent<ManipulablePlaceholder>();
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

                EditorUtility.SetDirty(manipulablePlaceholder);
            }
            else
            {
                if (interactionPlaceholder.TryGetComponent<ManipulablePlaceholder>(out var cmp))
                {
                    DestroyImmediate(cmp, true);
                }
            }

            if (interactable.InteractionModes.HasFlag(Core.Interaction.IInteractable.EInteractableType.VisualScriptingInteractable))
            {
                VisualScriptingInteractablePlaceholder vsPlaceholder = interactable.gameObject.GetOrAddComponent<VisualScriptingInteractablePlaceholder>();
                vsPlaceholder.DesktopAllowedStates = interactable.DesktopAllowedStates;
                vsPlaceholder.VRAllowedStates = interactable.VRAllowedStates;
                vsPlaceholder.InteractionsScriptMachine = interactable.InteractionsScriptMachine;
                vsPlaceholder.VrVisualScriptingInteraction = interactable.VrVisualScriptingInteraction;

                EditorUtility.SetDirty(vsPlaceholder);
            }
            else
            {
                if (interactionPlaceholder.TryGetComponent<VisualScriptingInteractablePlaceholder>(out var cmp))
                {
                    DestroyImmediate(cmp, true);
                }
            }

            EditorUtility.SetDirty(interactionPlaceholder.gameObject);

        }

        private bool DestroyOldComponentInScene()
        {
            GameObject[] gameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            bool modified = false;
            foreach (GameObject gameObject in gameObjects)
            {
                var isModified = DestroyComponentRecursive(gameObject);
                modified = modified || isModified;
            }
            return modified;
        }

        private bool DestroyOldComponentInPrefab(GameObject prefab)
        {
            var replaced = DestroyComponentRecursive(prefab);
            return replaced;
        }

        private bool DestroyComponentRecursive(GameObject gameObject)
        {
            InteractablePlaceholderObsolete[] interactables = gameObject.GetComponents<InteractablePlaceholderObsolete>();
            bool modified = false;
            foreach (var interactable in interactables)
            {
                if (interactable != null)
                {
                    DestroyImmediate(interactable, true);
                    EditorUtility.SetDirty(gameObject);
                }
                modified = true;
            }
            foreach (Transform child in gameObject.transform)
            {
                var isModified = DestroyComponentRecursive(child.gameObject);
                modified = modified || isModified;
            }
            return modified;
        }
    }
}
