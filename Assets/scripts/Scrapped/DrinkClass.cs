using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrinkClass : MonoBehaviour
{
    private class Drink
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
    }
    
}
