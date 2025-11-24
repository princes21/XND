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
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using System.Collections.Generic;

namespace Buffs
{
    class Fear : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle p;
        Particle p1;
        Particle p2;
        Particle p3;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
           var owner = ownerSpell.CastInfo.Owner;

            var to = Vector2.Normalize(unit.Position - owner.Position);

            if (unit is Minion || unit is Monster)
            {
                p = AddParticleTarget(owner, unit, "Global_Fear.troy", unit, 2.3f);
                p1 = AddParticleTarget(owner, unit, "LOC_fear.troy", unit, 2.3f);
                ForceMovement(unit, "RUN", new Vector2(unit.Position.X - to.X * -225f, unit.Position.Y - to.Y * -225f), 100, 0, 0, 0);
            }
            else
            {
                p2 = AddParticleTarget(owner, unit, "Global_Fear.troy", unit, 2f);
                p3 = AddParticleTarget(owner, unit, "LOC_fear.troy", unit, 2f);
                ForceMovement(unit, "RUN", new Vector2(unit.Position.X - to.X * -350f, unit.Position.Y - to.Y * -350f), 200, 0, 0, 0);
            }

        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            RemoveParticle(p1);
            RemoveParticle(p2);
            RemoveParticle(p3);

        }

        public void OnUpdate(float diff)
        {

        }
    }
}