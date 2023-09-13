using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GameplayScript : MonoBehaviour
{
    private Drink DC;
    private Order OC;
    public GameObject cup;
    public GameObject trash;
    public GameObject i1;
    public GameObject i2;
    public GameObject i3;
    public GameObject i4;
    public GameObject i5;
    
    //First playable values
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currCup;
    private int correct = 0;
    private int incorrect = 0;
    
    
    public Camera cam;

    private bool _iscamNotNull;

    // Start is called before the first frame update
    void Start()
    {
        OC = gameObject.AddComponent<Order>();
        OC.createOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //Give Customer Order
        {
            bool job;
            job = DC.check(OC.returnOrder());
            if (job == false)
            {
                incorrect++;
            }
            else if (job == true)
            {
                correct++;
            }
            
            OC = null;
            DC = null;
            OC = gameObject.AddComponent<Order>();
            OC.createOrder();
            //Debug.Log("Correct: " + correct + "| Wrong: " + incorrect);
            giveWarning("Order given to customer");
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Left Mouse Clicked");
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                //Debug.Log(hit.transform.name); 
                //Debug.Log("hit");
                if (hit.transform.name == cup.transform.name)
                {
                    if (DC != null)
                    {
                        //Debug.Log("You already have a cup, if you need to start over throw your previous one away");
                        giveWarning("You already have a cup, if you need to start over throw your previous one away");
                    }
                    else
                    {
                        DC = this.AddComponent<Drink>();
                        Debug.Log(">>new cup grabbed<<");
                        giveWarning("New Cup Grabbed");
                    }
                }
                if (DC != null)
                {
                    if (hit.transform.name == i1.transform.name)
                    {
                        DC.addIngredient(1);
                        //Debug.Log(">>added " + i1.transform.name + "<<");
                        giveWarning("Added " + i1.transform.name);
                    }
                
                    if (hit.transform.name == i2.transform.name)
                    {
                        DC.addIngredient(2);
                        //Debug.Log(">>added " + i2.transform.name + "<<");
                        giveWarning("Added " + i2.transform.name);
                    }
                
                    if (hit.transform.name == i3.transform.name)
                    {
                        DC.addIngredient(3);
                        //Debug.Log(">>added " + i3.transform.name + "<<");
                        giveWarning("Added " + i3.transform.name);
                    }
                
                    if (hit.transform.name == i4.transform.name)
                    {
                        DC.addIngredient(4);
                        //Debug.Log(">>added " + i4.transform.name + "<<");
                        giveWarning("Added " + i4.transform.name);
                    }
                
                    if (hit.transform.name == i5.transform.name)
                    {
                        DC.addIngredient(5);
                        //Debug.Log(">>added " + i5.transform.name + "<<");
                        giveWarning("Added " + i5.transform.name);
                    }

                    if (hit.transform.name == trash.transform.name)
                    {
                        DC = null;
                        giveWarning("Cup thrown away");
                    }
                }
                else
                {
                    //Debug.Log("You need to grab a cup first!");
                    giveWarning("You need to grab a cup first!");
                }
            }
        }
        updateText();
    }
    
    //Functions for first Playable only
    private void giveWarning(string message)
    {
        StopCoroutine(warningWait());
        warningText.text = message;
        StartCoroutine(warningWait());
    }

    private void updateText()
    {
        if (OC == null)
        {
            orderText.text =
                "Waiting for order(if this is here for more than a couple seconds something went wrong, please restart the game)";
        }
        else
        {
            orderText.text = OC.displayOrder();
        }
        if (DC == null)
        {
            currCup.text = "You need to grab a cup first!";
        }
        else
        {
            currCup.text = DC.checkCup();
        }
        scoreText.text = "Correct: " + correct + " | Wrong: " + incorrect;
    }

    private IEnumerator warningWait()
    {
        yield return new WaitForSeconds(3);
        warningText.text = "";
    }
}
