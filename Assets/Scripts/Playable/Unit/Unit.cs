using Unity.AppUI.Core;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;


[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Playable, IDamageable, ITargetor, ITarget, IUnitStateContext
{
    [SerializeField] private CoroutineExecutor coroutineExecutor;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private new Collider collider; // collider for calculating position delta.
    public float PositionDeltaY { get; private set; }


    private GameObject selectionIndicator;
    private IPlacementEvent placementEvent;
    private HealthSystem healthSystem;
    protected EntityProfilePanel profilePanel;

    // Resource
    protected ResourceBank teamResourceBank; 
    protected ResourceBank resourceBank; // For managing this unit's currently holding resources.

    protected virtual void Awake()
    {
        if (coroutineExecutor != null)
            coroutineExecutor = GetComponent<CoroutineExecutor>();

        if(agent == null)
            agent = GetComponent<NavMeshAgent>();

        if(animator == null)
            animator = GetComponent<Animator>();

        if (collider == null)
            collider = GetComponentInChildren<Collider>();

        PositionDeltaY = collider.bounds.extents.y;
    }

    public Unit SetUp(EntityData data, Team team, ResourceBank resourceBank, GameObject selectionIndicator, EntityProfilePanel profilePanel, IPlacementEvent placementEvent)
    {
        this.data = data;
        this.team = team;
        this.selectionIndicator = selectionIndicator;
        this.placementEvent = placementEvent;
        this.profilePanel = profilePanel;
        this.teamResourceBank = resourceBank;

        blackBoard = new BlackBoard(data, coroutineExecutor, team);
        stateMachine = new UnitStateMachine(this, blackBoard);

        healthSystem = new HealthSystem(data.MaxHealth);
        this.resourceBank = new ResourceBank();

        return this;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public override void OnSelected()
    {
        profilePanel.RegisterEntity(this);

        selectionIndicator.SetActive(false);
        selectionIndicator.SetActive(true);
    }

    public override void OnDeselected()
    {
        profilePanel.UnregisterEntity();

        selectionIndicator.SetActive(false);
    }

    public void SetTarget(Target target)
    {
        blackBoard.target = target;
        stateMachine.ChangeState<UnitMoveState>();
    }

    public override void ExecuteCommand(CommandData command)
    {
        base.ExecuteCommand(command);

        // When Place() is called:
        if(command is BuildCommandData)
            placementEvent.OnPlacementRequested += StartConstruction;
    }

    // Construction  
    private void StartConstruction(ITarget building)
    {
        placementEvent.OnPlacementRequested -= StartConstruction; // one shot handler 

        blackBoard.target = new Target(building);

        stateMachine.ChangeState<UnitMoveState>();
    }

    // Resource Management
    public void CarryResource(ResourceType type, int amount)
    {
        resourceBank.AddResource(type, amount);
    }

    public int DepositResource(ResourceType type)
    {
        var depositAmount = resourceBank.GetResourceAmount(type);
        resourceBank.SpendResource(type, depositAmount);

        return depositAmount;
    }

    public bool IsCarryingResources()
    {
        return resourceBank.GetResourceAmount(ResourceType.Gold) > 0 
            || resourceBank.GetResourceAmount(ResourceType.Wood) > 0;
    }


    #region Transform
    // Transform
    public Vector3 GetPosition() => transform.position;
    // TODO: Make rotation smoothe
    public void LookAt(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
    public int CaculateContactDistance(ITarget target)
    {
        if (target == null) return -1;
        return target.GetData().RadiusOnTerrain + blackBoard.BaseData.RadiusOnTerrain;
    }
    public int CaculateContactDistance(Target target)
    {
        return CaculateContactDistance(target.Entity);
    }
    #endregion

    #region NavMeshAgent
    // NavMesh Agent
    public void SetDestination(Vector3 destination)
    {
        agent.isStopped = false;
        agent?.SetDestination(destination);
    }
    public void ClearDestination() { agent.isStopped = true; }
    public float GetRemainingDistance() => agent.remainingDistance;
    public bool HasArrived(float tolerance = 0.1f) => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + tolerance;

    #endregion

    #region Animator
    // Animator
    public void PlayAnimation(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    }
    public void CrossFadeAnimation(string stateName, float normalizedTransitionDuration, int layer)
    {
        animator.CrossFade(stateName, normalizedTransitionDuration, layer);
    }

    public bool IsAnimationInProgress(string stateName, int layer = 0)
    {
        var info = animator.GetCurrentAnimatorStateInfo(layer);
        return info.IsName(stateName) && info.normalizedTime < 1f;
    }
    #endregion

    #region HealthSystem
    // Health System
    public IHealthSystem GetHealthSystem() => healthSystem;
    public void GetDamaged(int damage)
    {
        healthSystem.GetDamaged(damage);
    }
    #endregion

    public new UnitData GetData()
    {
        return data as UnitData;
    }
}