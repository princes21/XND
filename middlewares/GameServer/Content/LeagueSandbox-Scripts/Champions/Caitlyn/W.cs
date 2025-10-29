using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class CaitlynYordleTrap : ISpellScript
    {
        SpellSector ActivationRange;
		Vector2 spellPos;
        //public List<ISpellSector> mushroomRanges = new List<ISpellSector>();
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
			AddParticle(owner, null, "caitlyn_Base_yordleTrap_set", spellPos, 10f,1,"");			
            var Trap = AddMinion(owner, "CaitlynTrap", "CaitlynTrap", spellPos, owner.Team, owner.SkinID, true, false);
            AddBuff("CaitlynTrap", 60f, 1, spell, Trap, Trap);
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }
    }
}
