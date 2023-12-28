using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuffedDamageOverTimeDebuffs.NPCs
{
    public class DTDGlobalNPC : GlobalNPC
    {
        private readonly record struct DOTInfo(int RegenRate, int TickDamage);
        private static readonly Dictionary<int, DOTInfo> DOTDebuffInfoTable = new()
        {
            [BuffID.Poisoned] = new(-12, 1),
            [BuffID.OnFire] = new(-8, 1),
            [BuffID.OnFire3] = new(-30, 5),
            [BuffID.Frostburn] = new(-16, 2),
            [BuffID.Frostburn2] = new(-50, 10),
            [BuffID.CursedInferno] = new(-48, 10),
            [BuffID.Venom] = new(-60, 15),
            [BuffID.ShadowFlame] = new(-30, 5),
            [BuffID.SoulDrain] = new(-50, 5),
        };

        public override void UpdateLifeRegen(NPC npc, ref int damagePerTick)
        {
            for (int i = 0; i < NPC.maxBuffs; ++i)
            {
                if (npc.buffTime[i] <= 0 || !DOTDebuffInfoTable.TryGetValue(npc.buffType[i], out var dotInfo)) continue;
                
                var (regen, damage) = dotInfo;

                regen = -regen / 2;

                if (npc.lifeRegen > 0) npc.lifeRegen = 0;

                double lifeCoef = Math.Pow(npc.lifeMax / 70d, 0.4 + 0.3 * 1000 / (npc.lifeMax + 1000));
                double regenCoef = 10 * Math.Pow(0.1 * regen, 0.05) * Math.Log(regen + 1, 12);
                double newRegen = Math.Ceiling(lifeCoef * regenCoef);

                if (npc.aiStyle == NPCAIStyleID.Worm) newRegen *= 0.1;
                if (newRegen < regen) newRegen = regen;

                npc.lifeRegen -= (int)((newRegen - regen) * 2);

                int newDamagePerTick = (int)Math.Ceiling(newRegen * damage / regen - 0.5);
                if (damagePerTick < newDamagePerTick)
                    damagePerTick = newDamagePerTick;
            }
        }


    }
}
