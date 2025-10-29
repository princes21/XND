
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{

    public class CharScriptVayne : ICharScript
    {
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            AddBuff("PassiveMSVayne", 5f, 1, spell, owner, owner, true);
        }
    }
}