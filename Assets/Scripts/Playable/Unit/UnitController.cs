using System.Collections.Generic;
using UnityEngine;
using CustomResourceManagement;

public sealed class UnitController : MonoBehaviour
{
    // Temporary for testing.
    [SerializeField, Range(0, 99)] private int numberOfUnitOnStart = 3;
    private void GenerateUnits(int numberOfUnit)
    {
        for (int i = 0; i < numberOfUnit; i++)
        {
            var prefab = ResourceLoader.GetResource<Unit>(Prefabs.Playable.Unit.Unit_1);
            var randomPosition = new Vector3(Random.Range(26, 35), 2, Random.Range(20, 36));
            allUnits.Add(Instantiate(prefab, randomPosition, Quaternion.identity));
        }
    }

    [SerializeField] private new Camera camera;
    [SerializeField] private Canvas canvas;

    private List<ISelectable> allUnits;

    // Mouse drag event.
    private DragEventHandler dragEventHandler;

    // Playbles selection.
    private SelectionHandler selectionHandler;
    private List<ISelectable> selectedUnits;
    [SerializeField] private LayerMask layerMask_Selectable;

    // Attack of selected playables.
    private AttackHandler attackHandler;

    private void Awake()
    {
        selectedUnits = new List<ISelectable>();
        allUnits = new List<ISelectable>();

        
        dragEventHandler    = new DragEventHandler(allUnits.FilterByType<ISelectable, ITransformProvider>(), camera, canvas);
        selectionHandler    = new SelectionHandler(selectedUnits, camera, layerMask_Selectable);

        attackHandler       = new AttackHandler(selectedUnits.FilterByType <ISelectable, IAttackable>());

        dragEventHandler.OnUnitDetectedInDragArea += selectionHandler.SelectUnits;
    }

    private void Start()
    {
        GenerateUnits(numberOfUnitOnStart);
    }

    private void Update()
    {
        dragEventHandler.HandleDragEvent();
        selectionHandler.HandleUnitSelection();
        selectionHandler.HandleTargetSelection();
        attackHandler.HandleAttack();
    }
}