using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DishWashingManager : MonoBehaviour
{
    private bool DWActive = false;
    public GameObject dwUI;
    private int cups = 5;
    private int maxCups = 5;
    public TextMeshProUGUI cupText;
    private float fillIncrement = 0.20f;
    private float currFill = 0;
    public Image fill;
    public SoundManager soundManager;
    public ParticleSystem bubble;


    // Update is called once per frame
    void Update()
    {
        if (DWActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("key pressed");
                if (cups < maxCups)
                {
                    //Debug.Log("cup if statement entered");
                    currFill += fillIncrement;
                    soundManager.PlaySoundEffect("PlateWash");
                    soundManager.PlaySoundEffect("Bubble");
                    bubble.Play();
                    //Debug.Log(currFill);
                    if (currFill >= 1.0f)
                    {
                        cups++;
                        currFill = 0;
                    }
                }
            }
            fill.fillAmount = currFill;
            if (cups >= maxCups)
            {
                dwUI.SetActive(false);
            }
            else
            {
                dwUI.SetActive(true);
            }
            
            if (cups >= maxCups)
            {
                dwUI.SetActive(false);
            }
        }

        cupText.text = $"{cups}";
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

    public int getCups()
    {
        return cups;
    }

    public void subtractCup()
    {
        cups--;
    }
}
