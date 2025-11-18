using System;
using System.Numerics;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Enums;

namespace Spells
{
    public class Deceive : ISpellScript
    {
        Vector2 teleportTo;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell) { }

        public void OnDeactivate(ObjAIBase owner, Spell spell) { }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            // Calculate teleport position
            teleportTo = new Vector2(end.X, end.Y);
            float targetPosDistance = (float)Math.Sqrt(Math.Pow(owner.Position.X - teleportTo.X, 2f) + Math.Pow(owner.Position.Y - teleportTo.Y, 2f));
            FaceDirection(teleportTo, owner);
            teleportTo = GetPointFromUnit(owner, Math.Min(targetPosDistance, 400f));
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            // Teleport Shaco
            TeleportTo(owner, teleportTo.X, teleportTo.Y);

            // Buff duration based on Q level
            float duration = 1.5f + 0.5f * (spell.CastInfo.SpellLevel - 1); // Example: lvl1 = 1.5s, lvl2 = 2s, etc.

            AddBuff("Deceive", duration, 1, spell, owner, owner);
            // Particle effect
            AddParticle(owner, null, "JackintheboxPoof2.troy", owner.Position, 2f);

            AddBuff("AbilityUsed", 4.0f, 1, spell, owner, owner);
        }

        public void OnSpellChannel(Spell spell) { }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason) { }

        public void OnSpellPostChannel(Spell spell) { }

        public void OnUpdate(float diff) { }
    }
}
