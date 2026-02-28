using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{
     
    public class  CharScriptDarius : ICharScript

    {
        Spell Spell;
		AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)

        {
            Spell = spell;
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }
        }
        public void OnLaunchAttack(Spell spell)
        {
			Target = spell.CastInfo.Targets[0].Unit;
            var owner = Spell.CastInfo.Owner;
            if (owner.HasBuff("DariusHemoVisual"))
			{
		    AddBuff("DariusHemo", 6.0f, 5, Spell, Target, owner);
			}
			else
			{
			AddBuff("DariusHemo", 6.0f, 1, Spell, Target, owner);
			}
        }
    }
}