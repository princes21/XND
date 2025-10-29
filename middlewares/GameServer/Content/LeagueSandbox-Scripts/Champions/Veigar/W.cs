using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class VeigarDarkMatter : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
        };
        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var truecoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var distance = Vector2.Distance(spell.CastInfo.Owner.Position, truecoords);
            if (distance > 900f)
            {
                truecoords = GetPointFromUnit(spell.CastInfo.Owner, 900f);
            }
            string particles;
            if (ownerSkinID == 8)
            {
                particles = "Veigar_Skin08_W_cas.troy";

            }
            else
            {
                particles = "Veigar_Base_W_cas.troy";
            }
            AddParticle(owner, null, particles, truecoords, lifetime: 1.25f);


            AddBuff("VeigarW", 1.25f, 1, spell, owner, owner);
        }
    }
}
