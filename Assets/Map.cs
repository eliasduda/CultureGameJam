using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    MyInputs inputs ;
    public float movespeed = 50, zoomSpeed = 30, smooth = 10f;
    public Vector3 targetPos, targetScale;

    private float pinchDistance;
    private void OnEnable()
    {
        inputs = new MyInputs();
        inputs.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        targetScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 axis = inputs.ModelView.SwipeAxis.ReadValue<Vector2>();
        if (axis.magnitude > 0)
        {
            targetPos += new Vector3(axis.x, axis.y, 0) * movespeed * Time.deltaTime;
        }
        if (inputs.ModelView.Touch1.WasPerformedThisFrame())
        {
            pinchDistance = Vector3.Distance(inputs.ModelView.Touch0Position.ReadValue<Vector2>(), inputs.ModelView.Touch1Posiiton.ReadValue<Vector2>());
        }
        else if (inputs.ModelView.Touch1.IsPressed() && inputs.ModelView.Touch0.IsPressed())
        {
            float currentDist = Vector3.Distance(inputs.ModelView.Touch0Position.ReadValue<Vector2>(), inputs.ModelView.Touch1Posiiton.ReadValue<Vector2>());
            float zoomdelta = pinchDistance = currentDist - pinchDistance;
            pinchDistance = currentDist;

            if (zoomdelta != 0) Debug.Log("Pinching " + zoomdelta);
            targetScale += Vector3.one * zoomSpeed * zoomdelta * Time.deltaTime;
        }
        if (inputs.ModelView.Zoom.ReadValue<float>() > 0)
        {
            Debug.Log("Zoom " + inputs.ModelView.Zoom.ReadValue<float>());
            targetScale += Vector3.one * zoomSpeed * inputs.ModelView.Zoom.ReadValue<float>() * Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smooth);
    }
}
