using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class VayneInquisition : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle activate;
        ObjAIBase owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
          //  if (unit is ObjAIBase ai)
           // {
            //    var owner = ownerSpell.CastInfo.Owner as Champion;
             //   if (ai.HasBuff("VayneInquisition"))
              //  {
               //     OverrideAnimation(ai, "ult_idle1", "run");
                //}
                //else
                //{
                 //   OverrideAnimation(ai, "run", "ult_idle1");
                //} doesnt work properly anyway, something with the animation files i guess

                StatsModifier.AttackDamage.FlatBonus = 165;
                StatsModifier.MoveSpeed.PercentBonus = 0.15f;
                StatsModifier.Range.FlatBonus = 250f;
                StatsModifier.CriticalDamage.PercentBonus = 0.25f;
                unit.AddStatModifier(StatsModifier);
            }
        

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(activate);
        }
    }
}