using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour, IGameManager {
    private Dictionary<string, int> items;

    public ManagerStatus status { get; private set; }

    public void Startup() {
        Debug.Log("InventoryManager started");

        UpdateData(new Dictionary<string, int>());

        status = ManagerStatus.Started;
    }

    public void DisplayItems() {
        var itemDisplay = "Items: ";
        foreach(KeyValuePair<string, int> item in items) {
            itemDisplay += item.Key + " (" + item.Value + ") ";
        }

        Debug.Log(itemDisplay);
    }

    public void AddItem(string name) {
        if(items.ContainsKey(name)) {
            items[name] += 1;
        } else {
            items[name] = 1;
        }

        DisplayItems();
    }

    public List<string> GetItemList() => new List<string>(items.Keys);

    public int GetItemCount(string name) => items.ContainsKey(name) ? items[name] : 0;

    public string EquippedItem { get; private set; }

    public bool EquipItem(string name) {
        if(items.ContainsKey(name) && EquippedItem != name) {
            EquippedItem = name;
            Debug.Log($"Equipped {name}");

            return true;
        }

        EquippedItem = null;
        Debug.Log("Unequipped");

        return false;
    }

    public bool ConsumeItem(string name) {
        if(items.ContainsKey(name)) {
            items[name]--;
            if(items[name] == 0) {
                items.Remove(name);
            }
        } else {
            Debug.Log($"cannot consume {name}");
            return false;
        }

        DisplayItems();
        return true;
    }

    public void UpdateData(Dictionary<string, int> items) {
        this.items = items;
    }

    public Dictionary<string, int> GetData() => items;
}