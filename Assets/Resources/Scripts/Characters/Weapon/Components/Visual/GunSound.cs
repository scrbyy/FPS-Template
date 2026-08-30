using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))] 
public class GunSound : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioMixerGroup _mixerGroup;
    [SerializeField, Range(0f, 1f)] private float _volume;
    [SerializeField, Range(0.1f, 3f)] private float _pitchMin;
    [SerializeField, Range(0.1f, 3f)] private float _pitchMax;

    [Header("Sound Library")]
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private AudioClip _readySound;

    [Header("References")]
    [SerializeField] private Gun _gun;
     
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

    private void PlayReloadSound()
    {
        if (_reloadSound == null || _audioSource == null) return;
        _audioSource.PlayOneShot(_reloadSound, _volume);
    }

    private void PlayReadySound()
    {
        if (_readySound == null || _audioSource == null) return;
        _audioSource.PlayOneShot(_readySound, _volume);

    }

    private void OnEnable()
    {
        _gun.OnAttack += PlayShootSound;
        _gun.OnReloadStart += PlayReloadSound;
        _gun.OnReady += PlayReadySound;
    }

    private void OnDisable()
    {
        _gun.OnAttack -= PlayShootSound;
        _gun.OnReloadStart -= PlayReloadSound;
        _gun.OnReady -= PlayReadySound;
    }
}