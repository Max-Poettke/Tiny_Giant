using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

[System.Serializable]
public struct BowState : INetworkStruct
{
    public float thumbTouchedCommand;
    public float indexTouchedCommand;
    public float gripCommand;
    public float triggerCommand;
    // Optionnal commands
    public int poseCommand;
    public float pinchCommand;// Can be computed from triggerCommand by default
}
