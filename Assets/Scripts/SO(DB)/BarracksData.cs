using UnityEngine;

[CreateAssetMenu(fileName = "BarracksData", menuName = "Scriptable Objects/BarracksData")]
public class BarracksData : BuildingData
{
    [field: SerializeField] public int GenerationSlotCount;
}
