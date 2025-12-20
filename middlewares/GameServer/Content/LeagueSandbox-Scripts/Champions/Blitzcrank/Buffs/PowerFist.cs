using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;


namespace Buffs
{
    internal class PowerFist: IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        ObjAIBase Unit;
        Spell spell;
        
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase ai)
            {
                Unit = ai;

                ApiEventManager.OnHitUnit.AddListener(this, ai, TargetExecute, true);

                ai.SkipNextAutoAttack();


            }
            
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //ApiEventManager.OnHitUnit.RemoveListener(this);

        }
        public void TargetExecute(DamageData damageData)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
                float ad = Unit.Stats.AttackDamage.Total * 1.5f;
                var target = damageData.Target;
                var CasterPosition = Unit.Position;
                float damage = 10*(Unit.GetSpell("PowerFist").CastInfo.SpellLevel - 2) + ad;
                
                
                target.TakeDamage(Unit, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                var trueCoords = GetPointFromUnit(Unit, 350f);
                AddBuff("Stun", 0.75f, 1, spell, target, Unit);
                ForceMovement(target, "RUN", trueCoords, 2200, 0, 0, 0);
                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {

        }
    }
}