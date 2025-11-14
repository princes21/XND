using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;

namespace CharScripts
{
    public class CharScriptShaco : ICharScript
    {
        public void OnActivate(Champion owner)
        {
            // Optional: custom logic on spawn
        }

        public void OnDeactivate(Champion owner)
        {
            // Optional: cleanup on removal
        }

        public void OnUpdate(float diff)
        {
            // Optional: run per tick
        }

        public void OnMove(Champion owner, Vector2 newPosition)
        {
            // Optional: handle movement events
        }

        public void OnAttack(Champion owner, AttackableUnit target)
        {
            // Optional: handle auto-attack events
        }
    }
}
