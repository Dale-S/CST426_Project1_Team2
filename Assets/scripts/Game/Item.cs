using System.Collections.Generic;
//using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public enum Ingredient // Specifics are temporary
{
    Sugar,
    Milk,
    Syrup,
    Half,
    Cinnamon,
    Espresso,
    Water,
    Tea
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

    protected ItemType GetItemType()
    {
        return _type;
    }

    protected void SetItemType(ItemType type)
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

    protected bool HasIngredients()
    {
        return _hasIngredients;
    }

    public virtual string FormatItem()
    {
        return _name;
    }
}

public class DrinkCs : Item, IHasIngredients
{
    private Dictionary<Ingredient, int> _ingredientCount;

    public DrinkCs() : base("", 0.0f, ItemType.Null, true)
    {
        _ingredientCount = new Dictionary<Ingredient, int>();
    }

    public DrinkCs(string name, float cost, ItemType type, bool hasIngredients) : base(name, cost, type, hasIngredients)
    {
        _ingredientCount = new Dictionary<Ingredient, int>();
    }

    public Dictionary<Ingredient, int> GetIngredients()
    {
        return _ingredientCount;
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
            _ingredientCount[ingredient] = count;
        }

        CheckForPrimary(ingredient);
    }

    private void CheckForPrimary(Ingredient ingredient)
    {
        // If type is water or null, continue
        if (GetItemType() != ItemType.Water && GetItemType() != ItemType.Null) return;

        switch (ingredient)
        {
            case Ingredient.Espresso:
                SetItemType(ItemType.Coffee);
                break;
            case Ingredient.Tea:
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

        return "Current Cup: " + sb;
    }

    public float CompareItem(DrinkCs given)
    {
        Debug.Log($"{GetItemType()} {given.GetItemType()}");
        if (GetItemType() != given.GetItemType()) return 0f;
        if (!given.HasIngredients()) return 1f;

        IHasIngredients givenWithIngredients = given;
        return IngredientsAccuracy(givenWithIngredients.GetIngredients());
    }

    /// <summary>
    /// Checks the accuracy of a provided item's ingredients.
    /// TODO: Adjust the method used to find the final ratio
    /// </summary>
    /// <param name="providedCount"> Mapped ingredient-count of provided item </param>
    /// <returns> Ratio of the cost given as payment based on the accuracy of the item & ingredients fulfillment [0,1] </returns>
    public float IngredientsAccuracy(Dictionary<Ingredient, int> providedCount)
    {
        // Does each ingredient exist? Do the counts match? Are there extra ingredients?
        int correct = 0, incorrect = 0, missing = _ingredientCount.Count;

        foreach (var entry in providedCount)
        {
            if (!_ingredientCount.ContainsKey(entry.Key)) /* Key not expected */
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
        Debug.Log(sum);

        return Mathf.Clamp01(sum / _ingredientCount.Count);
    }
}

public class OrderCs
{
    private DrinkCs _orderItem;

    private void SetItem(DrinkCs item)
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

    public DrinkCs GetItem()
    {
        return _orderItem;
    }

    private void AssignCoffee()
    {
        const int espressoMin = 1;
        const int espressoMaxExclusive = 4;
        const int milkMin = 1;
        const int milkMaxExclusive = 3;
        const int sugarMin = 1;
        const int sugarMaxExclusive = 4;

        SetItem(new DrinkCs("latte", 4.0f, ItemType.Coffee, true));
        _orderItem.ProvideIngredients(new Dictionary<Ingredient, int>
        {
            { Ingredient.Espresso, Random.Range(espressoMin, espressoMaxExclusive) },
            { Ingredient.Milk, Random.Range(milkMin, milkMaxExclusive) },
            { Ingredient.Sugar, Random.Range(sugarMin, sugarMaxExclusive) }
        });
    }

    private void AssignTea()
    {
        const int teaMin = 2;
        const int teaMaxExclusive = 5;
        const int waterMin = 1;
        const int waterMaxExclusive = 4;
        
        SetItem(new DrinkCs("tea", 3.0f, ItemType.Tea, true));
        _orderItem.ProvideIngredients(new Dictionary<Ingredient, int>
        {
            { Ingredient.Tea, Random.Range(teaMin, teaMaxExclusive) },
            { Ingredient.Water, Random.Range(waterMin, waterMaxExclusive) }
        });
    }

    private void AssignWater()
    {
        const int waterMin = 2;
        const int waterMaxExclusive = 6;
        
        SetItem(new DrinkCs("water", 2.0f, ItemType.Water, true));
        _orderItem.ProvideIngredients(new Dictionary<Ingredient, int>
        {
            { Ingredient.Water, Random.Range(waterMin, waterMaxExclusive) }
        });
    }

    /// <summary>
    /// Checks the accuracy of a provided item compared to private _orderItem. 
    /// </summary>
    /// <param name="given"> Provided item </param>
    /// <returns> Ratio of the cost given as payment based on the accuracy of the item fulfillment [0,1] </returns>
    public float ItemAccuracy(DrinkCs given)
    {
        return _orderItem.CompareItem(given);
    }
}