﻿using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Kennen : Champion
    {
        public Kennen()
        {
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null || player.HasBuff("KennenLightningRush"))
                return;
            E.Cast();
            Aggresivity.addAgresiveMove(new AgresiveMove(60, 3500, true));
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (player.CountEnemiesInRange(500) > 1)
            {
                R.Cast(target);
                Aggresivity.addAgresiveMove(new AgresiveMove(90, 3500, true));
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(500);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        private float GetComboDamage(Obj_AI_Base vTarget)
        {
            var fComboDamage = 0d;
            float manaCost = 0;
            if (Q.IsReady() && Q.ManaCost <= player.Mana)
            {
                fComboDamage += player.GetSpellDamage(vTarget, SpellSlot.Q);
                manaCost += Q.ManaCost - 20;
            }

            if (E.IsReady() && E.ManaCost + manaCost <= player.Mana)
            {
                fComboDamage += player.GetSpellDamage(vTarget, SpellSlot.E);
                manaCost += E.ManaCost - 5;
            }

            if (R.IsReady())
            {
                fComboDamage += player.GetSpellDamage(vTarget, SpellSlot.R) /** R.Instance.Ammo*/;
            }
            
            return (float)fComboDamage;
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Linear, 250, 1650, 50)
            {
                AllowedCollisionCount = 1
            };

            W = new Spell.Active(SpellSlot.W, 750, DamageType.Magical);

            E = new Spell.Active(SpellSlot.E, uint.MaxValue, DamageType.Magical);

            R = new Spell.Active(SpellSlot.R, 550, DamageType.Magical);
        }
    }
}