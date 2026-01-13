using Unity.AppUI.Core;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;


[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Playable, IDamageable, ITargetor, ITarget, IUnitStateContext
{
    // State Machine
    private UnitStateMachine stateMachine;
    private BlackBoard blackBoard;
    private Command command;

    [SerializeField] private CoroutineExecutor coroutineExecutor;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private new Collider collider; // collider for calculating position delta.
    public float PositionDeltaY { get; private set; }


    private GameObject selectionIndicator;
    private IPlacementEvent placementEvent;
    private HealthSystem healthSystem;

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

    public Unit SetUp(EntityData data, Team team, GameObject selectionIndicator, IPlacementEvent placementEvent)
    {
        this.data = data;
        this.team = team;
        this.selectionIndicator = selectionIndicator;
        this.placementEvent = placementEvent;

        blackBoard = new BlackBoard(data, coroutineExecutor, team);
        stateMachine = new UnitStateMachine(this, blackBoard);

        healthSystem = new HealthSystem(data.MaxHealth);

        return this;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public override void OnSelected()
    {
        selectionIndicator.SetActive(false);
        selectionIndicator.SetActive(true);

        Debug.Log(GetTeam());
    }

    public override void OnDeselected()
    {
        selectionIndicator.SetActive(false);
    }

    public void SetTarget(Target target)
    {
        blackBoard.target = target;
        stateMachine.ChangeState<UnitMoveState>();
    }

    public override void ExecuteCommand(Command command)
    {
        base.ExecuteCommand(command);

        this.command = command;

        // When Place() is called:
        placementEvent.OnPlacementRequested += StartConstruction;
    }

    private void StartConstruction(ITarget building)
    {
        placementEvent.OnPlacementRequested -= StartConstruction; // one shot handler 

        blackBoard.constructionTime = command.generationTime;
        blackBoard.target = new Target(building);

        stateMachine.ChangeState<UnitMoveState>();
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

    public EntityData GetData()
    {
        return data;
    }
}