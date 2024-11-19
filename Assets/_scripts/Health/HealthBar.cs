using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider _slider;
    [SerializeField] HealthComponent _healthComponent;
    void Awake()
    {
        TryGetComponent<Slider>(out _slider);
        _slider.maxValue = _healthComponent.MaxHP;
        _healthComponent.OnHealthUpdated += (int v) => _slider.value = _healthComponent.HP;
    }

    private void Update()
    {
        transform.parent.rotation = Quaternion.identity;
    }
}
