using System;
using System.Collections;
using System.Collections.Generic;
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

    private Camera cam;

    private bool _iscamNotNull;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (DC == null)
            {
                Debug.Log("You need to grab a cup first!");
            }
            else
            {
                DC.checkCup();
            }
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
                        Debug.Log("You already have a cup, if you need to start over throw your previous one away");
                    }
                    else
                    {
                        DC = this.AddComponent<Drink>();
                        Debug.Log(">>new cup grabbed<<");
                    }
                }
                if (DC != null)
                {
                    if (hit.transform.name == i1.transform.name)
                    {
                        DC.addIngredient(1);
                        Debug.Log(">>added " + i1.transform.name + "<<");
                    }
                
                    if (hit.transform.name == i2.transform.name)
                    {
                        DC.addIngredient(2);
                        Debug.Log(">>added " + i2.transform.name + "<<");
                    }
                
                    if (hit.transform.name == i3.transform.name)
                    {
                        DC.addIngredient(3);
                        Debug.Log(">>added " + i3.transform.name + "<<");
                    }
                
                    if (hit.transform.name == i4.transform.name)
                    {
                        DC.addIngredient(4);
                        Debug.Log(">>added " + i4.transform.name + "<<");
                    }
                
                    if (hit.transform.name == i5.transform.name)
                    {
                        DC.addIngredient(5);
                        Debug.Log(">>added " + i5.transform.name + "<<");
                    }

                    if (hit.transform.name == trash.transform.name)
                    {
                        DC = null;
                    }
                }
                else
                {
                    Debug.Log("You need to grab a cup first!");
                }
            }
        }
    }
}
