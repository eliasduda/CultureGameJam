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
    public float zoomSmooth, zoomSpeed;
    private float minZoom = -30f, maxZoom = -100;

    private Camera cam;
    private float targetZoom;

    public Transform modelPivot;
    public DropShaddow shaddow;


    public ModelSettings currentModel;
    public MeshRenderer mesh;
    private int currentHint;
    private Vector3[] HintPositions;
    [System.NonSerialized]
    public float lowestHeight, boundsRadius;
    public bool modelIsLoaded = true;

    public void LoadModel(ModelSettings model)
    {
        currentModel = model;
        cam = MasterManager.Instance.mainCam;

        mesh = Instantiate(model.model, modelPivot).GetComponent<MeshRenderer>();
        if (!mesh) Debug.LogError("Controller found no mesh");
        mesh.gameObject.transform.position = modelPivot.position - mesh.bounds.center;
        lowestHeight = mesh.bounds.min.y;

        boundsRadius = Mathf.Max(mesh.bounds.size.x, mesh.bounds.size.z) * 0.5f;
        shaddow.SetTo(this);

        HintPositions = new Vector3[5];
        for(int i = 0; i < HintPositions.Length; i++)
        {
            HintPositions[i] = mesh.sharedMaterial.GetVector("_Hint" + (i + 1));
        }
        targetZoom = minZoom - (minZoom - minZoom) * 0.5f;

        modelIsLoaded = true;
    }

    void Update()
    {
        if (!modelIsLoaded) return;
        // Apply rotation around the local x-axis
        transform.Rotate(Vector3.right, rotationVelocity.y * Time.deltaTime, Space.World);
        // Apply rotation around the local up-axis (y-axis)
        modelPivot.Rotate(Vector3.up, -rotationVelocity.x * Time.deltaTime, Space.Self);

        // Make sure the world z-axis rotation is zero
        Vector3 eulerAngles = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(ClampAngle(eulerAngles.x, tiltLimits.x, tiltLimits.y), eulerAngles.y, eulerAngles.z);
        

        // Gradually decrease the velocity over time if no new velocity is added
        rotationVelocity = Vector2.Lerp(rotationVelocity, Vector2.zero, decayRate * Time.deltaTime);


        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, Mathf.Lerp(cam.transform.position.z, targetZoom, zoomSmooth * Time.deltaTime));
     
    }

    public void Zoom(float delta)
    {
        targetZoom = Mathf.Clamp(targetZoom + delta * zoomSpeed, maxZoom, minZoom);
    }

    internal void SetHint(int hint,Vector3 pos)
    {
        mesh.sharedMaterial.SetVector("_Hint"+hint, mesh.transform.InverseTransformPoint(pos));
    }

    public void GetHint()
    {
        currentHint++;
        mesh.sharedMaterial.SetFloat("_CurrentHint" , currentHint);
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

    public float CalculateDistanceToFitObjectInFrame(Camera camera, Vector3 objectBoundsSize)
    {
        // Get the size of the object (largest dimension)
        float objectHeight = objectBoundsSize.y; // height of the object
        float objectWidth = objectBoundsSize.x;  // width of the object
        float objectDepth = objectBoundsSize.z;  // depth of the object

        // Choose the largest dimension between width and height to make sure it fits
        float objectSize = Mathf.Max(objectHeight, objectWidth);

        // Calculate half of the field of view in radians
        float cameraFOVRadians = camera.fieldOfView * Mathf.Deg2Rad / 2.0f;

        // Get the aspect ratio of the camera (width/height)
        float aspectRatio = camera.aspect;

        // If the width is larger, adjust for the aspect ratio to avoid clipping on the sides
        if (objectWidth > objectHeight)
        {
            objectSize = objectSize / aspectRatio;
        }

        // Calculate the distance from the camera to fit the object in the frame
        float distance = objectSize / Mathf.Tan(cameraFOVRadians);

        // Return the required distance
        return distance;
    }
}
