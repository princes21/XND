using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;



namespace CharScripts
{
    public class CharScriptGragas : ICharScript
    {
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellCast.AddListener(owner, owner.GetSpell("GragasW"), ApplyHeal, false);
            ApiEventManager.OnSpellCast.AddListener(owner, owner.GetSpell("GragasWAttack"), ApplyHeal, false);
            ApiEventManager.OnSpellChannel.AddListener(owner, owner.GetSpell("GragasW"), ApplyHeal, false);
            ApiEventManager.OnSpellCast.AddListener(owner, owner.GetSpell("GragasE"), ApplyHeal, false);
            ApiEventManager.OnSpellCast.AddListener(owner, owner.GetSpell("GragasR"), ApplyHeal, false);
        }

        public void ApplyHeal(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var Heal = 50 + owner.Stats.AbilityPower.Total * 0.10f + owner.Stats.HealthPoints.Total * 0.015f;
            var IncMaxHealth = owner.Stats.HealthPoints.Total * 0.005f;
            owner.Stats.CurrentHealth += Heal;
            owner.Stats.HealthPoints.BaseBonus += IncMaxHealth;
            AddParticleTarget(owner, owner, "Global_Heal", owner, 1, 1f);

        }
    }
}




 
       