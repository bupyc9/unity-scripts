using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    public int curLevel { get; private set; }
    public int maxLevel { get; private set; }

    public void Startup() {
        Debug.Log("Mission manager starting...");

        UpdateData(0, 3);

        status = ManagerStatus.Started;
    }

    public void GoToNext() {
        if(curLevel < maxLevel) {
            curLevel++;
            var name = $"Level{curLevel}";
            Debug.Log($"Loading {name}");
            SceneManager.LoadScene(name);
        } else {
            Debug.Log("Last level");
            Messenger.Broadcast(GameEvent.GAME_COMPLETE);
        }
    }

    public void ReachObjective() {
        Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
    }

    public void RestartCurrent() {
        var name = $"Level{curLevel}";
        Debug.Log($"Loading {name}");

        SceneManager.LoadScene(name);
    }

    public void UpdateData(int curLevel, int maxLevel) {
        this.curLevel = curLevel;
        this.maxLevel = maxLevel;
    }
}
