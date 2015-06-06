using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Rack : ScriptableObject
{

    private class Shelf
    {

        private List<StorageItem> storedItems;
        public List<StorageItem> StoredItems
        {
            get { return storedItems; }
            set { storedItems = value; }
        }

        private int shelfMaxWeight;
        public int ShelfMaxWeight
        {
            get { return shelfMaxWeight; }
            set { shelfMaxWeight = value; }
        }

        private int remainingWeight;
        public int RemainingWeight
        {
            get { return remainingWeight; }
            set { remainingWeight = value; }
        }

        private float availableWidth;
        public float AvailableWidth
        {
            get { return availableWidth; }
            set { availableWidth = value; }
        }

        private float maxWidth;
        public float MaxWidth
        {
            get { return maxWidth; }
            set { maxWidth = value; }
        }

        public Shelf(float numPalletWidths, int maxWeight)
        {
            StoredItems = new List<StorageItem>();

            ShelfMaxWeight = maxWeight;
            RemainingWeight = maxWeight;
            MaxWidth = numPalletWidths;
            AvailableWidth = numPalletWidths;
        }

        public void UpdateWeight()
        {
            var weight = 0;

            foreach (var storedItem in StoredItems)
            {
                weight += storedItem.Weight;
            }

            RemainingWeight = ShelfMaxWeight - weight;
        }

        private bool CheckSpace(StorageItem item)
        {
            return (AvailableWidth - item.Width >= 0 &&
                    RemainingWeight - item.Weight >= 0);
        }

        public bool PutStorageItem(StorageItem item)
        {
            if (CheckSpace(item))
            {
                StoredItems.Add(item);
                AvailableWidth -= item.Width;
                RemainingWeight -= item.Weight;

                Debug.Log(String.Format("Putting Item. Weight: {0} - Available Weight: {0}", item.Weight, RemainingWeight));

                return true;
            }

            return false;
        }

        public StorageItem GetStorageItem(StorageItem.StorageItemType item)
        {
            var storedItem = StoredItems.Find(i => i.ItemType == item);
            if (storedItem != null)
            {
                // Found item. Remove from inventory and alter capacity.
                StoredItems.Remove(storedItem);
                AvailableWidth += storedItem.Width;
                RemainingWeight += storedItem.Weight;

                return storedItem;
            }

            // Found nothing
            return null;
        }


        public int GetGoods(int quantity)
        {
            var storageItem = StoredItems.FirstOrDefault(storage => storage.StoredGoods >= quantity);

            if (storageItem != null)
            {
                storageItem.RemoveGoods(quantity);
                UpdateWeight();
                return quantity;
            }

            return 0;
        }

        public bool PutGoods(int quantity)
        {
            var storageItem = StoredItems.FirstOrDefault(storage => storage.AddGoods(quantity));

            if (storageItem != null)
            {
                Debug.Log("Put goods in: " + storageItem.ItemType);
                UpdateWeight();
                return true;
            }

            return false;
        }
    }





    Dictionary<int, Shelf> shelves;
    private Dictionary<int, Shelf> Shelves
    {
        get { return shelves; }
        set { shelves = value; }
    }

    public int RackMaxCapacity
    {
        get
        {
            int capacity = 0;
            for (int i = 0; i < Shelves.Count; i++)
            {
                capacity += Shelves[i].ShelfMaxWeight;
            }
            return capacity;
        }
    }

    // Temporary. instead of returning an int, will return actual goods.
    public int GetGoods(int quantity)
    {
        foreach (var shelf in Shelves)
        {
            var returnValue = shelf.Value.GetGoods(quantity);

            if (returnValue == quantity)
            {
                return returnValue;
            }
        }

        return 0;
    }


    // PutGoods()
    public bool PutGoods(int quantity)
    {
        foreach (var shelf in Shelves)
        {
            if (shelf.Value.PutGoods(quantity))
            {
                return true;
            }
        }

        return false;
    }

    public StorageItem GetStorageItem(StorageItem.StorageItemType type)
    {
        foreach (var shelf in Shelves)
        {
            StorageItem item = shelf.Value.GetStorageItem(type);

            if (item != null)
            {
                return item;
            }
        }

        return null;
    }

    public bool PutStorageItem(StorageItem item)
    {
        foreach (var shelf in Shelves)
        {
            if (shelf.Value.PutStorageItem(item))
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return String.Format("Number of Shelves Free: {0} - Available Width: {1} - Current Weight: {2} - Remaining Weight: {3}", Shelves.Count(s => s.Value.AvailableWidth > 0),
                                                                                                                                 Shelves.Sum(s => s.Value.AvailableWidth),
                                                                                                                                 Shelves.Sum(s => s.Value.ShelfMaxWeight - s.Value.RemainingWeight),
                                                                                                                                 Shelves.Sum(s => s.Value.RemainingWeight));
    }

    public void Initialize(float numPalletWidths, int totalHeight, int numShelves, int maxCapacity)
    {
        Shelves = new Dictionary<int, Shelf>();

        for (int i = 1; i <= numShelves; i++)
        {
            Shelves.Add(i, new Shelf(numPalletWidths, maxCapacity / numShelves));
        }
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
