using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public abstract class WeaponSound : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected AudioMixerGroup _mixerGroup;

    [Header("Sound Library")]
    [SerializeField] protected Sound _attackSound;
    [SerializeField] protected Sound _openSound;

    protected Weapon Weapon { get; set; }

    protected AudioSource _audioSource;

    private System.Action _playAttackSound;
    private System.Action _playOpenSound;

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.playOnAwake = false;
        
        if (_mixerGroup != null) _audioSource.outputAudioMixerGroup = _mixerGroup;

        _playAttackSound = () => PlaySound(_attackSound);
        _playOpenSound = () => PlaySound(_openSound);
    }

    protected void PlaySound(Sound sound)
    {
        if (sound.Clip == null || _audioSource == null) return;

        if (sound.IsPitchRandomized)
        {
            _audioSource.pitch = Random.Range(sound.PitchMin, sound.PitchMax);
        }

        _audioSource.PlayOneShot(sound.Clip, sound.Volume);
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