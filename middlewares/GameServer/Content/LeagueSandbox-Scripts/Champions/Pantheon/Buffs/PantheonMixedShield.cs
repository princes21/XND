using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs;

public class PantheonMixedShield : IBuffGameScript
{
    private Buff _buff;

    private Shield NewShield;

    public BuffScriptMetaData BuffMetaData { get; set; } = new()
    {
        BuffType = BuffType.COMBAT_ENCHANCER,
        BuffAddType = BuffAddType.REPLACE_EXISTING
    };

    public StatsModifier StatsModifier { get; } = new();

    public void OnActivate(AttackableUnit unit, Buff buff, Spell spell)
    {
        _buff = buff;
        ObjAIBase owner = spell.CastInfo.Owner;
        float AD = owner.Stats.AttackDamage.Total * 1.40f;
        var ShieldAmount = 90f + AD;
        NewShield = new Shield(owner, owner, true, true, ShieldAmount);
        unit.AddShield(NewShield);
    }

    public void OnUpdate(float diff)
    {
        if (!NewShield.IsConsumed()) return;
        _buff.DeactivateBuff();
    }

    public void OnDeactivate(AttackableUnit unit, Buff buff, Spell spell) { unit.RemoveShield(NewShield); }
}