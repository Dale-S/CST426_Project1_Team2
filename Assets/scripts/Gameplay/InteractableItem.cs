using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [Header("Item Ingredient and Storage")]
    public Ingredient itemIngredient = Ingredient.Water;
    public int ingredientCount = 10;

    public string interactableName = "water";

    // To be used when the items need to have cool downs
    // private float _coolDownTime = 0.0f;

    public void OnInteract(DrinkCs currentDrink)
    {
        currentDrink.AddIngredient(itemIngredient, 1);
    }
}
