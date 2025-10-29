using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Enums;
using System.Linq;

namespace ItemPassives
{
    public class ItemID_3068 : IItemScript
    {
        private const float BURN_RADIUS = 300f;
        private const float BURN_INTERVAL = 1f;
        private float _timeSinceLastBurn = 0f;
        private ObjAIBase _owner;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            _owner = owner;
            _timeSinceLastBurn = 0f;
        }

        public void OnUpdate(float diff)
        {
            float deltaTime = diff / 1000f;
            _timeSinceLastBurn += deltaTime;

            var _Damage = 20f + _owner.Stats.HealthPoints.Total * 0.03f;

            if (_timeSinceLastBurn >= BURN_INTERVAL)
            {
                _timeSinceLastBurn = 0f;
                var enemies = ApiFunctionManager.GetUnitsInRange(_owner.Position, BURN_RADIUS, true)
                    .Where(u => u.Team != _owner.Team && u is AttackableUnit)
                    .Cast<AttackableUnit>();
                foreach (var enemy in enemies)
                {
                    if(!_owner.IsDead)
                    {
                        enemy.TakeDamage(_owner, _Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                    }
                }
            }
        }
    }
}