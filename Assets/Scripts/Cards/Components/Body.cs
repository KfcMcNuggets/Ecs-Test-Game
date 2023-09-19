using Leopotam.Ecs;
using UnityEngine;

struct Body : IEcsAutoReset<Body>
{
    public Transform objTransform;
    public float FinalYAngle;
    public EcsEntity entity;

    public void AutoReset(ref Body c)
    {
        c.FinalYAngle = 180;
    }
}
