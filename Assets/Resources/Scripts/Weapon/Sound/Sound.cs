using UnityEngine;

[System.Serializable]
public struct Sound
{
    [Header("Sound")]
    [SerializeField] public AudioClip _clip;

    [SerializeField, Range(0f, 1f)] public float _volume;
    [SerializeField, Range(0f, 1f)] public float _spatialBlend;

    [Header("Pitch")]
    [SerializeField] public bool _isPitchRandomized;

    [Space]
    [SerializeField, Range(-3f, 3f)] public float _pitchMin;
    [SerializeField, Range(-3f, 3f)] public float _pitchMax;
}