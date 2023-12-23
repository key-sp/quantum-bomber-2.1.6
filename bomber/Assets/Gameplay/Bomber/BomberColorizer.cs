using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEditor;
using UnityEngine;

public class BomberColorizer : MonoBehaviour
{
    [SerializeField] private Renderer[] _renderers = null;
    
    public void UpdateColor(Color color)
    {
        foreach (var r in _renderers)
        {
            foreach (var material in r.materials)
            {
                if (material.name != "Main (Instance)") continue;

                material.color = color;
            }
        }
    }

#if UNITY_EDITOR
    public void GetRenderers()
    {
        _renderers = GetComponentsInChildren<Renderer>();
    }
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(BomberColorizer))]
public class BomberColorizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var colorizer = (BomberColorizer)target;

        if (GUILayout.Button("Grab Renderers"))
        {
            colorizer.GetRenderers();
        }
    }
}

#endif