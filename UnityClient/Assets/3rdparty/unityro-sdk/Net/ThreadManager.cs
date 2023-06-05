using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour {

    private static readonly List<System.Action> executeOnMainThread = new List<System.Action>();
    private static readonly List<System.Action> executeCopiedOnMainThread = new List<System.Action>();
    private static bool actionToExecuteOnMainThread = false;

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    private void Update() {
        UpdateMain();
    }

    /// <summary>Sets an action to be executed on the main thread.</summary>
    /// <param name="_action">The action to be executed on the main thread.</param>
    public static void ExecuteOnMainThread(System.Action _action) {
        if(_action == null) {
            Debug.Log("No action to execute on main thread!");
            return;
        }

        lock(executeOnMainThread) {
            executeOnMainThread.Add(_action);
            actionToExecuteOnMainThread = true;
        }
    }

    /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
    public static void UpdateMain() {
        if(actionToExecuteOnMainThread) {
            executeCopiedOnMainThread.Clear();
            lock(executeOnMainThread) {
                executeCopiedOnMainThread.AddRange(executeOnMainThread);
                executeOnMainThread.Clear();
                actionToExecuteOnMainThread = false;
            }

            for(int i = 0; i < executeCopiedOnMainThread.Count; i++) {
                executeCopiedOnMainThread[i]();
            }
        }
    }
}