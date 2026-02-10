using System;
using UnityEngine;

/// <summary>
/// TaskId identifier for each task
/// Made by Marco Espinoza
/// Last Update: 2/9/2026
/// </summary>
/// 
[Serializable]
public class Task
{
    public string taskID;
    [TextArea]
    public string description;

    public bool isCompleted;
    
}
