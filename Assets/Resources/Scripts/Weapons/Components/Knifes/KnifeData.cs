using scrbyy.FpsTemplate.Characters.Stats;
using UnityEngine;

namespace scrbyy.FpsTemplate.Weapons.Knifes
{
    [CreateAssetMenu(fileName = "KnifeData", menuName = "FPS Template/Data Assets/Weapons/Knife Data Asset")]
    public class KnifeData : WeaponData, IStaminaReducer
    {
        public float StaminaCost => _staminaCost;

        [Header("Stamina")]
        [SerializeField] private float _staminaCost;
    }
}