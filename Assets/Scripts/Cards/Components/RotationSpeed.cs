using Leopotam.Ecs;
using UnityEngine;

struct RotationSpeed : IEcsAutoReset<RotationSpeed>
{
    public float Speed;
    public bool finished;

    public void AutoReset(ref RotationSpeed c)
    {
        c.Speed = 60;
    }
}
