using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Numerics;
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
    internal class AatroxQ : IBuffGameScript
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
			SetStatus(owner, StatusFlags.Ghosted, true);
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
			PlayAnimation(owner, "Spell1");
			AddParticleTarget(owner, owner, "Aatrox_Base_Q_Cast.troy", owner, 10f);
			AddParticle(owner, null, "Aatrox_Base_Q_Tar_Green.troy", truecoords);
			var randPoint1 = new Vector2(owner.Position.X + (40.0f), owner.Position.Y + 40.0f);	
			ForceMovement(owner, null, randPoint1, 110, 0, 80, 0);
            FaceDirection(truecoords, spell.CastInfo.Owner, true);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
			AddBuff("AatroxQDescent", 5f, 1, spell, owner, owner);	
        }
    }
}