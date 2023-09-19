using Leopotam.Ecs;
using UnityEngine;

struct Movement : IEcsAutoReset<Movement>
{
    public float Speed;
    public EcsEntity entity;

    public void AutoReset(ref Movement c)
    {
        c.Speed = 10;
    }
}
