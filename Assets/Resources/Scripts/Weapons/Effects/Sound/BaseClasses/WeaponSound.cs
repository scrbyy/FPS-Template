using UnityEngine;
using UnityEngine.Audio;

namespace scrbyy.FpsTemplate.Weapons.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class WeaponSound : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected AudioMixerGroup _mixerGroup;

        [Header("Sound Library")]
        [SerializeField] protected Sound.Sound _attackSound;
        [SerializeField] protected Sound.Sound _openSound;

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

        protected void PlaySound(Sound.Sound sound)
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
}