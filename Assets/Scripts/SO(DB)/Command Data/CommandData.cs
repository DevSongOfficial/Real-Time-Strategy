using UnityEngine;

public enum CommandType
{
    None,
    Build,
    UnitTrain,
}

public abstract class CommandData : ScriptableObject // Each button in the Command Panel corresponds to each command SO.
{
    public abstract CommandType Type { get; }

    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string Tooltip { get; private set; }
}

