using UnityEngine;

public class WeaponSound : MonoBehaviour
{
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private Transform _soundPoint;
    [SerializeField] private Weapon _weapon;

    private void PlayShootSound()
    {
        AudioSource.PlayClipAtPoint(_shootSound, _soundPoint.position);
    }
    private void OnEnable()
    {
        _weapon.OnAttack += PlayShootSound;
    }
    private void OnDisable()
    {
        _weapon.OnAttack -= PlayShootSound;
    }
}
