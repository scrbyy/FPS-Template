using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponSound : MonoBehaviour
{
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private Transform _soundPoint;
    private Weapon _weapon;

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
        _weapon = GetComponent<Weapon>();
    }
}
