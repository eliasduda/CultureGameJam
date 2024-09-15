using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public ModelSettings model;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadModel()
    {
        MainMenu.Instance.modelView.LoadModel(model);
        MainMenu.Instance.GoToModel();
    }
}
