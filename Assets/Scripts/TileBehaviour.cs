using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _tileMeshRenderer;

    private static readonly int TileHeight = Shader.PropertyToID("_TileHeight");
    private float _scaleY = 1.0f;
    private Material _tileMaterial;

    private Material TileMaterial
    {
        get
        {
            if (_tileMaterial == null)
            {
                _tileMaterial = _tileMeshRenderer.material;
            }

            return _tileMaterial;
        }
    }

    public void SetHeight(float newHeight)
    {
        _scaleY = newHeight;
        transform.localScale = new Vector3(transform.localScale.x, _scaleY, transform.localScale.z);
        
        TileMaterial.SetFloat(TileHeight, newHeight);
    }
}