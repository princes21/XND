namespace Buffs
{
    internal class ShacoCOTGRevive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            //It should check if the unit is a zombie (Sion passive / Yorick R) and wait until the unit isn't anymore for then spawn the ghost and then deactivate itself.
            buff.DeactivateBuff();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.SourceUnit is Champion ch && unit is ObjAIBase obj)
            {
                var pet = CreateClonePet(
                    owner: ch,
                    spell: ownerSpell,
                    cloned: ch,
                    position: Vector2.Zero,
                    buffName: "ShacoCotgPetSlow",
                    lifeTime: 0.0f,
                    cloneInventory: true,
                    showMinimapIfClone: true,
                    disallowPlayerControl: false,
                    doFade: false
                    );
                AddBuff("ShacoCOTGPetBuff2", 18.0f, 1, ownerSpell, pet, ch);
            }
        }
    }
}
