using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Container : MonoBehaviour
{
    
    public Vector3 containerPosition;
    public MeshData meshData = new MeshData();
    
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;
    

    public void Initialize(Material mat, Vector3 position)
    {
        ConfigComponents();
        meshRenderer.sharedMaterial = mat;
        containerPosition = position;
    }

    private void ConfigComponents()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public void GenerateMesh()
    {
        meshData.ClearData();
        Vector3 blockPos = new Vector3(8,8,8);
        Voxel block = new Voxel(){id = 1};
        int counter = 0;
        
        Vector3[] faceVertices = new Vector3[4];
        Vector2[] faceUVs = new Vector2[4];


        for (int i = 0; i < 6; i++)
        {
            
            for (int j = 0; j < 4; j++)
            {
                faceVertices[j] = voxelVertices[voxelVertexIndex[i, j]] + blockPos;
                faceUVs[j] = voxelUVs[j];
            }

            for (int j = 0; j < 6; j++)
            {
                meshData.vertices.Add(faceVertices[voxelTris[i, j]]);
                meshData.UVs.Add(faceUVs[voxelTris[i, j]]);
                
                meshData.triangles.Add(counter++);
            }
        }
        
    }

    public void UploadMesh()
    {
        meshData.UploadMesh();

        if (meshRenderer == null)
        {
            ConfigComponents();
        }
        
        meshFilter.mesh = meshData.mesh;
        if(meshData.vertices.Count > 3)
            meshCollider.sharedMesh = meshData.mesh;
    }
    

    #region MeshData
    
    public struct MeshData
    { 
        public Mesh mesh; 
        public List<Vector3> vertices; 
        public List<int> triangles;

        public List<Vector2> UVs;
        
        public bool inisialized;

        public void ClearData()
        {
            if (!inisialized)
            {
                vertices = new List<Vector3>();
                triangles = new List<int>();
                UVs = new List<Vector2>();
                inisialized = true;
                mesh = new Mesh();
                
                return;
            }
            
            vertices.Clear();
            triangles.Clear();
            UVs.Clear();
            mesh.Clear();
        }
        
        public void UploadMesh(bool shareVertices = false)
        {
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0,false);
            mesh.uv = UVs.ToArray();
            
            mesh.Optimize();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.UploadMeshData(false);
            
        }
    }
    
    
    #endregion

    #region Voxel statics

    static readonly Vector3[] voxelVertices = new Vector3[8]
    {
        new Vector3(0, 0, 0), // 左下角
        new Vector3(1, 0, 0), // 右下角
        new Vector3(0, 1, 0), // 左上角
        new Vector3(1, 1, 0), // 右上角
        
        new Vector3(0, 0, 1),  // 顶部
        new Vector3(1, 0, 1),
        new Vector3(0, 1, 1),
        new Vector3(1, 1, 1)
    };
    
    static readonly int[,] voxelVertexIndex = new int[6, 4]
    {
        {0, 1, 2, 3}, 
        {4, 5, 6, 7}, 
        {0, 4, 6, 2},
        {5, 1, 3, 7}, 
        {0, 1, 5, 4},
        {2, 3, 7, 6}
    };
    
    static readonly Vector2[] voxelUVs = new Vector2[4]
    {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(1, 1)
    };

    private static readonly int[,] voxelTris = new int[6, 6]
    {
        {0,2,3,0,3,1},
        {0,1,2,0,3,2},
        {0,2,3,0,3,1},
        {0,1,2,1,3,2},
        {0,1,2,0,3,1},
        {0,2,3,0,3,1}
    };

    #endregion

}
