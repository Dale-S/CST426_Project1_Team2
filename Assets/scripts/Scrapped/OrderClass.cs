using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderClass : MonoBehaviour
{
    private class Order
    {
        private List<int> request;
        
        public Order()
        {
            request = new List<int> {};
        }

        private void createOrder()
        {
            int size = Random.Range(1, 4);
            for (int i = 0; i < size; i++)
            {
                int randIngredient = Random.Range(1, 5);
                this.request.Add(randIngredient);
            }
        }
    }
}
