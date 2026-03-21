using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private Player player;
    [SerializeField] private MapEditor mapEditor;

    [Header("Resource & Capacity")]
    [SerializeField] private ResourceView resourceView;
    [SerializeField] private int maxUnitCapacityOnStart;

    private TeamManager teamManager;

    

    private void Awake()
    {
        teamManager = new TeamManager(resourceView, maxUnitCapacityOnStart);
    }

    public TeamContext GetTeamContext(Team team) => teamManager.GetTeamContext(team);
}