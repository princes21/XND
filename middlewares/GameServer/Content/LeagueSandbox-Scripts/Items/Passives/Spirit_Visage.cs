using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using Buffs;

namespace ItemPassives
{
    public class ItemID_3065 : IItemScript
    {
        private int Counter;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            Counter = 0;
        }
        public void OnLaunchAttack(Spell spell)
        {
            var NameofBuffICON = "AtmasImpalerDummySpell";
            var NameofBuffICON_2 = "DefensiveBallCurl";
            var StunBuff = "Stun";
            var owner = spell.CastInfo.Owner;

            //The real code here begins.
            if(!owner.HasBuff(NameofBuffICON))
            {
                RemoveBuff(owner, NameofBuffICON);
                Counter = 0;
            }

            if (Counter == 10 && !owner.HasBuff(NameofBuffICON_2) && owner.Stats.MagicResist.Total >= 300 && owner.HasBuff(NameofBuffICON))
            {
                AddBuff(StunBuff, 5.0f, 1, spell, owner, owner);
                AddBuff(NameofBuffICON_2, 155.5f, 1, spell, owner, owner);
                AddBuff("SpiritVisage_Exalted", 35.5f, 1, spell, owner, owner);
                RemoveBuff(owner, NameofBuffICON);
                RemoveBuff(owner, "SpiritVisage_Stacks");
                Counter = 0;
            }
            // Adds Stacks
            if(owner.Stats.Range.Total <= 255 && !owner.HasBuff(NameofBuffICON_2) && owner.Stats.MagicResist.Total >= 300)
            {
                AddBuff(NameofBuffICON, 4.0f, 1, spell, owner, owner);
                Counter++;
            }
        }

        public void OnDeactivate(ObjAIBase owner)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }
    }
}