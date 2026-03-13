using AIScripts;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeaguePackets.Game;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using static LeagueSandbox.GameServer.API.ApiMapFunctionManager;
using static System.Net.Mime.MediaTypeNames;


namespace CharScripts
{
    public class CharScriptAlistar : ICharScript // doesn't work
    {
        ObjAIBase _owner;
        Spell spell;
        public SpellSector DamageSector;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            _owner = owner;
            ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("Pulverize"), OnSpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("Headbutt"), OnSpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("TriumphantRoar"), OnSpellCast);
            ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("FerociousHowl"), OnSpellCast);
        }

        public void OnSpellCast(Spell spell)
        {
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Tickrate = 1,
                Length = 175f,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ApRatio = owner.Stats.AbilityPower.Total * 0.4f;
            var damage = 80 * spell.CastInfo.SpellLevel + ApRatio;
            target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }
    }
}