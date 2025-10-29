using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class AatroxPassiveDeath : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private Buff ThisBuff;
		AttackableUnit Unit;
		float timeSinceLastTick;
        float TickingDamage;
		Spell Spell;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			Spell = ownerSpell;
			Unit = unit;
			ThisBuff = buff;
			var owner = ownerSpell.CastInfo.Owner as Champion;
			owner.StopMovement();     
			AddParticleTarget(owner, owner, "Aatrox_Passive_Death_Activate", owner, buff.Duration);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
			PlayAnimation(owner, "Passive_Death",3f);
			buff.SetStatusEffect(StatusFlags.Targetable, false);
			unit.Stats.SetActionState(ActionState.CAN_MOVE, false);
			SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, true);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, true);
			buff.SetStatusEffect(StatusFlags.Stunned, true);
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);
        }
		public void Heal(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
			var BloodP = owner.Stats.CurrentMana;
			var Blood = owner.Stats.ManaPoints.Total * 0.35f;
            var Health = (owner.Stats.CurrentMana + Blood) / 12f;
			var HealthM = owner.Stats.CurrentMana / 12f;
            owner.Stats.CurrentHealth += Health;
            owner.Stats.CurrentMana -= HealthM; 		
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as Champion;
			AddParticleTarget(owner, owner, "Aatrox_Passive_Death_End", owner);
			StopAnimation(owner, "Passive_Death");
			buff.SetStatusEffect(StatusFlags.Targetable, true);
			unit.Stats.SetActionState(ActionState.CAN_MOVE, true);
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, true);
            buff.SetStatusEffect(StatusFlags.Ghosted, false);
			buff.SetStatusEffect(StatusFlags.Stunned, false);
			SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, false);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
			AddBuff("AatroxPassiveActivate", 140f, 1, ownerSpell, ownerSpell.CastInfo.Owner , ownerSpell.CastInfo.Owner,false);
        }
        public void OnUpdate(float diff)
        {
			timeSinceLastTick += diff;
            if (timeSinceLastTick >= 250.0f)
            {              
                Heal(Spell);
                timeSinceLastTick = 0;
            }
        }
    }
}
