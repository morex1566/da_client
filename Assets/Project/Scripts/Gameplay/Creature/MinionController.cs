using UnityEngine;

[DisallowMultipleComponent]
public class MinionController : CreatureController
{
    public new MinionData Data => base.Data as MinionData;
}
