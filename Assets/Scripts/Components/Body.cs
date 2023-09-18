using Leopotam.Ecs;
using UnityEngine;

struct Body : IEcsAutoReset<Body>
{
    public RectTransform ObjRt;
    public float FinalYAngle;

    public void AutoReset(ref Body c)
    {
        c.FinalYAngle = 180;
    }
}
