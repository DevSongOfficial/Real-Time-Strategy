using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mapEditor;

    [Header("Resource & Capacity")]
    [SerializeField] private ResourceView resourceView;
    [SerializeField] private int maxUnitCapacityOnStart;

    private TeamManager teamManager;

    private void Awake()
    {
        teamManager = new TeamManager(resourceView, maxUnitCapacityOnStart);
    }

    private void Update()
    {
#if UNITY_EDITOR
        HandleDebugToggle();
#endif
    }


    public TeamContext GetTeamContext(Team team) => teamManager.GetTeamContext(team);




    private void HandleDebugToggle()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        var active = player.activeSelf;
        player.SetActive(!active);
        mapEditor.SetActive(active);
    }
}