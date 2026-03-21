using UnityEngine;

public class TeamManager
{
    public TeamContext TeamContext_Green { get; private set; }
    public TeamContext TeamContext_Red { get; private set; }
    public TeamContext TeamContext_Blue { get; private set; }
    public TeamContext TeamContext_Neutral { get; private set; }

    public TeamManager(ResourceView resourceView, int maxUnitCapacityOnStart)
    {
        // Green (my team)
        {
            var team = Team.Green;
            var resourceBank = new ResourceBank(resourceView);
            var capacitySlots = new UnitCapacitySlots(maxUnitCapacityOnStart, resourceView);
            TeamContext_Green = new TeamContext(team, resourceBank, capacitySlots);
        }
        // Red 
        {
            var team = Team.Red;
            var resourceBank = new ResourceBank(resourceView);
            var capacitySlots = new UnitCapacitySlots(maxUnitCapacityOnStart, resourceView);
            TeamContext_Red = new TeamContext(team, resourceBank, capacitySlots);
        }
        // Blue 
        {
            var team = Team.Blue;
            var resourceBank = new ResourceBank(resourceView);
            var capacitySlots = new UnitCapacitySlots(maxUnitCapacityOnStart, resourceView);
            TeamContext_Blue = new TeamContext(team, resourceBank, capacitySlots);
        }
        // Neutral 
        {
            var team = Team.None;
            var resourceBank = new ResourceBank(resourceView);
            var capacitySlots = new UnitCapacitySlots(maxUnitCapacityOnStart, resourceView);
            TeamContext_Neutral = new TeamContext(team, resourceBank, capacitySlots);
        }
    }

    public TeamContext GetTeamContext(Team team)
    {
        switch(team)
        {
            case Team.Green: return TeamContext_Green;
            case Team.Red: return TeamContext_Red;
            case Team.Blue: return TeamContext_Blue;
            default: return TeamContext_Neutral;
        }
    }
}