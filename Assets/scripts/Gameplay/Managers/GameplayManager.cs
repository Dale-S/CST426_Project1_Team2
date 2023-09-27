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
    //Timer values for message
    private float messageMaxTime = 3.0f;
    private float currentMessageTime = 0f;

    private bool GMActive = true;
    
    //Money Handler Instantiation
    public MoneyHandler MH;
    public DishWashingManager DWM;
    public MinigameManager MM;

    private DrinkCs _drink; // change this to be done by an DrinkManager

    //First playable values
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI warningText;
    public GameObject arrow;

    //public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currCup;
    private int _successfulFulfillmentCount = 0;
    private int _failedFulfillmentCount = 0;

    private const int LayerMask = 1 << 6;


    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        OrderManager.Instance.NextOrder();
        _drink = null;
    }

    // Update is called once per frame
    void Update()
    {
        //Warning Timer Update
        if (currentMessageTime > 0)
        {
            currentMessageTime -= 1 * Time.deltaTime;
        }
        else
        {
            warningText.text = "";
        }

        if (DWM.getCups() < 1)
        {
            if (MM.currFocus() != 0)
            {
                arrow.SetActive(false);
            }
            else
            {
                arrow.SetActive(true);
            }
        }
        else
        {
            arrow.SetActive(false);
        }

        if (GMActive)
        {
            if (Input.GetKeyDown(KeyCode.R) && _drink != null) //Give Customer Order
            {
                //  TODO: Remove temp logic here
                float ratio = OrderManager.Instance.ValidateOrder(_drink);
                if (ratio < 0.5f)
                {
                    _failedFulfillmentCount++;
                }
                else
                {
                    _successfulFulfillmentCount++;
                    MH.addFunds(10);
                }
            
                _drink = null;
                OrderManager.Instance.NextOrder();
                GiveMessage("Order given to customer");
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                /*
                * Interaction will either be with the ingredients, cup, or trash
                * Place these gameObjects in their own Physics layer
                * Perform based on whether they have the tag
                */
                if (Physics.Raycast(ray, out var hit, 100, LayerMask))
                {
                    if (hit.transform.CompareTag("Cup"))
                    {
                        HandleClickCup();
                    }
            
                    else if (_drink == null)
                    {
                        GiveMessage("You need to grab a cup first!");
                    }
            
                    else if (hit.transform.CompareTag("Ingredient"))
                    {
                        InteractableItem interactableItem = hit.transform.gameObject.GetComponent<InteractableItem>(); 
                        HandleClickIngredient(interactableItem);
                    }
            
                    else if (hit.transform.CompareTag("Trash"))
                    {
                        HandleClickTrash();
                    }
                }
            }
        }
        UpdateText();
    }

    private void HandleClickTrash()
    {
        _drink = null;
        GiveMessage("Cup thrown away");
    }

    private void HandleClickCup()
    {
        if (DWM.getCups() < 1)
        {
            GiveMessage("Out of clean cups, go wash some dishes!");
        }
        else if (_drink != null)
        {
            GiveMessage("You already have a cup, if you need to start over throw your previous one away");
        }
        else
        {
            _drink = new DrinkCs();
            Debug.Log(">>new cup grabbed<<");
            GiveMessage("New Cup Grabbed");
            if (DWM.getCups() > 0)
            {
                DWM.subtractCup();
            }
        }
    }

    private void HandleClickIngredient(InteractableItem interactableItem)
    {
        interactableItem.OnInteract(_drink);
        GiveMessage("Added " + interactableItem.interactableName);
    }

    //Functions for first Playable only
    private void GiveMessage(string message)
    {
        currentMessageTime = messageMaxTime;
        warningText.text = message;
    }

    // TODO: The purpose of this is not clear; there are duplicated calls
    private void UpdateText()
    {
        orderText.text = OrderManager.Instance.GetOrder() == null
            ? "Waiting for order(if this is here for more than a couple seconds something went wrong, please restart the game)"
            : OrderManager.Instance.PrintOrder();

        currCup.text = _drink == null
            ? "You need to grab a cup first!"
            : "" + _drink.FormatItem();

        //scoreText.text = "Correct: " + _successfulFulfillmentCount + " | Wrong: " + _failedFulfillmentCount;
    }

    public void gmState(bool value)
    {
        GMActive = value;
    }

    /*private IEnumerator MessageDelay()
    {
        yield return new WaitForSeconds(3);
        warningText.text = "";
    }*/
}