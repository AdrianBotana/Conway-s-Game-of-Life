using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawer3D : MonoBehaviour
{
    [SerializeField] public Mesh instanceMesh;
    [SerializeField] public Material instanceMaterial;

    private ComputeBuffer positionBuffer;
    private ComputeBuffer argsBuffer;
    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    void Awake()
    {
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
    }

    void OnDestroy()
    {
        if (positionBuffer != null)
            positionBuffer.Release();
        positionBuffer = null;

        if (argsBuffer != null)
            argsBuffer.Release();
        argsBuffer = null;
    }

    void Update()
    {
        Graphics.DrawMeshInstancedIndirect(instanceMesh, 0, instanceMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), argsBuffer);
    }

    public void Draw(Vector4[] positions)
    {
        // Positions
        if (positionBuffer != null)
            positionBuffer.Release();

        positionBuffer = new ComputeBuffer(positions.Length, 16);
        positionBuffer.SetData(positions);
        instanceMaterial.SetBuffer("positionBuffer", positionBuffer);

        args[0] = (uint)instanceMesh.GetIndexCount(0);
        args[1] = (uint)positions.Length;
        args[2] = (uint)instanceMesh.GetIndexStart(0);
        args[3] = (uint)instanceMesh.GetBaseVertex(0);

        argsBuffer.SetData(args);
    }
}
