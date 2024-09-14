using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum SubMenu
{ 
    MapView,
    ModelView,
    AchievmentView
}

public class MainMenu : MonoBehaviour
{
    public UIDocument doc;
    public VisualElement currentMenu;

    private VisualElement root, mapMenu, modelMenu;
    public Rect modelScreenBounds;

    private void Awake()
    {
        root = doc.rootVisualElement;
        mapMenu = root.Q<VisualElement>("Menu_Map");
        modelMenu = root.Q<VisualElement>("Menu_Hints");


        modelScreenBounds = root.Q<VisualElement>("3D_Model").worldBound;
    }
    // Start is called before the first frame update
    void Start()
    {

        SwitchMenuPage(SubMenu.ModelView);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentMenu == modelMenu)
        {
        }
    }

    void SwitchMenuPage(SubMenu page)
    {
        switch (page)
        {
            case SubMenu.ModelView:
                currentMenu = modelMenu;
                break;
            case SubMenu.MapView:
                currentMenu = mapMenu;
                break;
            case SubMenu.AchievmentView:
                currentMenu = modelMenu;
                break;
        }

        mapMenu.style.display = DisplayStyle.None;
        modelMenu.style.display = DisplayStyle.None;
        currentMenu.style.display = DisplayStyle.Flex;

        if (currentMenu != modelMenu) MasterManager.Instance.modelController.gameObject.SetActive(false);
        else MasterManager.Instance.modelController.gameObject.SetActive(true);
    }

}
