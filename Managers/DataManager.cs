using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    private string fileName;

    public void Startup() {
        Debug.Log("Data manager starting...");

        fileName = Path.Combine(Application.persistentDataPath, "game.dat");

        status = ManagerStatus.Started;
    }

    public void SaveGameState() {
        var gamestate = new Dictionary<string, object>();

        //gamestate.Add("inventory", Managers.Inventory.GetData());
        //gamestate.Add("health", Managers.Player.health);
        //gamestate.Add("maxHealth", Managers.Player.maxHealth);
        //gamestate.Add("curLevel", Managers.Mission.curLevel);
        //gamestate.Add("maxLevel", Managers.Mission.maxLevel);

        var stream = File.Create(fileName);
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, gamestate);
        stream.Close();
    }

    public void LoadGameState() {
        if (!File.Exists(fileName)) {
            Debug.Log("No saved game");
            return;
        }

        Dictionary<string, object> gamestate;
        var formatter = new BinaryFormatter();
        var stream = File.Open(fileName, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        //Managers.Inventory.UpdateData((Dictionary<string, int>)gamestate["inventory"]);
        //Managers.Player.UpdateData((int)gamestate["health"], (int)gamestate["maxHealth"]);
        //Managers.Mission.UpdateData((int)gamestate["curLevel"], (int)gamestate["maxLevel"]);
        //Managers.Mission.RestartCurrent();
    }
}
