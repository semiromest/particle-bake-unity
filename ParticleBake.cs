using UnityEngine;
using UnityEditor;

public class ParticleBakeTool : EditorWindow
{
    private ParticleSystem particleSystem;
    private Camera bakeCamera;
    private bool useTransform = true; 
    private bool applyMaterial = true; 
    private bool exportAsImage = false; 
    private bool exportAsMesh = false; 
    private int textureResolution = 1024; 
    private string imageSavePath = "Assets/BakedParticleTexture.png";
    private string meshSavePath = "Assets/BakedParticleMesh.asset"; 
    private const string linkURL = "https://www.linkedin.com/in/ibrahim-melih-do%C4%9Fan-633215178/";

    [MenuItem("Tools/Particle Bake Tool")]
    public static void ShowWindow()
    {
        GetWindow<ParticleBakeTool>("Particle Bake Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Particle Bake Tool", EditorStyles.boldLabel);

        particleSystem = (ParticleSystem)EditorGUILayout.ObjectField("Particle System", particleSystem, typeof(ParticleSystem), true);
        bakeCamera = (Camera)EditorGUILayout.ObjectField("Bake Camera", bakeCamera, typeof(Camera), true);
        useTransform = EditorGUILayout.Toggle("Use Transform", useTransform);
        applyMaterial = EditorGUILayout.Toggle("Apply Material to Mesh", applyMaterial);
        exportAsImage = EditorGUILayout.Toggle("Export as Image", exportAsImage);
        exportAsMesh = EditorGUILayout.Toggle("Export as Mesh", exportAsMesh);

        if (exportAsImage)
        {
            GUILayout.Space(10);
            textureResolution = EditorGUILayout.IntField("Texture Resolution", textureResolution);
            imageSavePath = EditorGUILayout.TextField("Image Save Path", imageSavePath);
        }

        if (exportAsMesh)
        {
            GUILayout.Space(10);
            meshSavePath = EditorGUILayout.TextField("Mesh Save Path", meshSavePath);
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Bake Particles"))
        {
            if (exportAsMesh)
            {
                BakeParticles();
            }


            if (exportAsImage)
            {
                ExportMeshAsImage();
            }

            if (!exportAsMesh && !exportAsImage)
            {
                Debug.LogWarning("There is no selected option.");
            }
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Contact Me-LinkedIn"))
        {
            Application.OpenURL(linkURL);
        }
    }

    private void BakeParticles()
    {
        if (particleSystem == null)
        {
            Debug.LogError("Particle System is not assigned!");
            return;
        }

        ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        if (renderer == null)
        {
            Debug.LogError("No ParticleSystemRenderer found on the selected object!");
            return;
        }

        Mesh bakedMesh = new Mesh();
        ParticleSystemBakeMeshOptions bakeOptions = useTransform
            ? ParticleSystemBakeMeshOptions.BakePosition
            : ParticleSystemBakeMeshOptions.Default;

        renderer.BakeMesh(bakedMesh, bakeCamera, bakeOptions);

        GameObject bakedObject = new GameObject("Baked Particle Mesh");
        MeshFilter meshFilter = bakedObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = bakedObject.AddComponent<MeshRenderer>();

        meshFilter.sharedMesh = bakedMesh;

        if (applyMaterial)
        {
            meshRenderer.sharedMaterial = renderer.sharedMaterial;
        }

        if (exportAsMesh)
        {
            ExportMeshAsAsset(bakedMesh);
        }

        Debug.Log("Particle mesh successfully baked!");
    }

    private void ExportMeshAsImage()
    {
        if (bakeCamera == null)
        {
            Debug.LogError("Bake Camera is not assigned!");
            return;
        }

        Color originalBackgroundColor = bakeCamera.backgroundColor;
        CameraClearFlags originalClearFlags = bakeCamera.clearFlags;
        RenderTexture originalTargetTexture = bakeCamera.targetTexture;

        try
        {
            string directory = System.IO.Path.GetDirectoryName(imageSavePath);
            EnsureDirectoryExists(directory);

            imageSavePath = GetUniqueFilePath(imageSavePath);

            RenderTexture renderTexture = new RenderTexture(textureResolution, textureResolution, 24);

            bakeCamera.backgroundColor = Color.clear;
            bakeCamera.clearFlags = CameraClearFlags.SolidColor;

            bakeCamera.targetTexture = renderTexture;
            bakeCamera.Render();

            RenderTexture.active = renderTexture;

            Texture2D texture = new Texture2D(textureResolution, textureResolution, TextureFormat.ARGB32, false);
            texture.ReadPixels(new Rect(0, 0, textureResolution, textureResolution), 0, 0);
            texture.Apply();

            RenderTexture.active = null;

            byte[] bytes = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(imageSavePath, bytes);

            Debug.Log($"Texture saved to: {imageSavePath}");
            AssetDatabase.Refresh();
        }
        finally
        {
            bakeCamera.backgroundColor = originalBackgroundColor;
            bakeCamera.clearFlags = originalClearFlags;
            bakeCamera.targetTexture = originalTargetTexture;
        }
    }

    private void ExportMeshAsAsset(Mesh bakedMesh)
    {
        if (string.IsNullOrEmpty(meshSavePath))
        {
            Debug.LogError("Mesh save path is empty!");
            return;
        }

        string directory = System.IO.Path.GetDirectoryName(meshSavePath);
        EnsureDirectoryExists(directory);

        meshSavePath = GetUniqueFilePath(meshSavePath);

        AssetDatabase.CreateAsset(bakedMesh, meshSavePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Mesh saved to: {meshSavePath}");
    }

    private void EnsureDirectoryExists(string path)
    {
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
    }

    private string GetUniqueFilePath(string filePath)
    {
        string directory = System.IO.Path.GetDirectoryName(filePath);
        string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
        string extension = System.IO.Path.GetExtension(filePath);

        int counter = 1;
        string newPath = filePath;

        while (System.IO.File.Exists(newPath))
        {
            newPath = System.IO.Path.Combine(directory, $"{fileName} ({counter++}){extension}");
        }

        return newPath;
    }

}
