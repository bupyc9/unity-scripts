using System;

public interface IGameManager {
    ManagerStatus status { get; }

    void Startup();
}