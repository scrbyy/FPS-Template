using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public abstract class WeaponSound : MonoBehaviour
{
    protected Weapon Weapon { get; set; }

    [Header("Settings")]
    [SerializeField] protected AudioMixerGroup _mixerGroup;

    [Header("Sound Library")]
    [SerializeField] protected Sound _attackSound;
    [SerializeField] protected Sound _openSound;

    protected AudioSource _audioSource;

    private System.Action _playAttackSound;
    private System.Action _playOpenSound;

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.playOnAwake = false;
        
        if (_mixerGroup != null) _audioSource.outputAudioMixerGroup = _mixerGroup;

        _playAttackSound = () => PlaySound(_attackSound._clip, _attackSound._volume);
        _playOpenSound = () => PlaySound(_openSound._clip, _openSound._volume);
    }

    protected void PlaySound(AudioClip sound, float volume)
    {
        if (sound == null || _audioSource == null) return;

        _audioSource.PlayOneShot(sound, volume);
    }

    protected void PlaySound(AudioClip sound, float volume, float minPitch, float maxPitch)
    {
        if (sound == null || _audioSource == null) return;

        _audioSource.pitch = Random.Range(minPitch, maxPitch);

        _audioSource.PlayOneShot(sound, volume);
    }

    protected virtual void OnEnable()
    {
        Weapon.OnAttack += _playAttackSound;
        Weapon.OnOpening += _playOpenSound;
    }

    protected virtual void OnDisable()
    {
        Weapon.OnAttack -= _playAttackSound;
        Weapon.OnOpening -= _playOpenSound;
    }
}