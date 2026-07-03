using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))] 
public class WeaponSound : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Weapon _weapon;
    [SerializeField] private AudioClip _shootSound;

    [Header("Settings")]
    [SerializeField] private AudioMixerGroup _mixerGroup; 
    [SerializeField, Range(0f, 1f)] private float _volume;
    [SerializeField, Range(0.1f, 3f)] private float _pitchMin;
    [SerializeField, Range(0.1f, 3f)] private float _pitchMax;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1.0f;
        if (_mixerGroup != null) _audioSource.outputAudioMixerGroup = _mixerGroup;
    }

    private void PlayShootSound()
    {
        if (_shootSound == null || _audioSource == null) return;

        _audioSource.pitch = Random.Range(_pitchMin, _pitchMax);

        _audioSource.PlayOneShot(_shootSound, _volume);
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