using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private float RotationSpeed;

    public void Rotate(Vector2 rotationDelta)
    {
        transform.localEulerAngles += new Vector3(-rotationDelta.y, rotationDelta.x) * RotationSpeed;
    }
}