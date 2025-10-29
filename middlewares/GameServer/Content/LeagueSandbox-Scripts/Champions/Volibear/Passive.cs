using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{
     
    public class  CharScriptVolibear : ICharScript

    {
        Spell Spell;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {       
            if (owner.HasBuff("VolibearQ"))
            {
				PlayAnimation(owner, "spell1_idle");
			}
			else
			{
				StopAnimation(owner,"spell1_idle");
			}
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }
    }
}