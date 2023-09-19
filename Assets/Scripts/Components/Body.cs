using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

struct Body : IEcsAutoReset<Body>
{
    public RectTransform ObjRt;
    public float FinalYAngle;
    public Image image;
    public Sprite face,
        shirt;

    public void AutoReset(ref Body c)
    {
        c.FinalYAngle = 180;
    }
}
