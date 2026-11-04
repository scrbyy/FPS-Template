using UnityEngine;

[System.Serializable]
public struct Sound
{
    public AudioClip Clip => _clip;
    public float Volume => _volume;
    public bool IsPitchRandomized => _isPitchRandomized;
    public float PitchMin => _pitchMin;
    public float PitchMax => _pitchMax;

    [Header("Sound")]
    [SerializeField] private AudioClip _clip;
    [SerializeField, Range(0f, 1f)] private float _volume;

    [Header("Pitch")]
    [SerializeField] private bool _isPitchRandomized;
    [Space]
    [SerializeField, Range(-3f, 3f)] private float _pitchMin;
    [SerializeField, Range(-3f, 3f)] private float _pitchMax;
}