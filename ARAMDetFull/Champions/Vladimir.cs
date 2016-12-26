﻿using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Vladimir : Champion
    {

        public Vladimir()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Hextech_Protobelt_01),
                            new ConditionalItem(ItemId.Ionian_Boots_of_Lucidity),
                            new ConditionalItem(ItemId.Spirit_Visage),
                            new ConditionalItem(ItemId.Rabadons_Deathcap),
                            new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                            new ConditionalItem(ItemId.Liandrys_Torment),
                        },
                startingItems = new List<ItemId>
                        {
                            ItemId.Hextech_Revolver
                        }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            //if (!Sector.inTowerRange(target.Position.To2D()) && (MapControl.balanceAroundPoint(target.Position.To2D(), 700) >= -1 || (MapControl.fightIsOn() != null && MapControl.fightIsOn().NetworkId == target.NetworkId)))
            Q.CastOnUnit(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            if (player.CountEnemiesInRange(400) > 1 || (player.HealthPercent < 25 && player.CountEnemiesInRange(700) > 0))
                W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.Cast();
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (target.IsValidTarget(R.Range))
            {
                R.CastIfWillHit(target, 2);
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 610); //1000?
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, 1200, 150); //300?
        }

        public override void farm()
        {
            var mins = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.IsInRange(player, W.Range - 50));
            if (Q.IsReady() && mins.Count() > 0)
            {
                Q.Cast(mins.FirstOrDefault());
            }
            if (E.IsReady() && player.HealthPercent > 30 && mins.Count() > 0)
            {
                E.Cast();
            }
        }
    }
}