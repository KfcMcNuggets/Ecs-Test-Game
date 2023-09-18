using Leopotam.Ecs;

struct MovementSpeed : IEcsAutoReset<MovementSpeed>
{
    public float Speed;

    public void AutoReset(ref MovementSpeed c)
    {
        c.Speed = 500;
    }
}
