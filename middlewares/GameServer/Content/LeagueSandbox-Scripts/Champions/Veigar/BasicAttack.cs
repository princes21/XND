using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class VeigarBasicAttack : ISpellScript
    {
        ObjAIBase _owner;
        AttackableUnit _targ;

        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Arc
            },
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            ApiEventManager.OnSpellCast.AddListener(this, spell, TargetExecute);
        }
        float ap;
        float dmg;
        public void TargetExecute(Spell spell)
        {
            ap = spell.CastInfo.Owner.Stats.AbilityPower.Total;
            dmg = spell.CastInfo.Owner.Stats.AttackDamage.Total;

            _targ = spell.CastInfo.Targets[0].Unit;
            var x = _owner.GetSpell("VeigarBasicAttack").CreateSpellMissile(ScriptMetadata.MissileParameters);
            //x.SetSpeed(1000f);
            // in spellfluxmissile.json
            // "MissileSpeed": "1000.0000",
            ApiEventManager.OnSpellMissileEnd.AddListener(this, x, OnSpellEnd, true);
        }
        public void OnSpellEnd(SpellMissile mis)
        {
            _targ.TakeDamage(_owner, dmg, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false); // TODO: veigar does not deal critical damage with this implementation, check if it's possible to fix.
        }
    }

}