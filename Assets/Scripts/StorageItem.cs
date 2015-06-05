using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StorageItem : MonoBehaviour
{
    // Temporary
    private int storedGoods;
    public int StoredGoods
    {
        get { return storedGoods; }
        set { storedGoods = value; }
    }

    // Temporary
    private int storedGoodBaseWeight;
    public int StoredGoodBaseWeight
    {
        get { return storedGoodBaseWeight; }
        set { storedGoodBaseWeight = value; }
    }

    private int baseWeight;
    public int BaseWeight
    {
        get { return baseWeight; }
        set { baseWeight = value; }
    }

    private float width;
    public float Width
    {
        get { return width; }
        set { width = value; }
    }

    public int Weight
    {
        // Base weight of the storage item plus the weight for all stored items.
        get { return BaseWeight + (StoredGoods * StoredGoodBaseWeight); }
    }

    private int maxWeight;
    public int MaxWeight
    {
        get { return maxWeight; }
        set { maxWeight = value; }
    }

    public StorageItem(float width = 1, int weight = 15, int maxWeight = 1000, int storedItemBaseWeight = 1)
    {
        StoredGoods = 0;
        BaseWeight = weight;
        MaxWeight = maxWeight;
        Width = width;
        StoredGoodBaseWeight = storedItemBaseWeight;
    }

    // Temporary
    public int RemoveGoods(int quantity)
    {
        if (StoredGoods >= quantity)
        {
            StoredGoods -= quantity;
            return quantity;
        }

        return 0;
    }

    public bool AddGoods(int quantity)
    {
        if (Weight + (quantity * StoredGoodBaseWeight) <= MaxWeight)
        {
            StoredGoods += quantity;
            return true;
        }

        return false;
    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
