using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Serialization;

public class GameplayManager : MonoBehaviour
{
    private DrinkCS _drink; // change this to be done by an DrinkManager
    private readonly OrderManager _orderManager = OrderManager.orderManager;

    //First playable values
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currCup;
    private int _successfulFulfillmentCount = 0;
    private int _failedFulfillmentCount = 0;


    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        _orderManager.NextOrder();
        _drink = new DrinkCS();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && _drink != null) //Give Customer Order
        {
            //  TODO: Remove temp logic here
            float ratio = _orderManager.ValidateOrder(_drink);
            if (ratio < 0.5f)
                _failedFulfillmentCount++;
            else
                _successfulFulfillmentCount++;

            _drink = null;
            _orderManager.NextOrder();
            GiveMessage("Order given to customer");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            /*
             * Interaction will either be with the ingredients, cup, or trash
             * Place these gameObjects in their own Physics layer
             * Perform based on whether they have 
             */
            if (Physics.Raycast(ray, out var hit, 100))
            {
                if (hit.transform.name == cup.transform.name)
                {
                    HandleClickCup();
                    return;
                }

                if (_drink != null)
                {
                    if (String.Equals(Tag))

                    if (hit.transform.name == trash.transform.name)
                    {
                        DC = null;
                        GiveMessage("Cup thrown away");
                    }
                }
                else
                {
                    GiveMessage("You need to grab a cup first!");
                }
            }
        }

        UpdateText();
    }

    private void HandleClickTrash()
    {
        
    }

    private void HandleClickCup()
    {
        if (_drink != null)
        {
            GiveMessage("You already have a cup, if you need to start over throw your previous one away");
        }
        else
        {
            _drink = new DrinkCS();
            Debug.Log(">>new cup grabbed<<");
            GiveMessage("New Cup Grabbed");
        }
    }

    private void HandleClickIngredient()
    {
        if (hit.transform.name == i1.transform.name)
        {
            DC.addIngredient(1);
            GiveMessage("Added " + i1.transform.name);
        }

        if (hit.transform.name == i2.transform.name)
        {
            DC.addIngredient(2);
            GiveMessage("Added " + i2.transform.name);
        }

        if (hit.transform.name == i3.transform.name)
        {
            DC.addIngredient(3);
            GiveMessage("Added " + i3.transform.name);
        }

        if (hit.transform.name == i4.transform.name)
        {
            DC.addIngredient(4);
            GiveMessage("Added " + i4.transform.name);
        }

        if (hit.transform.name == i5.transform.name)
        {
            DC.addIngredient(5);
            GiveMessage("Added " + i5.transform.name);
        }
    }

    //Functions for first Playable only
    private void GiveMessage(string message)
    {
        StopCoroutine(MessageDelay());
        warningText.text = message;
        StartCoroutine(MessageDelay());
    }

    // TODO: The purpose of this is not clear; there are duplicated calls
    private void UpdateText() 
    {
        if (_orderManager.GetOrder() == null)
        {
            orderText.text =
                "Waiting for order(if this is here for more than a couple seconds something went wrong, please restart the game)";
        }
        else
        {
            orderText.text = _orderManager.PrintOrder();
        }

        if (_drink == null)
        {
            currCup.text = "You need to grab a cup first!";
        }
        else
        {
            currCup.text = "" + _orderManager.ValidateOrder(_drink);
        }

        scoreText.text = "Correct: " + _successfulFulfillmentCount + " | Wrong: " + _failedFulfillmentCount;
    }

    private IEnumerator MessageDelay()
    {
        yield return new WaitForSeconds(3);
        warningText.text = "";
    }
}