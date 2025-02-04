using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    private Image healthTracker;
    [SerializeField] private Image healthBar;
    [SerializeField] private HealthBarSettings settings;

    private new Camera camera;

    private ITarget target;

    public void SetUp(Camera camera, Target target)
    {
        this.camera = camera;
        this.target = target.Entity;
        this.target.GetHealthSystem().OnHelathChanged += UpdateHealthBarLength;
    }

    private void Awake()
    {
        healthTracker = GetComponent<Image>();
        healthTracker.rectTransform.sizeDelta = settings.Size;
    }

    private void Update()
    {
        UpdateHealthBarPosition();
    }

    private void UpdateHealthBarPosition()
    {
        Vector3 targetWorldPosition = target.GetPosition();

        Vector3 screenPosition = camera.WorldToScreenPoint(targetWorldPosition);

        healthTracker.rectTransform.position = screenPosition + settings.Offset;
    }

    private void UpdateHealthBarLength()
    {
        healthBar.fillAmount = (float)target.GetHealthSystem().CurrentHealth / target.GetHealthSystem().MaxHealth;
    }   
}