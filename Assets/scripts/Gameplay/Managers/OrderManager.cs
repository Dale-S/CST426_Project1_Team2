using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    /* Represents the expected Order and its Item(s) */
    public static OrderCs CurrentOrder;
    
    void Awake()
    {
        Instance = this;
        CurrentOrder = new OrderCs();
    }

    public void NextOrder()
    {
        CurrentOrder.InitRandomItem();
    }

    public string PrintOrder()
    {
        return CurrentOrder.GetItem().FormatItem();
    }

    public OrderCs GetOrder()
    {
        return CurrentOrder;
    }

    public float ValidateOrder(DrinkCs given)
    {
        return CurrentOrder.ItemAccuracy(given);
    }
}