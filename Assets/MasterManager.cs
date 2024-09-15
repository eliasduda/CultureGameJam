using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class MasterManager : MonoBehaviour
{
    public static MasterManager Instance;

    public Camera modelCam, mainCam;
    public ModelController modelController;
    public MainMenuToolkit menu;
    public PhoneCamera phoneCam;

    public ModelSettings startModel;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        modelController.LoadModel(startModel);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
