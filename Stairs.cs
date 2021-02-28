using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stairs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }
}
