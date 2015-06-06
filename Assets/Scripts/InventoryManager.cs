using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour
{

    Rack rack;

    // Use this for initialization
    void Start()
    {
        rack = ScriptableObject.CreateInstance<Rack>();
        rack.Initialize(2, 10, 4, 8000);

        StorageItem pallet1 = ScriptableObject.CreateInstance<StorageItem>();
        StorageItem bin1 = ScriptableObject.CreateInstance<StorageItem>();
        StorageItem bin2 = ScriptableObject.CreateInstance<StorageItem>();

        pallet1.Initialize(itemType: StorageItem.StorageItemType.Pallet, weight: 450, width: 1, maxWeight: 450);
        bin1.Initialize(itemType: StorageItem.StorageItemType.Bin, width: 0.5f, storedItemBaseWeight: 1, maxWeight: 750);
        bin2.Initialize(itemType: StorageItem.StorageItemType.Bin, width: 0.5f, storedItemBaseWeight: 2, maxWeight: 750);


        Debug.Log(rack.ToString());
        rack.PutStorageItem(pallet1);
        Debug.Log(rack.ToString());
        rack.PutStorageItem(bin1);
        Debug.Log(rack.ToString());
        rack.PutStorageItem(bin2);
        Debug.Log(rack.ToString());
        Debug.Log("****************");
        Debug.Log("Adding 500 items");
        Debug.Log("Status: " + rack.PutGoods(500));
        Debug.Log(rack.ToString());
        Debug.Log("Adding 368 items");
        Debug.Log("Status: " + rack.PutGoods(368));
        Debug.Log(rack.ToString());
        Debug.Log("Adding 367 items");
        Debug.Log("Status: " + rack.PutGoods(367));
        Debug.Log(rack.ToString());

    }

    // Update is called once per frame
    void Update()
    {

    }
}
