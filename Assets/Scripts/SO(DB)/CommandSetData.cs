using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommandSetData", menuName = "Scriptable Objects/CommandSetData")]
public class CommandSetData : ScriptableObject
{
    [field: SerializeField] public List<Command> Commands { get; private set; } = new List<Command>();
}

public enum CommandType
{
    None,
    Build,
    TrainUnit,
}

[Serializable]
public class Command
{
    public CommandType Type => entityToGenerate is BuildingData ? CommandType.Build : CommandType.TrainUnit;
    public Sprite icon;
    public string tooltip;


    // A building to build or unit to train
    public EntityData entityToGenerate;
    public float generationTime;
}
