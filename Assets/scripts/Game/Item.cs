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

public abstract class Item
{
    private readonly string _name;
    private readonly float _cost;
    private ItemType _type;
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

    public void SetItemType(ItemType type)
    {
        _type = type;
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
}

public class DrinkCS : Item, IHasIngredients
{
    private Dictionary<Ingredient, int> _ingredientCount;

    public DrinkCS() : base("", 0.0f, ItemType.Null, true)
    {
    }

    public DrinkCS(string name, float cost, ItemType type, bool hasIngredients) : base(name, cost, type, hasIngredients)
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
        {
            CheckForPrimary(ingredient);
            _ingredientCount[ingredient] = count;
        }
    }

    private void CheckForPrimary(Ingredient ingredient)
    {
        // If type is water or null, continue
        if (GetItemType() != ItemType.Water || GetItemType() != ItemType.Null) return;

        switch (ingredient)
        {
            case Ingredient.SingleShot:
                SetItemType(ItemType.Coffee);
                break;
            case Ingredient.Teabag:
                SetItemType(ItemType.Tea);
                break;
            case Ingredient.Water:
                SetItemType(ItemType.Water);
                break;
        }
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

    public float CompareItem(DrinkCS given)
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
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                AssignCoffee();
                break;
            case 1:
                AssignTea();
                break;
            case 2:
                AssignWater();
                break;
            default:
                Debug.LogError($"Error creating random order object with rand {rand}");
                break;
        }
    }

    public DrinkCS GetItem()
    {
        return _orderItem;
    }

    private void AssignCoffee()
    {
        SetItem(new DrinkCS("latte", 4.0f, ItemType.Coffee, true));
        _orderItem.ProvideIngredients(new Dictionary<Ingredient, int>
        {
            { Ingredient.SingleShot, 2 }, { Ingredient.Milk, 1 }, { Ingredient.Sugar, 2 }
        });
    }

    private void AssignTea()
    {
        SetItem(new DrinkCS("tea", 3.0f, ItemType.Tea, true));
        _orderItem.ProvideIngredients(new Dictionary<Ingredient, int>
        {
            { Ingredient.Teabag, 2 }, { Ingredient.Water, 2 }
        });
    }

    private void AssignWater()
    {
        SetItem(new DrinkCS("water", 2.0f, ItemType.Water, true));
        _orderItem.ProvideIngredients(new Dictionary<Ingredient, int>
        {
            { Ingredient.Water, 3 }
        });
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