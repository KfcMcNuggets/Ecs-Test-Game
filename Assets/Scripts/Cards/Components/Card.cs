using Leopotam.Ecs;

struct Card : IEcsAutoReset<Card>
{
    public int CardId;

    public int MaxRemixes;

    public int CurrentMixes;
    public EcsEntity entity;

    public void AutoReset(ref Card c)
    {
        c.MaxRemixes = 1;
    }
}
