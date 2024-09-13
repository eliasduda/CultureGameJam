using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    MyInputs inputs;
    Camera cam;
    Vector2 currentPos, currentDelta;
    bool clickedOnModel = false;
    public int currentHintSetter = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new MyInputs();
        cam = MasterManager.Instance.mainCam;
        inputs.Enable();
    }
    void OnDestroy()
    {
        inputs.Dispose();
    }
    // Update is called once per frame
    void Update()
    {
        currentPos = inputs.ModelView.PrimaryTouchPosition.ReadValue<Vector2>();
        currentDelta = inputs.ModelView.SwipeAxis.ReadValue<Vector2>();

        if (inputs.ModelView.PrimaryTouchButton.WasReleasedThisFrame()) Debug.Log("Touch Up");
        if (inputs.ModelView.PrimaryTouchButton.WasPressedThisFrame()) Debug.Log("Touch Down");

        if (inputs.ModelView.PrimaryTouchButton.WasReleasedThisFrame()) clickedOnModel = false;
        else if (currentPos != Vector2.zero && inputs.ModelView.PrimaryTouchButton.WasPressedThisFrame())
        {
            Ray r = cam.ScreenPointToRay(currentPos);
            if(Physics.Raycast(r, out RaycastHit hit, 100f))
            {
                clickedOnModel = true;
                Debug.Log("Clicked On Model");
                if (Input.GetKey(KeyCode.H))
                {
                    Debug.Log("Set Hint");
                    MasterManager.Instance.modelController.SetHint(currentHintSetter, hit.point);
                    currentHintSetter++;
                }
            }
        }

        if(clickedOnModel && inputs.ModelView.PrimaryTouchButton.IsInProgress()){

            MasterManager.Instance.modelController.SetRotationVelocity(currentDelta);
        }
    }
}
