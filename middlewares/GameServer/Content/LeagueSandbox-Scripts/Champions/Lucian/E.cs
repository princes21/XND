using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class LucianE : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);

            FaceDirection(spellPos, owner, true);
            AddBuff("AbilityUsed", 4.0f, 1, spell, owner, owner);
            AddBuff("LucianEBuff", 1.5f, 1, spell, owner, owner);
            var trueCoords = GetPointFromUnit(owner, spell.SpellData.CastRangeDisplayOverride);

            ForceMovement(owner, "Spell3", trueCoords, 1350, 0, 0, 0, movementOrdersFacing: ForceMovementOrdersFacing.KEEP_CURRENT_FACING);
        }
    }
}
