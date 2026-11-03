using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class WeaponSound : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioMixerGroup _mixerGroup;

    [Header("Sound Library")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _openSound;

    [Header("References")]
    [SerializeField] private Weapon _weapon;

    private AudioSource _audioSource;

    private System.Action _playAttackSound;
    private System.Action _playOpenSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1.0f;
        if (_mixerGroup != null) _audioSource.outputAudioMixerGroup = _mixerGroup;

        _playAttackSound = () => PlaySound(_attackSound, false, 1.0f);
        _playOpenSound = () => PlaySound(_openSound, false, 1.0f);
    }

    private void PlaySound(AudioClip sound, bool randomizePitch, float volume)
    {
        if (sound == null || _audioSource == null) return;

        if (randomizePitch) _audioSource.pitch = Random.Range(0.8f, 1.2f);
        else _audioSource.pitch = 1.0f;

        _audioSource.PlayOneShot(sound, volume);
    }

    private void OnEnable()
    {
        _weapon.OnAttack += _playAttackSound;
        _weapon.OnOpened += _playOpenSound;
    }
    private void OnDisable()
    {
        _weapon.OnAttack -= _playAttackSound;
        _weapon.OnOpened -= _playOpenSound;
    }
}