using UnityEngine;

public class TEST_MouseWorldPositionIndicator : MonoBehaviour
{
    [SerializeField] private Material terrainMaterial;

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Layer.Ground.ToLayerMask()))
        {
            Vector4 center = new Vector4(hit.transform.position.x, hit.transform.position.y, 0, 0);
            terrainMaterial.SetVector("_Center", center);
        }
    }
}
