using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    public List<GameObject> menus;
    public GameObject mapMenu;
    public GameObject modelMenu;
    public GameObject memoryMenu;
    public GameObject collectionMenu;
    public GameObject cameraMenu;
    public Map map;
    public ModelController modelView;
    public Slider hintSlider;
    public PhoneCamera phoneCam;
    public GameObject modelButton, CameraButton;
    private GameObject currentMenu;


    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        menus.Add(mapMenu);
        menus.Add(modelMenu);
        menus.Add(memoryMenu);
        menus.Add(collectionMenu);
        menus.Add(cameraMenu);
        SwitchMenu(mapMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchMenu(GameObject newMenu)
    {
        foreach(GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        currentMenu = newMenu;
        currentMenu.SetActive(true);

        map.gameObject.SetActive(currentMenu == mapMenu);
        modelView.gameObject.SetActive(currentMenu == modelMenu || currentMenu == memoryMenu);
        if (currentMenu == cameraMenu) phoneCam.StartCamera();
        else phoneCam.StopCamera();

        if (currentMenu == memoryMenu) modelView.Reveal();
        if (currentMenu == collectionMenu) collectionMenu.GetComponent<CollectionMenu>().GoToOverview();

        CameraButton.SetActive(currentMenu == modelMenu);
        modelButton.SetActive(currentMenu != modelMenu);
    }

    public void GoToMap() { SwitchMenu(mapMenu); }
    public void GoToColledction() { SwitchMenu(collectionMenu); }
    public void GoToModel() { SwitchMenu(modelMenu); }
    public void GoToCamera() { SwitchMenu(cameraMenu); }
    public void GoToMemory() { SwitchMenu(memoryMenu); }
}
