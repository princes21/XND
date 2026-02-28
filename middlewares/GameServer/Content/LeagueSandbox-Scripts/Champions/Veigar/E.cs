using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Spells
{
    public class VeigarEventHorizon : ISpellScript
    {
        Vector2 truecoords;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,

        };
        Spell spell1;
        AttackableUnit unit2;
        ObjAIBase obj3;
        public SpellSector DRMundoWAOE;
        private float ticks = 0;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            truecoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var distance = Vector2.Distance(spell.CastInfo.Owner.Position, truecoords);
            Minion m = AddMinion(owner, "TestCube", "TestCube", spellPos, owner.Team, 0, false, false, false);
            m.SetCollisionRadius(0.0f);
            m.SetStatus(StatusFlags.Targetable, false);
            m.SetStatus(StatusFlags.Ghosted, true);
            DRMundoWAOE = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = m,
                Length = 450f,
                Tickrate = 2,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 3f
            });

            if (distance > 650f)
            {
                truecoords = GetPointFromUnit(spell.CastInfo.Owner, 650f);
            }

            string cage = "";
            switch (ownerSkinID)
            {
                case 8:
                    cage = "Veigar_Skin08_E_cage_green.troy";
                    break;
                case 6:
                    cage = "Veigar_Skin06_E_cage_green.troy";
                    break;
                case 4:
                    cage = "Veigar_Skin04_E_cage_green.troy";
                    break;
                default:
                    cage = "Veigar_Base_E_cage_green.troy";
                    break;
            }
            AddParticle(owner, null, cage, truecoords, lifetime: 3f);
            //TODO: Stun Hitbox & Buff
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);

            var dist = System.Math.Abs(Vector2.Distance(target.Position, spellPos));

            if (dist >= 300)
            {
                AddBuff("Stun", 1.50f, 1, spell, target, owner);
            }
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("AbilityUsed", 4f, 1, spell, owner, owner);
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;

            if (ticks <= 180)
            {
                truecoords = new Vector2(spell1.CastInfo.TargetPosition.X, spell1.CastInfo.TargetPosition.Z);
                var dist = System.Math.Abs(Vector2.Distance(unit2.Position, truecoords));
                var units = GetUnitsInRange(truecoords, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (dist == 350)
                    {

                        AddBuff("Stun", 1.5f, 1, spell1, unit2, obj3);

                    }
                }

            }
        }
    }
}