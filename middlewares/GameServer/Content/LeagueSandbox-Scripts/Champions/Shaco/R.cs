namespace Spells
{
    public class HallucinateFull : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            OnPreDamagePriority = 10
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            if (!(owner.HasBuff("Invisibility")))
            {
                AddParticle(owner, owner, "HallucinatePoof.troy", owner.Position);
            }

                Buff buff = AddBuff("ShacoChildrenOfTheGrave", 10.400024f, 1, spell, target, spell.CastInfo.Owner);

                OnBuffDeactivated.AddListener(this, buff, OnBuffRemoved, true);

        }

        public void OnBuffRemoved(Buff buff)
        {
        }
    }

    public class HallucinateGuide : BasePetController
    {
    }
}
