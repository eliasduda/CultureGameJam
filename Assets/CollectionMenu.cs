using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionMenu : MonoBehaviour
{
    public GameObject sights, memories, achievments, overview;
    // Start is called before the first frame update

    void CloseAll()
    {
        sights.SetActive(false);
        memories.SetActive(false);
        achievments.SetActive(false);
        overview.SetActive(false);
    }

    public void GoToSights()
    {
        CloseAll();
        sights.SetActive(true);
    }
    public void GoToMemories()
    {
        CloseAll();
        memories.SetActive(true);
    }
    public void GoToAchievments()
    {
        CloseAll();
        achievments.SetActive(true);
    }
    public void GoToOverview()
    {
        CloseAll();
        overview.SetActive(true);
    }
}
