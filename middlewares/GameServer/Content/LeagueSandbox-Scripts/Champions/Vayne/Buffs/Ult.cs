using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game;
using LeagueSandbox.GameServer;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Logging;
using LeagueSandbox.GameServer.Scripting.CSharp;
using log4net;
using PacketDefinitions420;
using System.Collections.Generic;
using System.Xml.Linq;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using Channel = GameServerCore.Packets.Enums.Channel;

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
        // Create a dictionary mapping original animations to new ones
        // Declare it as a readonly field

        private readonly Dictionary<string, string> UltAnimOverrides = new Dictionary<string, string>
        {
            { "idle1", "Idle_Ult" },
            { "run", "Run_Ult" },
            { "Attack1", "Attack_Ult" },
            { "Attack2", "Attack_Ult" },
            { "Crit", "Attack_Ult" },
        };

        private readonly Dictionary<string, string> ResetAnimOverrides = new Dictionary<string, string>
        {
            { "idle1", "idle1" },
            { "run", "run" },
            { "Attack1", "Attack1" },
            { "Attack2", "Attack2" },
            { "Crit", "Crit" },
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle activate;
        ObjAIBase owner;
        Game _game;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            unit.SetAnimStates(UltAnimOverrides);
            StatsModifier.AttackDamage.FlatBonus = 165;
            StatsModifier.MoveSpeed.PercentBonus = 0.15f;
            StatsModifier.Range.FlatBonus = 250f;
            StatsModifier.CriticalDamage.PercentBonus = 0.25f;
            unit.AddStatModifier(StatsModifier);
        //  ownerSpell.CastInfo.Owner.SetAutoAttackSpell("VayneUltAttack", false); todo: figure why it's buggy
       //    ownerSpell.SetAutocast();
            }


        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(activate);
            unit.SetAnimStates(ResetAnimOverrides);
        }
    }
}