using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    private Image healthTracker; // health bar BG image
    [SerializeField] private Image healthBar;
    [SerializeField] private HealthBarSettings settings;

    private CameraController cameraController;

    private ITarget target;

    public void SetUp(CameraController cameraController, ITarget target)
    {
        this.cameraController = cameraController;
        this.target = target;
        this.target.GetHealthSystem().OnHelathChanged += UpdateHealthBarLength;

        healthBar.color = DetermineColor(target.GetTeam());
    }

    private void Awake()
    {
        healthTracker = GetComponent<Image>();
        healthTracker.rectTransform.sizeDelta = settings.Size;
    }

    private void LateUpdate()
    {
        UpdateHealthBarPosition();
    }

    private void UpdateHealthBarPosition()
    {
        Vector3 targetWorldPosition = target.GetPosition();
        Vector3 screenPosition = cameraController.Camera.WorldToScreenPoint(targetWorldPosition);
        float offsetScale = cameraController.ZoomLevel / cameraController.Camera.orthographicSize;

        healthTracker.rectTransform.position = screenPosition + settings.Offset * offsetScale;
    }

    private void UpdateHealthBarLength()
    {
        healthBar.fillAmount = (float)target.GetHealthSystem().CurrentHealth / target.GetHealthSystem().MaxHealth;
    }   

    private Color DetermineColor(Team team)
    {
        switch (team)
        {
            case Team.Green: return settings.Color_GreenTeam;
            case Team.Red: return settings.Color_RedTeam;
            case Team.Blue: return settings.Color_BlueTeam;
            default: return Color.black;
        }
    }
}