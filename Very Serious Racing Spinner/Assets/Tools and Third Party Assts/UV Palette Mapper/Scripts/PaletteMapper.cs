using UnityEngine;
using UnityEditor;
using System.IO;

public class PaletteMapper : EditorWindow
{
    private GameObject selectedObject;
    private Texture2D paletteTexture;
    private Vector2 scrollPosition;

    // Keys for persisting texture slot across sessions
    private const string PREFS_TEXTURE_KEY = "CozyPaletteMapper_TexturePath";

    [MenuItem("Tools/UV Palette ReMapper")]
    public static void ShowWindow()
    {
        PaletteMapper window = GetWindow<PaletteMapper>("UV Palette ReMapper");
        window.minSize = new Vector2(300, 450);
    }

    private void OnEnable()
    {
        Selection.selectionChanged += Repaint;
        LoadSavedTexture();
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= Repaint;
    }

    private void OnGUI()
    {
        GUILayout.Label("UV Palette ReMapper", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 1. Get Selected Object from Hierarchy
        selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            EditorGUILayout.HelpBox("Please select a GameObject with a MeshFilter in your hierarchy.", MessageType.Info);
            return;
        }

        MeshFilter meshFilter = selectedObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = selectedObject.GetComponent<MeshRenderer>();

        if (meshFilter == null || meshRenderer == null)
        {
            EditorGUILayout.HelpBox("Selected GameObject must have a MeshFilter and MeshRenderer.", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Selected Object:", selectedObject.name, EditorStyles.boldLabel);

        // 2. Action Buttons (Restore and Bake Panel)
        MeshKeeper keeper = selectedObject.GetComponent<MeshKeeper>();
        if (keeper != null && keeper.originalMesh != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.name.EndsWith("_mapped"))
        {
            EditorGUILayout.BeginHorizontal();

            // Restore Button
            GUI.backgroundColor = new Color(0.9f, 0.4f, 0.4f); // Soft red
            if (GUILayout.Button("↩ Restore Mesh", GUILayout.Height(28)))
            {
                RestoreOriginalMesh(meshFilter, keeper);
            }

            // Bake Permanent Button
            GUI.backgroundColor = new Color(0.4f, 0.8f, 0.4f); // Soft green
            if (GUILayout.Button("💾 Make Permanent", GUILayout.Height(28)))
            {
                BakeMeshPermanently(meshFilter, keeper);
            }

            GUI.backgroundColor = Color.white; // Reset UI color
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        // 3. Select the Palette Texture
        EditorGUI.BeginChangeCheck();
        paletteTexture = (Texture2D)EditorGUILayout.ObjectField("Palette Texture", paletteTexture, typeof(Texture2D), false);
        if (EditorGUI.EndChangeCheck())
        {
            SaveTextureSelection();
        }

        if (paletteTexture == null)
        {
            EditorGUILayout.HelpBox("Assign your color palette texture to begin mapping.", MessageType.Info);
            return;
        }

        // Check if texture is readable
        string path = AssetDatabase.GetAssetPath(paletteTexture);
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
        if (importer != null && !importer.isReadable)
        {
            EditorGUILayout.HelpBox("Texture must be Read/Write enabled in its import settings!", MessageType.Error);
            if (GUILayout.Button("Fix Texture Import Settings"))
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
            }
            return;
        }

        EditorGUILayout.Space();
        GUILayout.Label("Click a color block below to map selected sub-mesh:", EditorStyles.miniLabel);

        // 4. Draw the Interactive Color Palette
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        float aspect = (float)paletteTexture.height / paletteTexture.width;
        float width = position.width - 30;
        float height = width * aspect;

        Rect rect = GUILayoutUtility.GetRect(width, height);
        GUI.DrawTexture(rect, paletteTexture);

        Event currentEvent = Event.current;
        if (currentEvent.type == EventType.MouseDown && rect.Contains(currentEvent.mousePosition))
        {
            Vector2 localClick = currentEvent.mousePosition - rect.position;

            float u = localClick.x / rect.width;
            float v = 1.0f - (localClick.y / rect.height);

            MapUVsToCoordinate(meshFilter, meshRenderer, u, v);
            currentEvent.Use();
        }

        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// Safely manages mesh cloning, caches the original reference via MeshKeeper, and collapses UV coordinates.
    /// </summary>
    private void MapUVsToCoordinate(MeshFilter filter, MeshRenderer renderer, float targetU, float targetV)
    {
        Undo.RecordObjects(new Object[] { filter, renderer, filter.gameObject }, "Map UV to Palette");

        Mesh currentMesh = filter.sharedMesh;
        if (currentMesh == null) return;

        Mesh targetMesh;

        if (currentMesh.name.EndsWith("_mapped"))
        {
            targetMesh = currentMesh;
            Undo.RecordObject(targetMesh, "Update UV Coordinates");
        }
        else
        {
            MeshKeeper keeper = filter.gameObject.GetComponent<MeshKeeper>();
            if (keeper == null)
            {
                keeper = Undo.AddComponent<MeshKeeper>(filter.gameObject);
            }

            keeper.originalMesh = currentMesh;
            targetMesh = Instantiate(currentMesh);

            string cleanName = currentMesh.name.Replace("(Clone)", "");
            targetMesh.name = cleanName + "_mapped";

            filter.sharedMesh = targetMesh;
        }

        // Apply Texture to Material
        Material targetMaterial = renderer.sharedMaterial;

        if (targetMaterial == null || targetMaterial.name.Contains("Default-Material"))
        {
            Shader defaultShader = Shader.Find("Universal Render Pipeline/Lit");
            if (defaultShader == null) defaultShader = Shader.Find("Standard");

            targetMaterial = new Material(defaultShader);
            targetMaterial.name = "PaletteMapped_Material";
            renderer.sharedMaterial = targetMaterial;
        }

        Undo.RecordObject(targetMaterial, "Apply Palette Texture");

        if (targetMaterial.HasProperty("_BaseMap"))
            targetMaterial.SetTexture("_BaseMap", paletteTexture);
        else if (targetMaterial.HasProperty("_MainTex"))
            targetMaterial.SetTexture("_MainTex", paletteTexture);

        // Modify the UV coordinates
        Vector2[] uvs = targetMesh.uv;

        if (uvs == null || uvs.Length == 0)
        {
            uvs = new Vector2[targetMesh.vertexCount];
        }

        Vector2 targetUV = new Vector2(targetU, targetV);
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = targetUV;
        }

        targetMesh.uv = uvs;

        targetMesh.RecalculateBounds();
        targetMesh.RecalculateNormals();
        targetMesh.RecalculateTangents();

        EditorUtility.SetDirty(filter.gameObject);
        EditorUtility.SetDirty(renderer.gameObject);

        Debug.Log($"[{filter.gameObject.name}] Mapped mesh named: {targetMesh.name}");
    }

    /// <summary>
    /// Instantly restores the original mesh directly from the cached MeshKeeper component reference.
    /// </summary>
    private void RestoreOriginalMesh(MeshFilter filter, MeshKeeper keeper)
    {
        if (keeper == null || keeper.originalMesh == null) return;

        Mesh currentMesh = filter.sharedMesh;

        Undo.RecordObjects(new Object[] { filter, keeper }, "Restore Original Mesh");

        filter.sharedMesh = keeper.originalMesh;

        if (currentMesh != null && currentMesh.name.EndsWith("_mapped"))
        {
            DestroyImmediate(currentMesh);
        }

        DestroyImmediate(keeper);

        EditorUtility.SetDirty(filter.gameObject);
        Debug.Log($"[{filter.gameObject.name}] Successfully restored original mesh directly from stored cache!");
    }

    /// <summary>
    /// Bakes the current UV modifications directly into the object within the scene, 
    /// leaving no extra asset files behind.
    /// </summary>
    private void BakeMeshPermanently(MeshFilter filter, MeshKeeper keeper)
    {
        Mesh modifiedMesh = filter.sharedMesh;
        if (modifiedMesh == null) return;

        // 1. Create an official Undo group for the operation
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Bake UV Permanently");

        // 2. Remove the custom string flags so the name is completely clean in the inspector
        string cleanName = modifiedMesh.name.Replace("_mapped", "").Replace("(Clone)", "");
        modifiedMesh.name = cleanName;

        // 3. Destroy the keeper component so it can no longer be reverted
        Undo.DestroyObjectImmediate(keeper);

        // 4. Force Unity to register that this specific GameObject has modified, local mesh data
        EditorUtility.SetDirty(filter.gameObject);

        // If this object is part of a Prefab Stage, this forces the changes to save back to the Prefab asset
        var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null)
        {
            EditorUtility.SetDirty(prefabStage.prefabContentsRoot);
        }

        Debug.Log($"[{filter.gameObject.name}] UV data baked permanently into the scene geometry. No files generated!");
        EditorUtility.DisplayDialog("UVs Applied Permanently",
            $"The new UV layout has been permanently applied to {filter.gameObject.name} inside this scene!\n\n" +
            "The tool's temporary helper files have been removed.", "Awesome");
    }

    /// <summary>
    /// Saves selected texture path profile into local computer registry configurations.
    /// </summary>
    private void SaveTextureSelection()
    {
        if (paletteTexture != null)
        {
            string path = AssetDatabase.GetAssetPath(paletteTexture);
            EditorPrefs.SetString(PREFS_TEXTURE_KEY, path);
        }
        else
        {
            EditorPrefs.DeleteKey(PREFS_TEXTURE_KEY);
        }
    }

    /// <summary>
    /// Automatically fetches saved configuration references from local profiles.
    /// </summary>
    private void LoadSavedTexture()
    {
        if (EditorPrefs.HasKey(PREFS_TEXTURE_KEY))
        {
            string path = EditorPrefs.GetString(PREFS_TEXTURE_KEY);
            if (!string.IsNullOrEmpty(path))
            {
                paletteTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            }
        }
    }
}