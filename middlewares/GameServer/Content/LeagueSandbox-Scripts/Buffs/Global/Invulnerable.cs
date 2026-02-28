using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Threading;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;



namespace Buffs
{
    internal class Invulnerable : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INVULNERABILITY,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1,
            IsHidden = false
        };
        AttackableUnit Unit;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            ApiEventManager.OnPreTakeDamage.AddListener(this, unit, OnPreTakeDamage, false);
        }

          private void OnPreTakeDamage(DamageData data)
           {
            if (data.DamageSource == DamageSource.DAMAGE_SOURCE_ATTACK || data.DamageSource == DamageSource.DAMAGE_SOURCE_SPELL)
            {
                data.DamageResultType = DamageResultType.RESULT_INVULNERABLE;
                data.PostMitigationDamage = 0f;
            }
        }


        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
        }
    }
}

