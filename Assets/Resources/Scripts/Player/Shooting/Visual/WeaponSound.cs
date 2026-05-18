using UnityEngine;

[RequireComponent(typeof(WeaponT))]
public class WeaponSound : MonoBehaviour
{
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private Transform _soundPoint;
    private WeaponT _weapon;

    private void PlayShootSound()
    {
        AudioSource.PlayClipAtPoint(_shootSound, _soundPoint.position);
    }
    private void OnEnable()
    {
        _weapon.OnWeaponShoot += PlayShootSound;
    }
    private void OnDisable()
    {
        _weapon.OnWeaponShoot -= PlayShootSound;
    }
    private void Awake()
    {
        _weapon = GetComponent<WeaponT>();
    }
}
