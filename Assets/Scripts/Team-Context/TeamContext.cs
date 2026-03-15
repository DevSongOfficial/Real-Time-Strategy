using System.Collections.Generic;
using UnityEngine;

public sealed class TeamContext
{
    public Team Team { get; }
    public ResourceBank ResourceBank { get; }
    public UnitCapacitySlots CapacitySlots { get; }
    public HeadQuarters HeadQuarters { get; private set; }

    public TeamContext(Team team, ResourceBank resourceBank,UnitCapacitySlots capacitySlots)
    {
        Team = team;
        ResourceBank = resourceBank;
        CapacitySlots = capacitySlots;
    }

    public void SetHeadQuarters(HeadQuarters headQuarters)
    {
        HeadQuarters = headQuarters;
    }
}