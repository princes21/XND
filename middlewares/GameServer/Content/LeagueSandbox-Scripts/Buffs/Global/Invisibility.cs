using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;


namespace Buffs
{
    internal class Invisibility : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INVISIBILITY,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        Buff thisBuff;
        private AttackableUnit Unit;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;

            if (unit.Team == TeamId.TEAM_BLUE)
            {
                BecomeInvisible(unit);
                buff.SetStatusEffect(StatusFlags.Stealthed, true);
            }
            if (unit.Team == TeamId.TEAM_PURPLE)
            {
                BecomeInvisible(unit);
                buff.SetStatusEffect(StatusFlags.Stealthed, true);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (thisBuff != null)
            {
                thisBuff.DeactivateBuff();
            }

            if (unit.Team == TeamId.TEAM_BLUE)
            {
                BecomeVisible(unit);
                buff.SetStatusEffect(StatusFlags.Stealthed, false);
            }
            if (unit.Team == TeamId.TEAM_PURPLE)
            {
                BecomeVisible(unit);
                buff.SetStatusEffect(StatusFlags.Stealthed, false);
            }
        }
    }
}