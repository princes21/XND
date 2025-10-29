using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;

namespace Buffs
{
    class AatroxQDescent : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Buff thisBuff;
        private Spell spell;
        private ObjAIBase owner;
		Particle P;
		string pcastname;
        string phitname;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner;
			owner.StopMovement();
            spell = ownerSpell;
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 600f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 600f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
			ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
            ApiEventManager.OnMoveSuccess.AddListener(this, owner, OnMoveSuccess, true);				
			P = AddParticleTarget(owner, owner, "Aatrox_Base_Q_Cast.troy", owner, 10f);
            ForceMovement(owner, null, truecoords, 2450, 0, 0, 0);
        }
        public void OnMoveEnd(AttackableUnit unit)
        {
			if (spell.CastInfo.Owner is Champion c)
            {
				if (c.HasBuff("AatroxR"))
                {
				OverrideAnimation(c, "RUN_ULT", "RUN");
			    }
			    else
			    {
				//OverrideAnimation(c, "RUN", "RUN_ULT");
			    }
				StopAnimation(c, "Spell1",true,true,true);
				SetStatus(owner, StatusFlags.Ghosted, false);			  			
            }
			RemoveBuff(thisBuff);		
			RemoveParticle(P);		
        }
		public void OnMoveSuccess(AttackableUnit unit)
        {
			if (spell.CastInfo.Owner is Champion c)
            {
				AOE(spell);	  			
            }	
        }
        public void AOE(Spell spell)
        {
     		if (spell.CastInfo.Owner is Champion c)
            {
				if (c.SkinID == 1)
                {
                    pcastname = "Aatrox_Skin01_Q_Land";
                }
                else
                {
                    pcastname = "Aatrox_Base_Q_Land";
                }
			    AddParticle(c, null, pcastname, c.Position);
				AddParticleTarget(c, c, "Aatrox_Base_Q_land_sound.troy", c, 10f);
                var damage = 70 + (45 * (spell.CastInfo.SpellLevel - 1)) + (c.Stats.AttackDamage.Total * 0.6f);
                var units = GetUnitsInRange(c.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                            units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
						    AddParticleTarget(c, units[i], "Aatrox_Base_Q_Hit.troy", units[i], 1f);
				            AddParticleTarget(c, units[i], ".troy", units[i], 1f);
                    }
                }
                var unitss = GetUnitsInRange(c.Position, 100f, true);
                for (int i = 0; i < unitss.Count; i++)
                {	
                    if (unitss[i].Team != c.Team && !(unitss[i] is ObjBuilding || unitss[i] is BaseTurret))
                    {
						AddBuff("AatroxQKnockup", 1f, 1, spell, unitss[i], c);	
                    }
                }
			}
        }
    }
}