using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed;
    [SerializeField] private Space rotationSpace;

    private void FixedUpdate()
    {
        transform.Rotate(rotationSpeed * Time.fixedDeltaTime, rotationSpace);
    }
}
