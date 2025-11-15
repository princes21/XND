using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class HallucinateFull : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new()
        {
            OnPreDamagePriority = 10
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell,
                                   AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (!owner.HasBuff("Invisibility"))
                AddParticle(owner, owner, "HallucinatePoof.troy", owner.Position);

            AddBuff("ShacoChildrenOfTheGrave", 0.5f, 1, spell, owner, owner);
        }
    }

    public class HallucinateGuide : BasePetController { }
}