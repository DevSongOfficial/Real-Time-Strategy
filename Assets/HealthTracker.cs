using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    private Image healthTracker;

    private new Camera camera;

    private ITarget target;
    private IHealthSystem targetHealth;

    public void SetUp(Camera camera, Target target)
    {
        this.camera = camera;

        this.target = target.Entity;
        targetHealth = target.Entity.GetHealthSystem();

        targetHealth.OnHelathChanged += UpdateHealthBarLength;
    }

    private void Awake()
    {
        healthTracker = GetComponent<Image>();
    }

    private void Update()
    {
        UpdateHealthBarPosition();
    }

    private void UpdateHealthBarPosition()
    {
        Vector3 targetWorldPosition = target.GetPosition();

        Vector3 screenPosition = camera.WorldToScreenPoint(targetWorldPosition);

        healthTracker.rectTransform.position = screenPosition;
    }

    private void UpdateHealthBarLength()
    {
        healthBar.fillAmount = (float)targetHealth.CurrentHealth / targetHealth.MaxHealth;
    }

    
}
