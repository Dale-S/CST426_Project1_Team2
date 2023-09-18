using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Order : MonoBehaviour
{
    private List<int> request; //Order in list form
    private List<int> ingCount; //Number of each ingredient
    private static string ingName1 = "ingredient1";
    private static string ingName2 = "ingredient2";
    private static string ingName3 = "ingredient3";
    private static string ingName4 = "ingredient4";
    private static string ingName5 = "ingredient5";
    public TextMeshProUGUI orderText; //temp for first playable
    private int size;
    private string[] ingNames = {ingName1, ingName2, ingName3, ingName4, ingName5};
    

    public Order()
    {
        request = new List<int> {};
        ingCount = new List<int> {};
    }

    public void createOrder()
    {
        for (int i = 0; i < 5; i++)
        {
            ingCount.Add(0);
        }
        size = Random.Range(2, 6);
        for (int i = 0; i < size; i++)
        {
            int randIngredient = Random.Range(1, 6);
            //Debug.Log(randIngredient);
            this.request.Add(randIngredient);
            ingCount[randIngredient - 1] += 1;
        }
    }

    public string displayOrder()
    {
        var sb = new System.Text.StringBuilder();

        for (int i = 0; i < 5; i++)
        {
            if (ingCount[i] > 0)
            {
                string currString = $"\n{ingCount[i]} of {ingNames[i]}";
                sb.Append(currString);
            }
        }
        //Debug.Log(sb);
        string finalString = "Order: " + sb;
        return finalString;
    }

    public List<int> returnOrder()
    {
        return this.request;
    }
}
