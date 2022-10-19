using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RoofBomb;

public class Building_RoofBomb : Building
{
    public override IEnumerable<Gizmo> GetGizmos()
    {
        yield return new Command_Action
        {
            defaultLabel = "CommandDetonateLabel".Translate(),
            icon = ContentFinder<Texture2D>.Get("UI/Commands/Detonate"),
            defaultDesc = "CommandDetonateDesc".Translate(),
            action = Command_Detonate
        };
    }

    private void Command_Detonate()
    {
        GetComp<CompExplosive>().StartWick();
    }

    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        var map = Map;
        base.Destroy(mode);
        FleckMaker.ThrowMicroSparks(DrawPos, map);
        if (mode != DestroyMode.KillFinalize)
        {
            return;
        }

        foreach (var current in this.CellsAdjacent8WayAndInside())
        {
            if (map.roofGrid.RoofAt(current) != null) // if there is a roof
            {
                map.roofGrid.SetRoof(current, null);
            }
        }
    }
}