using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityCreateButton : MonoBehaviour
{
    [SerializeField] private EntityData entityToCreate;
    public event System.Action<EntityData> OnButtonClicked;

    private void RaiseEvent() { OnButtonClicked?.Invoke(entityToCreate); }

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(RaiseEvent);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
