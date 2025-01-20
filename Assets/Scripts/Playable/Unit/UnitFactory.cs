using Unity.AI.Navigation.Editor;
using UnityEditor;
using UnityEngine;
using CustomResourceManagement;


public class UnitFactory : PlayableAbsFactory<Unit>
{
    public override Unit CreatePlayable()
    {
        return new Unit();
    }
}