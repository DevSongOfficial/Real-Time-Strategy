using UnityEngine;
using CustomResourceManagement;

public abstract class PlayableAbsFactory<T> where T : Playable
{
    public abstract T CreatePlayable();
}
