using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishWashingManager : MonoBehaviour
{
    private bool DWActive = false;
    public GameObject dwUI;
    
    // Update is called once per frame
    void Update()
    {
        if (DWActive)
        {
            
        }
    }

    public void dwState(bool value)
    {
        DWActive = value;
        if (value)
        {
            dwUI.SetActive(true);
        }
        else
        {
            dwUI.SetActive(false);
        }
    }
}
