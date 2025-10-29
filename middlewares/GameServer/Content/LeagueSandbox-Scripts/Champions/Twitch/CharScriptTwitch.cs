
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    public class CharScriptTwitch : ICharScript
    {
        Spell Spell;
		AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Spell = spell;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            RemoveBuff(owner, "Invisibility");
            RemoveBuff(owner, "Targetable");
        }
    }
}

