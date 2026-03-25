using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponLight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _lightDuration;
    private Weapon _weapon;

    private Coroutine _coroutine;

    private void ShowLight()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _light.enabled = true;
        _coroutine = StartCoroutine(LightDelay());
    }
    private void OnEnable()
    {
        _weapon.OnWeaponShoot += ShowLight;
    }
    private void OnDisable()
    {
        _weapon.OnWeaponShoot -= ShowLight;
    }
    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    private IEnumerator LightDelay()
    {
        yield return new WaitForSeconds(_lightDuration);
        _light.enabled = false;
        _coroutine = null;
    }
}
