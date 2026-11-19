namespace scrbyy.FpsTemplate.Weapons.Initialization
{
    public interface IFeatureBinder
    {
        void Subscribe(Weapon weapon);
        void Unsubscribe(Weapon weapon);
    }
}