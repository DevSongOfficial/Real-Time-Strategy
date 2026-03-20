using System;
using System.Collections;
using Unity.AppUI.Core;
using Unity.VisualScripting;
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


    public event Action<Unit> OnDestroyed;
    public event Action<Unit> OnDeathRequested;

    private GridSystem gridSystem;
    private GameObject selectionIndicator;
    private HealthSystem healthSystem;
    protected IPlacementEvent placementEvent;
    protected EntityProfilePanel profilePanel;

    // Resource
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

    public Unit SetUp(EntityData data, TeamContext teamContext, GameObject selectionIndicator, EntityProfilePanel profilePanel, IPlacementEvent placementEvent, GridSystem gridSystem)
    {
        this.data = data;
        this.selectionIndicator = selectionIndicator;
        this.placementEvent = placementEvent;
        this.profilePanel = profilePanel;
        this.teamContext = teamContext;
        this.gridSystem = gridSystem;

        blackBoard = new BlackBoard(data, coroutineExecutor, teamContext);
        stateMachine = new UnitStateMachine(this, blackBoard);

        healthSystem = new HealthSystem(data.MaxHealth);
        healthSystem.OnDie += RequestDeath;

        resourceBank = new ResourceBank();

        return this;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    #region Selection
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

    public override bool CanSelect()
    {
        return IsAlive();
    }
    #endregion



    public void SetTarget(Target target)
    {
        blackBoard.SetTarget(target);
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
        if  (target is Building) return blackBoard.BaseData.RadiusOnTerrain;
        else if (target is Unit) return target.GetData().RadiusOnTerrain + blackBoard.BaseData.RadiusOnTerrain;

        return -1;
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
        agent.SetDestination(destination);
    }

    private float offset = 0.1f;
    public void SetDestination(Target target)
    {
        if (target.Entity is not Building building)
            return;

        Vector3 destination = target.GetPosition();
        Vector2Int cellPosition = building.GetCellPosition();
        Vector2Int cellSize = building.GetData().CellSize;

        Vector2Int origin = gridSystem.MouseToOrigin(cellPosition, cellSize);

        Vector3 min = gridSystem.CellToWorld(new Vector3Int(origin.x, 0, origin.y));
        Vector3 max = gridSystem.CellToWorld(new Vector3Int(origin.x + cellSize.x, 0, origin.y + cellSize.y));

        // Find the closest cell' position.
        float x = Mathf.Clamp(transform.position.x, min.x, max.x);
        float z = Mathf.Clamp(transform.position.z, min.z, max.z);
        Vector3 closest = new Vector3(x, transform.position.y, z);

        Vector3 direction = (closest - transform.position).normalized;

        // Set destination
        destination = closest;
        if (direction.sqrMagnitude > 0.0001f)    // if direction is valid:
            destination -= direction * offset;  // destination이 현재는 건물이 차지하는 cell 중 가장 가까운 cell이므로,
                                                // so move it back toward the unit by the offset to place it just outside the bounds.

        //if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        //    destination = hit.position;

        SetDestination(destination);
    }
    

    public void ClearDestination() 
    { 
        agent.isStopped = true;
        agent.ResetPath();
    }
    public float GetRemainingDistance()
    {
        // return agent.remainingDistance;

        return Vector3.Distance(agent.destination, transform.position);
    }
    public bool HasArrived(float tolerance = 0.1f)
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + tolerance;
    }

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
    public bool IsAlive()
    {
        return healthSystem.CurrentHealth > 0;
    }

    private void RequestDeath()
    {
        OnDeathRequested?.Invoke(this);
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        stateMachine.ChangeState<UnitDieState>();
        
        if(profilePanel.CurrentEntity == this as ISelectable)
            profilePanel.UnregisterEntity();

        yield return new WaitForSeconds(3);

        OnDestroyed?.Invoke(this);
        GameObject.Destroy(gameObject);

    }
    #endregion

    public new UnitData GetData()
    {
        return data as UnitData;
    }
}