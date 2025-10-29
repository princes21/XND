using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace ItemSpells
{
    public class ItemMercurial : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            //Cleanse the Buffs
            if(owner.HasBuff("Stun")) { RemoveBuff(owner,"Stun"); }
            if(owner.HasBuff("Blind")) { RemoveBuff(owner,"Blind"); }
            if(owner.HasBuff("Disarm")) { RemoveBuff(owner,"Disarm"); }
            if(owner.HasBuff("Silence")) { RemoveBuff(owner,"Silence"); }
            if(owner.HasBuff("Slow")) { RemoveBuff(owner,"Slow"); }
            if(owner.HasBuff("DebuffSlow2Seconds")) { RemoveBuff(owner,"DebuffSlow2Seconds"); }
            if(owner.HasBuff("Frozen_Mallet_Slow")) { RemoveBuff(owner,"Frozen_Mallet_Slow"); }
            if(owner.HasBuff("FrozenHeartASDebuff")) { RemoveBuff(owner,"FrozenHeartASDebuff"); }
            if(owner.HasBuff("LiandrysTormentBurn")) { RemoveBuff(owner,"LiandrysTormentBurn"); }
            if(owner.HasBuff("LiandrysTormentBurnCrit")) { RemoveBuff(owner,"LiandrysTormentBurnCrit"); }
            if(owner.HasBuff("GreviousWound")) { RemoveBuff(owner,"GreviousWound"); }
            if(owner.HasBuff("RilaysCrystalScepterDebuff")) { RemoveBuff(owner,"RilaysCrystalScepterDebuff"); }
            if(owner.HasBuff("TheBlackCleaverDebuff")) { RemoveBuff(owner,"TheBlackCleaverDebuff"); }
            if(owner.HasBuff("DariusHemo")) { RemoveBuff(owner,"DariusHemo"); }
            if(owner.HasBuff("ArmorReduction")) { RemoveBuff(owner,"ArmorReduction"); }
            if(owner.HasBuff("BloodThirster_MeleeBleed")) { RemoveBuff(owner,"BloodThirster_MeleeBleed"); }
            if(owner.HasBuff("BloodThirster_RangedBleed")) { RemoveBuff(owner,"BloodThirster_RangedBleed"); }

            //Add Immune to Debuffs & Grants Speed Buff
            AddBuff("MercurialScimitar_ImmuneDebuffs", 3.5f, 1, spell, owner, owner);
            AddBuff("MercurialScimitar_SpeedBuff", 1.5f, 1, spell, owner, owner);
        }
    }
}
