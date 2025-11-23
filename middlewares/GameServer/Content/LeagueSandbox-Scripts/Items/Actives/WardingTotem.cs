using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace ItemSpells
{
    public class TrinketTotemLvl1 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var totem = CreatePet(
                owner: (Champion)owner,
                spell: spell,
                position: Cursor,
                name: "TrinketTotemLvl1",
                model: "VisionWard",
                buffName: "",
                lifeTime: 18f,
                cloneInventory: false,
                showMinimapIfClone: false,
                disallowPlayerControl: false,
                doFade: false,
                isClone: false,
                stats: null,
                aiScript: "Pet"
                
            );
        }
    }
}
