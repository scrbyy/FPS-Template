namespace scrbyy.FpsTemplate.Weapons.Initializers
{
    public interface IFeatureBinder
    {
        void Subscribe(Weapon weapon);
        void Unsubscribe(Weapon weapon);
    }
}