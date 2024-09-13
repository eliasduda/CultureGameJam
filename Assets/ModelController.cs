using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    Vector2 rotationVelocity;
    public float decayRate = 5f;
    public float rotationSpeed = 5f;
    public Vector2 tiltLimits = new Vector2(-20, 10);
    public Transform modelPivot;
    MeshRenderer mesh;

    private void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        if (!mesh) Debug.LogError("Controller found no mesh");
        mesh.gameObject.transform.position = modelPivot.position - mesh.bounds.center;

    }

    void Update()
    {
        // Apply rotation around the local x-axis
        transform.Rotate(Vector3.right, -rotationVelocity.y * Time.deltaTime, Space.World);
        // Apply rotation around the local up-axis (y-axis)
        modelPivot.Rotate(Vector3.up, -rotationVelocity.x * Time.deltaTime, Space.Self);

        // Make sure the world z-axis rotation is zero
        Vector3 eulerAngles = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(ClampAngle(eulerAngles.x, tiltLimits.x, tiltLimits.y), eulerAngles.y, eulerAngles.z);
        

        // Gradually decrease the velocity over time if no new velocity is added
        rotationVelocity = Vector2.Lerp(rotationVelocity, Vector2.zero, decayRate * Time.deltaTime);
    }

    internal void SetHint(int hint,Vector3 pos)
    {
        mesh.sharedMaterial.SetVector("_Hint"+hint, mesh.transform.InverseTransformPoint(pos));
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle > 180) angle -= 360; // Convert to [-180, 180] range for clamping
        return Mathf.Clamp(angle, min, max);
    }

    // Function to set rotation velocity
    public void SetRotationVelocity(Vector2 newVelocity)
    {
        rotationVelocity += newVelocity * rotationSpeed;
    }
}
