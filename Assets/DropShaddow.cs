using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShaddow : MonoBehaviour
{
    private Material shaddowMat;
    private float shaddowDistance = 6f, shaddowOffset = 2f, falloffDist = 7f;
    // Start is called before the first frame update
    public void SetTo(ModelController modelController)
    {
        modelController = MasterManager.Instance.modelController;
        shaddowMat = gameObject.GetComponent<MeshRenderer>().material;
        transform.position = modelController.transform.position + Vector3.up * (modelController.lowestHeight - shaddowDistance) + Vector3.right * shaddowOffset;

        shaddowMat.SetFloat("_FalloffStart", modelController.boundsRadius);
        shaddowMat.SetFloat("_FalloffEnd", modelController.boundsRadius + falloffDist);
    }

}
