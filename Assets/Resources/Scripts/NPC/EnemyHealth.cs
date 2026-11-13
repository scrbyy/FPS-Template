public class EnemyHealth : CharacterHealth
{
    protected override void HandleEmptyValue()
    {
        Destroy(gameObject);
    }
}
