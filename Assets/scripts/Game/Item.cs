using System.Collections.Generic;
using UnityEngine;

public enum Ingredient // Specifics are temporary
{
    Sugar,
    Milk,
    Syrup,
    Half,
    Cinnamon,
    SingleShot,
    Water,
    Teabag
}

public enum ItemType // Specifics are temporary
{
    Tea,
    Coffee,
    Water,
    Muffin,
    Apple,
}

public interface IHasIngredients
{
    public Dictionary<Ingredient, int> GetIngredients();
    public void ProvideIngredients(Dictionary<Ingredient, int> ingredientCount);
}

public class Item
{
    private readonly string _name;
    private readonly float _cost;
    private readonly ItemType _type;
    private readonly bool _hasIngredients;

    protected Item(string name, float cost, ItemType type, bool hasIngredients)
    {
        _name = name;
        _cost = cost;
        _type = type;
        _hasIngredients = hasIngredients;
    }

    public ItemType GetItemType()
    {
        return _type;
    }

    public string GetName()
    {
        return _name;
    }

    public float GetCost()
    {
        return _cost;
    }

    public bool HasIngredients()
    {
        return _hasIngredients;
    }
}

public class DrinkCS : Item, IHasIngredients
{
    private Dictionary<Ingredient, int> _ingredientCount;

    public DrinkCS(string name, float cost, ItemType type) : base(name, cost, type, true)
    {
        _ingredientCount = new Dictionary<Ingredient, int>();
    }

    public Dictionary<Ingredient, int> GetIngredients()
    {
        return null;
    }

    public void ProvideIngredients(Dictionary<Ingredient, int> ingredientCount)
    {
        _ingredientCount = ingredientCount;
    }
}

public class Food : Item
{
    public Food(string name, float cost, ItemType type) : base(name, cost, type, false)
    {
    }
}


public class OrderCs
{
    public List<Item> GetItems()
    {
        // Could be useful
        if (new Food("latte", 5.5f, ItemType.Coffee).GetType() == typeof(Drink))
        {
            Debug.Log("hello");
        }

        return new List<Item>
        {
            new DrinkCS("latte", 5.5f, ItemType.Coffee),
            new Food("bread", 5.5f, ItemType.Muffin)
        };
    }

    // number of expected items v. number of expected items received
    // each item found adds a value between 0 and 1
    // sum of these values / number of expected items == final ratio
    // extra, unexpected items may have some const reduction to our sum
    // at worst, we give the item(s) for no charge
    // at a certain total ratio, on a curve, we add a tip as a percentage of penultimate payment

    /// <summary>
    /// Checks the accuracy of the attempted fulfillment for this Order.
    /// </summary>
    /// <param name="provided"> List of type Item for items given to the customer </param>
    /// <returns> Ratio of the cost given as payment based on the accuracy of the order fulfillment [0,1] </returns>
    public float FulfillmentAccuracy(List<Item> provided)
    {
        float correctness = 0f;
        provided.ForEach(delegate(Item item) { correctness += ItemAccuracy(item); });
        return correctness;
    }

    /// <summary>
    /// Checks the accuracy of a provided item. 
    /// </summary>
    /// <param name="item"> Provided item </param>
    /// <returns> Ratio of the cost given as payment based on the accuracy of the item fulfillment [0,1] </returns>
    private float ItemAccuracy(Item item)
    {
        if (item.GetItemType() != ItemType.Coffee) return 0f;
        if (!item.HasIngredients()) return 1f;

        IHasIngredients d = (IHasIngredients)item;
        return IngredientsAccuracy(d.GetIngredients());
    }

    /// <summary>
    /// Checks the accuracy of a provided item's ingredients. 
    /// </summary>
    /// <param name="providedIngredientCount"> Mapped ingredient-count of provided item </param>
    /// <returns> Ratio of the cost given as payment based on the accuracy of the item & ingredients fulfillment [0,1] </returns>
    private float IngredientsAccuracy(Dictionary<Ingredient, int> providedIngredientCount)
    {
        // Use maps (expected v provided) and loop (for each on the provided map) to calc a correctness ratio
        return 1.0f;
    }
}