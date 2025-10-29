using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace CharScripts
{
    internal class CharScriptSRU_Red : ICharScript
    {
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            AddBuff("GlobalMonsterBuff", 25000.0f, 1, spell, owner, owner, true);
            AddBuff("RedMonsterBuff", 25000.0f, 1, spell, owner, owner, true);
        }
    }
}
