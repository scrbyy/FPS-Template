public class EnemyHealth : ObjectHealth
{
    protected override void HandleEmptyValue()
    {
        Destroy(gameObject);
    }
}
