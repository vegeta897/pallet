using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rack : MonoBehaviour
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
            ShelfMaxWeight = maxWeight;
            maxWidth = numPalletWidths;
        }
        
        public bool PutItem(StorageItem item)
        {
            if (AvailableWidth + item.Width <= MaxWidth &&
                RemainingWeight + item.Weight <= ShelfMaxWeight)
            {
                StoredItems.Add(item);
                AvailableWidth -= item.Width;
                RemainingWeight -= item.Weight;

                return true;
            }

            return false;
        }

        public StorageItem GetItem(StorageItem item)
        {
            var storedItem = StoredItems.Find(i => i == item);
            if (storedItem != null)
            {
                // Found item. Remove from inventory and alter capacity.
                StoredItems.Remove(storedItem);
                AvailableWidth += item.Width;
                RemainingWeight += item.Weight;

                return storedItem;
            }

            // Found nothing
            return null;
        }

        // Method to check if shelf has any free room to put a storage item.
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

    public Rack(float numPalletWidths, int totalHeight, int numShelves, int maxCapacity)
    {
        for (int i = 1; i <= numShelves; i++)
        {
            Shelves.Add(i, new Shelf(numPalletWidths, numShelves / maxCapacity));
        }
    }

    // Method to check if item exists. Delegate object may be able to be used?

    // GetGoods()

    // PutGoods()




    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
