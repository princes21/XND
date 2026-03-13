using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using GameServerLib.GameObjects.AttackableUnits;


namespace Buffs
{
    internal class FerociousHowl : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        ObjAIBase owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell, ObjAIBase owner)
        {
            owner = owner;
            StatsModifier.AttackDamage.FlatBonus = 125f;
            StatsModifier.CooldownReduction.FlatBonus += 0.2f;
            StatsModifier.AbilityPower.FlatBonus += 90f;
            unit.AddStatModifier(StatsModifier);
            ApiEventManager.OnPreTakeDamage.AddListener(this, owner, OnPreTakeDamage, false);
        }

        private void OnPreTakeDamage(DamageData data)
        {
            float Damage = data.Damage;
            float reducedDamage = Damage * 0.20f; // 80% damage reduction

            if (data.DamageSource == DamageSource.DAMAGE_SOURCE_ATTACK)
            {
                data.PostMitigationDamage = reducedDamage;
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {

        }
    }
}