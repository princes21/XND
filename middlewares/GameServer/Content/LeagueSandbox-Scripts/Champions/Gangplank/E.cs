using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore;
using System.Linq;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class RaiseMorale : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var hasbuff = owner.HasBuff("GangplankE");
            var units = GetUnitsInRange(owner.Position, 1000, true).Where(x => x.Team != CustomConvert.GetEnemyTeam(owner.Team));
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);

            if (hasbuff == false)
            {
                AddBuff("GangplankE", 7.0f, 1, spell, owner, owner);
            }

            foreach (var allyTarget in units)
            {
                if (allyTarget is AttackableUnit && owner != allyTarget && hasbuff == false)
                {
                    AddBuff("GangplankE", 7.0f, 1, spell, allyTarget, owner);
                }
            }
        }
    }
}
