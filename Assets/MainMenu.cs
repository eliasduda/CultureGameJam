using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class MainMenu : MonoBehaviour
{
    public UIDocument doc;
    public VisualElement currentMenu;

    private VisualElement root, mapMenu, hintMenu, cameraMenu, collectionMenu;
    private Slider eyeSlider, mapSlider;
    private Button cameraButton;

    private void Awake()
    {
        root = doc.rootVisualElement;
        mapMenu = root.Q<VisualElement>("Menu_Map");
        hintMenu = root.Q<VisualElement>("Menu_Hints");
        cameraMenu = root.Q<VisualElement>("Menu_Camera");
        collectionMenu = root.Q<VisualElement>("Menu_Collection");

        root.Q<Button>("Button_GoCurrentModelCamera").clickable.clicked += GoToModelWitchCamera;
        root.Q<Button>("Button_ToCollection").clickable.clicked += GoToCollection;
        root.Q<Button>("Button_CloseCameraFeed").clickable.clicked += CloseCameraView;
        root.Q<Button>("Button_CloseModel").clickable.clicked += GoToMap;
        root.Q<Button>("Button_TakePicture").clickable.clicked += TakePicture;
        cameraButton = root.Q<Button>("Button_OpenCamera");
        cameraButton.clickable.clicked += OpenCameraView;



        eyeSlider = root.Q<Slider>("Slider_Eye");
        mapSlider = hintMenu.Q<Slider>("Slider_Map");
    }
    // Start is called before the first frame update
    void Start()
    {
        SwitchMenuPage(hintMenu);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentMenu == hintMenu)
        {
            

        }
    }
    void TakePicture()
    {
        Debug.Log("Take Pic");
        MasterManager.Instance.phoneCam.SavePicture();
    }
    void GoToModelWitchCamera()
    {
        SwitchMenuPage(hintMenu);
        OpenCameraView();
    }
    void GoToModelSpecific(ModelSettings model)
    {
        MasterManager.Instance.modelController.LoadModel(model);
        SwitchMenuPage(hintMenu);
    }
    void GoToCollection()
    {
        SwitchMenuPage(collectionMenu);
    }

    void GoToMap()
    {
        SwitchMenuPage(mapMenu);
    }
    void CloseCameraView()
    {
        Debug.Log("CloseCamera");
        cameraMenu.style.display = DisplayStyle.None;
        MasterManager.Instance.phoneCam.StopCamera();
    }
    void OpenCameraView()
    {
        cameraMenu.style.display = DisplayStyle.Flex;
        MasterManager.Instance.phoneCam.StartCamera();
    }

    public void RenderInCameraFrame(Texture2D tex)
    {
        root.Q<VisualElement>("CameraFeed").style.backgroundImage = new StyleBackground(tex);
    }

    void SwitchMenuPage(VisualElement menu)
    {
        currentMenu = menu;

        mapMenu.style.display = DisplayStyle.None;
        cameraMenu.style.display = DisplayStyle.None;
        hintMenu.style.display = DisplayStyle.None;
        currentMenu.style.display = DisplayStyle.Flex;
        collectionMenu.style.display = DisplayStyle.Flex;

        if (currentMenu == hintMenu)
        {
            if (MasterManager.Instance.modelController.currentModel.wasFinished)
            {
                hintMenu.Q<VisualElement>("UI_Finished").style.display = DisplayStyle.Flex;
                hintMenu.Q<VisualElement>("UI_NotFinished").style.display = DisplayStyle.None;
            }
            else
            {
                hintMenu.Q<VisualElement>("UI_Finished").style.display = DisplayStyle.None;
                hintMenu.Q<VisualElement>("UI_NotFinished").style.display = DisplayStyle.Flex;
            }

        }
    }

    public bool IsScreenposInBounds(Vector3 screenPos)
    {
        Rect modelScreenBounds = root.Q<VisualElement>("3D_Model").worldBound;
        float Dist = Vector2.Distance(screenPos, modelScreenBounds.center);
        return Dist < modelScreenBounds.size.x/2;
    }
}
