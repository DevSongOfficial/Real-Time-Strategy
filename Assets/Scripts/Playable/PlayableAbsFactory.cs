using UnityEngine;
using CustomResourceManagement;

public abstract class PlayableAbsFactory<T> where T : Playable
{
    public abstract T Create(EntityData data, Team team);
}
