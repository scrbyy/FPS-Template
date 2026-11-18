using scrbyy.FpsTemplate.Characters.Stats;

namespace scrbyy.FpsTemplate.Characters.Enemy
{
    public class EnemyHealth : CharacterHealth
    {
        protected override void HandleEmptyValue()
        {
            Destroy(gameObject);
        }
    }
}