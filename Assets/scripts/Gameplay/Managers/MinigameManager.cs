using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    private int focus = 0;
    private bool paused = false;
    private bool disabled = false;
    
    //Managers for minigame state altering
    public DishWashingManager DWM;
    public GameplayManager GM;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) //When Game is Paused
        {
            if (!paused) 
            {
                GM.gmState(false);
                DWM.dwState(false);
                paused = true;
            }
            else
            {
                paused = false;
            }
        }

        if (!disabled)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                focus--;
            
                if (focus < 0)
                {
                    focus = 3;
                }

                if (focus > 3)
                {
                    focus = 0;
                }
                setMG();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                focus++;
            
                if (focus < 0)
                {
                    focus = 3;
                }

                if (focus > 3)
                {
                    focus = 0;
                }
                setMG();
            }
        }
    }

    private void setMG()
    {
        if (focus == 0) //Set coffee station true
        {
            DWM.dwState(false);
            GM.gmState(true);
        }
        else if (focus == 3) //Set dishwasher true
        {
            GM.gmState(false);
            DWM.dwState(true);
        }
        else
        {
            GM.gmState(false);
            DWM.dwState(false);
        }
    }

    public int currFocus()
    {
        return focus;
    }

    public void disableAll()
    {
        DWM.dwState(false);
        GM.gmState(false);
        disabled = true;
    }

    public void enableAll()
    {
        disabled = false;
        setMG();
    }

    public void unPause()
    {
        paused = false;
        setMG();
    }
}
