using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{

    public class CharScriptKatarina : ICharScript

    {
        Spell Spell;
        AttackableUnit Target;
        ObjAIBase _owner;

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Spell = spell;
            _owner = owner;

            {
                ApiEventManager.OnKillUnit.AddListener(this, owner, OnKillUnit, false);
            }
        }
        public void OnKillUnit(DeathData deathData)
        {
            _owner.Spells[0].LowerCooldown((float)1.25);   //Q
            _owner.Spells[1].LowerCooldown((float)0.5);   //W
            _owner.Spells[2].LowerCooldown((float)0.5);   //E
            _owner.Spells[3].LowerCooldown((float)1.25);   //R
        }
    }
}