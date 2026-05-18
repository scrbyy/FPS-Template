using UnityEngine;
using System.Collections;

public class GunShotLight : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private float _lightDuration;
    [SerializeField] private Gun _gun;

    private Coroutine _coroutine;
    
    private void ShowLight()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _light.enabled = true;
        _coroutine = StartCoroutine(HideDelay());
    }

    private IEnumerator HideDelay()
    {
        yield return new WaitForSeconds(_lightDuration);
        _light.enabled = false;
        _coroutine = null;
    }

    private void OnEnable()
    {
        _gun.OnAttack += ShowLight;
    }

    private void OnDisable()
    {
        _gun.OnAttack -= ShowLight;
    }
}
