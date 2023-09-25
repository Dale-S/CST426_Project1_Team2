using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager orderManager;

    /* Represents the expected Order and its Item(s) */
    public static OrderCs CurrentOrder;


    // Start is called before the first frame update
    void Start()
    {
        orderManager = this;
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

    public float ValidateOrder(DrinkCS given)
    {
        return CurrentOrder.ItemAccuracy(given);
    }
}