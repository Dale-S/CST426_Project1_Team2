using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drink : MonoBehaviour
{
    private List<int> ingredients;

    public Drink()
    {
        ingredients = new List<int> {};
    }

    public void addIngredient(int ingredient)
    {
        this.ingredients.Add(ingredient);
    }

    public bool check(List<int> Order)
    {
        if (this.ingredients.Count != Order.Count)
        {
            return false;
        }

        int size = this.ingredients.Count;
        this.ingredients.Sort();
        Order.Sort();

        for (int i = 0; i < size; i++)
        {
            if (ingredients.ElementAt(i) != Order.ElementAt(i))
            {
                return false;
            }
        }

        return true;
    }

    public void checkCup()
    {
        int size = ingredients.Count;
        String output = "";
        Debug.Log("\\/ \\/ \\/ \\/ \\/ \\/ \\/ \\/ \\/ \\/ \\/");
        Debug.Log("Ingredients: ");
        if (size > 0)
        {
            for (int i = 0; i < size - 1; i++)
            {
                output = output + ingredients.ElementAt(i) + ", ";
            }

            output = output + ingredients.ElementAt(size - 1);
        }
        Debug.Log(output);
        Debug.Log("/\\ /\\ /\\ /\\ /\\ /\\ /\\ /\\ /\\ /\\ /\\");
    }
}
