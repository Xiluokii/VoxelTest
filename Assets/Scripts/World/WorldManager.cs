using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 这里管理所有的块 和 物体实例化
public class WorldManager : MonoBehaviour
{
   
    public Material testMaterial;
    private Container _container;
    
    void Start()
    {
        GameObject con = new GameObject("Container");
        con.transform.parent = transform;
        _container = con.AddComponent<Container>();
        
        _container.Initialize(testMaterial, Vector3.zero);
        _container.GenerateMesh();
        _container.UploadMesh();
    }

    void Update()
    {
        
    }
}
