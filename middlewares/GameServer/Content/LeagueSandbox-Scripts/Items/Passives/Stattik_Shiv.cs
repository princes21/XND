using System.Numerics;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using GameServerCore.Scripting.CSharp;

namespace ItemPassives
{
    public class ItemID_3087 : IItemScript
    {
        private ObjAIBase _owner;
        private Vector2 _lastPos;
        private float _movedAccum;

        private const float MOVE_STEP_DIST = 32f;
        private const byte MOVE_STEP_STACKS = 5;
        private const byte ON_HIT_STACKS = 25;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(ObjAIBase owner)
        {
            _owner = owner;
            _lastPos = owner.Position;
            _movedAccum = 0f;

            // Adăugăm buff-ul container
            AddBuff("ItemStatikShankCharge", float.MaxValue, 1, null, owner, owner);

            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            ApiEventManager.OnUpdateStats.AddListener(this, owner, OnUpdateStats, false);
        }

public void OnDeactivate(ObjAIBase owner)
{
    ApiEventManager.OnLaunchAttack.RemoveListener(this, owner);
    ApiEventManager.OnUpdateStats.RemoveListener(this, owner);

    // curățăm buff-ul container dacă itemul e vândut
    if (HasBuff(owner, "ItemStatikShankCharge"))
    {
        RemoveBuff(owner, "ItemStatikShankCharge");
    }
}
        private void OnLaunchAttack(Spell spell)
        {
            // +10 stive vizibile
            for (int i = 0; i < ON_HIT_STACKS; i++)
            {
                AddBuff("ItemStatikShankCharge", float.MaxValue, 1, spell, _owner, _owner);
            }
        }

        private void OnUpdateStats(LeagueSandbox.GameServer.GameObjects.AttackableUnits.AttackableUnit who, float diff)
        {
            var cur = _owner.Position;
            var moved = Vector2.Distance(cur, _lastPos);
            if (moved <= 0f) return;

            _movedAccum += moved;
            _lastPos = cur;

            while (_movedAccum >= MOVE_STEP_DIST)
            {
                _movedAccum -= MOVE_STEP_DIST;
                AddBuff("ItemStatikShankCharge", float.MaxValue, MOVE_STEP_STACKS, null, _owner, _owner);
            }
        }
    }
}
