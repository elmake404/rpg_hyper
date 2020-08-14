using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialReplacement : MonoBehaviour
{
    [SerializeField]
    private Material _currentMaterial, _newMaterial;
    [SerializeField]
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField]
    private MeshRenderer _meshRenderer;

    public void NewMaterial()
    {
        if (_skinnedMeshRenderer!=null)
        {
            _skinnedMeshRenderer.material = _newMaterial;
        }
        else
        {
            _meshRenderer.material = _newMaterial;
        }
    }
    public void OldMaterial()
    {
        if (_skinnedMeshRenderer!=null)
        {
            _skinnedMeshRenderer.material = _currentMaterial;
        }
        else
        {
            _meshRenderer.material = _currentMaterial;
        }
    }
}
