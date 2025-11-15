using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ShacoCOTGRevive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new()
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            buff.DeactivateBuff();   // instant tick → spawn clone
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.SourceUnit is not Champion ch) return;

            var pet = CreateClonePet(
                owner: ch,
                spell: ownerSpell,
                cloned: ch,
                position: ch.Position,
                buffName: "ShacoCOTGPetBuff2",
                lifeTime: 18f,
                cloneInventory: true,
                showMinimapIfClone: true,
                disallowPlayerControl: false,
                doFade: false
            );

            AddBuff("ShacoCOTGPetBuff2", 18f, 1, ownerSpell, pet, ch);
        }

        public void OnUpdate(float diff) { }
    }
}