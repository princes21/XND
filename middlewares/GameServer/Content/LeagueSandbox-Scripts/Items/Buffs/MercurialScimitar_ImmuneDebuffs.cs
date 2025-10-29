using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    class MercurialScimitar_ImmuneDebuffs : IBuffGameScript
    {
        ObjAIBase owner;
        Particle p;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner) 
        {
            p = AddParticleTarget(owner, owner, "Item_Mercurial.troy", owner);
        }

        public void OnUpdate(float diff)
        {
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
            if(owner.HasBuff("BloodThirster_MeleeBleed")) { RemoveBuff(owner,"BloodThirster_MeleeBleed"); }
            if(owner.HasBuff("BloodThirster_RangedBleed")) { RemoveBuff(owner,"BloodThirster_RangedBleed"); }
        }
        public void OnDeactivate(ObjAIBase owner)
        {
            RemoveParticle(p);
        }
    }
}