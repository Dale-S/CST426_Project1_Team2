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
    // Drink
    Tea,
    Coffee,
    Water,
    Null
}

public interface IHasIngredients
{
    public Dictionary<Ingredient, int> GetIngredients();
    public void ProvideIngredients(Dictionary<Ingredient, int> ingredientCount);
    public void AddIngredient(Ingredient ingredient, int count);
    public float IngredientsAccuracy(Dictionary<Ingredient, int> expected);
}

public abstract class Item<T>
{
    private readonly string _name;
    private readonly float _cost;
    private readonly ItemType _type;
    private readonly bool _hasIngredients;

    protected Item()
    {
        _name = "";
        _cost = 0.0f;
        _type = ItemType.Null;
        _hasIngredients = false;
    }

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

    public virtual string FormatItem()
    {
        return _name;
    }

    public abstract float CompareItem(T given);
}

public class DrinkCS : Item<DrinkCS>, IHasIngredients
{
    private Dictionary<Ingredient, int> _ingredientCount;

    public DrinkCS() : base("", 0.0f, ItemType.Null, true)
    {
    }

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

    public void AddIngredient(Ingredient ingredient, int count)
    {
        if (_ingredientCount.ContainsKey(ingredient))
            _ingredientCount[ingredient] += count;
        else
            _ingredientCount[ingredient] = count;
    }

    public override string FormatItem()
    {
        var sb = new System.Text.StringBuilder();

        foreach (var entry in _ingredientCount)
        {
            if (entry.Value > 0)
            {
                string currString = $"\n{entry.Value} of {entry.Key}";
                sb.Append(currString);
            }
        }

        return "Order: " + sb;
    }

    public override float CompareItem(DrinkCS given)
    {
        if (GetItemType() != given.GetItemType()) return 0f;
        if (!given.HasIngredients()) return 1f;

        IHasIngredients givenWithIngredients = given;
        return IngredientsAccuracy(givenWithIngredients.GetIngredients());
    }

    /// <summary>
    /// Checks the accuracy of a provided item's ingredients. 
    /// </summary>
    /// <param name="providedCount"> Mapped ingredient-count of provided item </param>
    /// <returns> Ratio of the cost given as payment based on the accuracy of the item & ingredients fulfillment [0,1] </returns>
    public float IngredientsAccuracy(Dictionary<Ingredient, int> providedCount)
    {
        // Does each ingredient exist? Do the counts match? Are there extra ingredients?
        int correct = 0, incorrect = 0, missing = _ingredientCount.Count;

        foreach (var entry in providedCount)
        {
            if (!providedCount.ContainsKey(entry.Key)) /* Key not expected */
            {
                incorrect++;
            }
            else if (entry.Value != _ingredientCount[entry.Key]) /* Found wrong value for key */
            {
                missing--;
                incorrect++;
            }
            else /* Found correct value or key */
            {
                correct++;
                missing--;
            }
        }

        float sum = correct - missing - incorrect;
        return Mathf.Clamp01(sum / _ingredientCount.Count);
    }
}

public class OrderCs
{
    private DrinkCS _orderItem;

    private void SetItem(DrinkCS item)
    {
        _orderItem = item;
    }

    public void InitRandomItem()
    {
        // TODO:
    }

    public Item<DrinkCS> GetItem()
    {
        return _orderItem;
    }

    /// <summary>
    /// NOT TO BE USED - Checks the accuracy of the attempted fulfillment for this Order.
    /// </summary>
    /// <param name="provided"> List of type Item for items given to the customer </param>
    /// <returns> Ratio of the cost given as payment based on the accuracy of the order fulfillment [0,1] </returns>
    public float FulfillmentAccuracy(List<DrinkCS> provided)
    {
        float correctness = 0f;
        provided.ForEach(delegate(DrinkCS item) { correctness += ItemAccuracy(item); });
        return correctness;
    }

    /// <summary>
    /// Checks the accuracy of a provided item compared to private _orderItem. 
    /// </summary>
    /// <param name="given"> Provided item </param>
    /// <returns> Ratio of the cost given as payment based on the accuracy of the item fulfillment [0,1] </returns>
    public float ItemAccuracy(DrinkCS given)
    {
        return _orderItem.CompareItem(given);
    }
}