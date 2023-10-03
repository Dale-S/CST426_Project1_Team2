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

    //Design Variables
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI warningText;
    public GameObject arrow;
    public GameObject heldCup;
    public GameObject tableCup;

    //public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currCup;
    private int _successfulFulfillmentCount = 0;
    private int _failedFulfillmentCount = 0;

    //SoundManager to play sounds
    public SoundManager _soundManager;

    private const int LayerMask = 1 << 6;

    //particle effects
    public ParticleSystem sugar, water, tea, milk, espresso;

    public Camera playerCamera;

    [Header("Icon-Count Text Elements")] public TextMeshProUGUI espressoCountText;
    public TextMeshProUGUI milkCountText;
    public TextMeshProUGUI sugarCountText;
    public TextMeshProUGUI teaCountText;
    public TextMeshProUGUI waterCountText;

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
            if (MM.currFocus() != 0 || _drink != null)
            {
                arrow.SetActive(false);
            }
            else
            {
                arrow.SetActive(true);
                tableCup.SetActive(false);
            }

            tableCup.SetActive(false);
        }
        else
        {
            arrow.SetActive(false);
            tableCup.SetActive(true);
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
                    _soundManager.PlaySoundEffect("WrongOrder");
                }
                else
                {
                    _successfulFulfillmentCount++;
                    _soundManager.PlaySoundEffect("RightOrder");
                    MH.addFunds(10);
                }

                _drink = null;
                heldCup.SetActive(false);
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
        heldCup.SetActive(false);
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
            heldCup.SetActive(true);
            Debug.Log(">>new cup grabbed<<");
            GiveMessage("New Cup Grabbed");
            if (DWM.getCups() > 0)
            {
                DWM.subtractCup();
                _soundManager.PlaySoundEffect("IceClink");
            }
        }
    }

    private void HandleClickIngredient(InteractableItem interactableItem)
    {
        interactableItem.OnInteract(_drink);
        switch (interactableItem.interactableName)
        {
            case "Water":
                _soundManager.PlaySoundEffect("MilkPour");
                water.Play();
                break;
            case "Milk":
                _soundManager.PlaySoundEffect("MilkPour");
                milk.Play();
                break;
            case "Sugar":
                _soundManager.PlaySoundEffect("SugarPour");
                sugar.Play();
                break;
            case "Espresso":
                _soundManager.PlaySoundEffect("CoffeePour");
                espresso.Play();
                break;
            case "Tea":
                _soundManager.PlaySoundEffect("CoffeePour");
                tea.Play();
                break;
        }

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

        if (_drink == null)
            FormatIconCountZero();
        else
            FormatIconCount();
    }

    private void FormatIconCountZero()
    {
        UpdateCount(espressoCountText, 0);
        UpdateCount(milkCountText, 0);
        UpdateCount(waterCountText, 0);
        UpdateCount(teaCountText, 0);
        UpdateCount(sugarCountText, 0);
    }

    private void FormatIconCount()
    {
        Dictionary<Ingredient, int> ingredientCountDict = _drink.GetIngredients();
        UpdateCount(espressoCountText,
            ingredientCountDict.ContainsKey(Ingredient.Espresso) ? ingredientCountDict[Ingredient.Espresso] : 0);        
        UpdateCount(waterCountText,
            ingredientCountDict.ContainsKey(Ingredient.Water) ? ingredientCountDict[Ingredient.Water] : 0);
        UpdateCount(teaCountText,
            ingredientCountDict.ContainsKey(Ingredient.Tea) ? ingredientCountDict[Ingredient.Tea] : 0);
        UpdateCount(milkCountText,
            ingredientCountDict.ContainsKey(Ingredient.Milk) ? ingredientCountDict[Ingredient.Milk] : 0);
        UpdateCount(sugarCountText,
            ingredientCountDict.ContainsKey(Ingredient.Sugar) ? ingredientCountDict[Ingredient.Sugar] : 0);
    }

    private void UpdateCount(TextMeshProUGUI textElement, int count)
    {
        textElement.text = count.ToString();
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