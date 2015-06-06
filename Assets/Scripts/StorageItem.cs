using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StorageItem : ScriptableObject
{
    public enum StorageItemType
    {
        Pallet,
        Bin
    }


    private StorageItemType itemType;
    public StorageItemType ItemType
    {
        get { return itemType; }
        private set { itemType = value; }
    }


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
        Debug.Log("Attempting to add goods in: " + ItemType);
        Debug.Log(String.Format("Weight: {0} Quantity: {1} StoredGoodBaseWeight: {2} MaxWeight: {3}", Weight, quantity, StoredGoodBaseWeight, MaxWeight));

        if (Weight + (quantity * StoredGoodBaseWeight) <= MaxWeight)
        {
            StoredGoods += quantity;
            return true;
        }

        return false;
    }

    public void Initialize(StorageItemType itemType, float width = 1, int weight = 15, int maxWeight = 1000, int storedItemBaseWeight = 1)
    {
        ItemType = itemType;
        StoredGoods = 0;
        BaseWeight = weight;
        MaxWeight = maxWeight;
        Width = width;
        StoredGoodBaseWeight = storedItemBaseWeight;
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
