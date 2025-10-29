using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using System.Linq;
using System.Collections.Generic;
using System;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Chatbox;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.Players;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;

namespace AIScripts
{
    public class CharAI : IAIScript
    {
        private object _playerManager;
        private object userId;
        private object stats;

        public AIScriptMetaData AIScriptMetaData { get; set; } = new AIScriptMetaData();
        public Champion ChampionInstance { get; private set; }
        bool isInCombat = false;
        private const float combatDuration = 10.0f; // Duration of combat in seconds
        private float combatTimer = 0.0f; // Timer to track combat duration
        private bool minionsSpawned = true;
        private float TimeSinceLastDamage { get; set; }
        private const float recallDelay = 5.0f; // Delay in seconds before recalling
        bool isUnderTower = false;

        float AttackDamage { get; set; }


        public void OnActivate(ObjAIBase owner)
        {
            // Check if the owner is a Champion instance
            if (owner is Champion champion)
            {
                // Set ChampionInstance to the owner
                ChampionInstance = champion;

                // Set IsBot to true for the ChampionInstance
                ChampionInstance.IsBot = true;

                // Register event listener for OnTakeDamage event
                ApiEventManager.OnTakeDamage.AddListener(this, ChampionInstance, OnTakeDamage, false);


                // Level up the ChampionInstance
                ChampionInstance.LevelUp();

                // Now that ChampionInstance is initialized, set isInCombat to false
                isInCombat = false;

                // Now that ChampionInstance is initialized, set isUnderTower to false
                isUnderTower = false;
            }
        }

        public void OnUpdate(float diff)
        {
            if (ChampionInstance != null && !ChampionInstance.IsDead)
            {
                TimeSinceLastDamage += diff; // Increment the time since last damage

                // Evaluate objectives and prioritize attacking enemy champions
                EvaluateObjectives();

                float healthThreshold = 0.2f; // Adjust this value according to your needs
                if (ChampionInstance.Stats.CurrentHealth / ChampionInstance.Stats.HealthPoints.Total <= healthThreshold)
                {
                    // If health is low, retreat
                    Retreat();
                }

                // Check if minions have spawned
                if (!minionsSpawned)
                {
                    // Stay under tower if minions haven't spawned
                    StayUnderTower();
                }
            }
        }

        private void StayUnderTower()
        {
            Vector2 towerPosition = ChampionInstance.Team == TeamId.TEAM_BLUE ? new Vector2(5500, 11500) : new Vector2(13300, 4300);
            MoveToPosition(towerPosition);
        }

        private bool ShouldBeAggressive()
        {
            // Check if the bot should be aggressive based on health, mana, and other factors
            return ChampionInstance.Stats.CurrentHealth > 0.5f
                && ChampionInstance.Stats.CurrentMana > 0.3f
                && GetNearbyChampions().Count > 0; // Add more conditions as needed
        }

        private void EvaluateObjectives()
        {
            // Get nearby enemy champions
            List<Champion> nearbyEnemyChampions = GetNearbyChampions();

            if (nearbyEnemyChampions.Count > 0)
            {
                // Attack the closest enemy champion if any are nearby
                Champion closestEnemyChampion = nearbyEnemyChampions.OrderBy(champion => Vector2.Distance(ChampionInstance.Position, champion.Position)).First();
                Attack(closestEnemyChampion);
            }
            else
            {
                // Check for nearby minions if no enemy champions are nearby
                List<AttackableUnit> nearbyMinions = GetNearbyMinions();

                if (nearbyMinions.Count > 0)
                {
                    // Attack the closest minion
                    Attack(nearbyMinions.First());
                }
                else
                {
                    // If no enemy champions or minions are nearby, check for enemy structures
                    List<AttackableUnit> enemyStructures = GetNearbyEnemyStructures();

                    if (enemyStructures.Count > 0)
                    {
                        // Attack the closest enemy structure
                        Attack(enemyStructures.First());
                    }
                    else
                    {
                        // If no nearby enemy structures, prioritize farming or other actions
                        // For simplicity, stay under tower
                        StayUnderTower();
                    }
                }
            }
        }
        private List<AttackableUnit> GetNearbyEnemyStructures()
        {
            // Get the bot's current position
            Vector2 botPosition = ChampionInstance.Position;

            // Scan for nearby units within the specified range
            List<AttackableUnit> units = GetUnitsInRange(botPosition, 1200.0f, true);

            // Filter out enemy structures from the list of units
            List<AttackableUnit> enemyStructures = units.Where(unit => IsEnemyStructure(unit)).ToList();

            return enemyStructures;
        }

        private bool IsEnemyStructure(AttackableUnit unit)
        {
            // Check if the unit is an enemy tower or inhibitor
            if (unit is LaneTurret || unit is Inhibitor)
            {
                // Add more checks as needed for other types of structures
                if (unit.Team != ChampionInstance.Team && !unit.IsDead)
                {
                    return true;
                }
            }
            return false;
        }

        private void Retreat()
        {
            // Check if there are nearby enemies before retreating
            List<Champion> nearbyEnemyChampions = GetNearbyChampions();
            if (nearbyEnemyChampions.Count == 0)
            {
                // Set move order to MoveTo a safe location, such as under a tower
                MoveTowardsBase();
            }
            else
            {
                // If enemies are nearby, try to kite them while retreating
                KiteEnemies(nearbyEnemyChampions);
            }
        }

        private void MoveTowardsBase()
        {
            Vector2 basePosition = GetBasePosition();
            {
                // move towards the base position
                MoveToPosition(basePosition);
            }
        }




        private Vector2 GetBasePosition()
        {
            // Return the fountain position based on the bot's team
            if (ChampionInstance.Team == TeamId.TEAM_BLUE)
            {
                return new Vector2(500, 1500); // Team Blue fountain position
            }
            else if (ChampionInstance.Team == TeamId.TEAM_PURPLE)
            {
                return new Vector2(13500, 5200); // Team Purple fountain position
            }
            else
            {
                // Default to the center of the map if team is not defined
                return new Vector2(7500, 7500);
            }
        }




        private Vector2 GetClosestMinionPosition()
        {
            // Get the bot's current position
            Vector2 botPosition = ChampionInstance.Position;

            // Scan for nearby units within the specified range
            List<AttackableUnit> units = GetUnitsInRange(botPosition, 1000.0f, true);

            // Filter out minions from the list of units
            List<AttackableUnit> minions = units.Where(unit => unit is Minion && !unit.IsDead).ToList();

            if (minions.Count > 0)
            {
                // Find the closest minion position
                AttackableUnit closestMinion = minions.OrderBy(minion => Vector2.Distance(botPosition, minion.Position)).First();
                return closestMinion.Position;
            }
            else
            {
                // If no minions found, return the bot's current position
                return botPosition;
            }
        }

        private void AttackEnemyChampionIfInRange(List<Champion> nearbyEnemyChampions)
        {
            foreach (Champion enemyChampion in nearbyEnemyChampions)
            {
                float attackRange = ChampionInstance.Stats.Range.Total;
                float distanceToEnemy = Vector2.Distance(ChampionInstance.Position, enemyChampion.Position);

                // If the enemy champion is within attack range, attack it
                if (distanceToEnemy <= attackRange)
                {
                    Attack(enemyChampion);
                    break; // Attack the first enemy in range and break the loop
                }
            }
        }


        private void FollowAlliedMinions()
        {
            // Get the bot's current position
            Vector2 botPosition = ChampionInstance.Position;

            // Scan for nearby units within a certain range
            float detectionRange = 2000.0f; // Adjust the range as needed
            List<AttackableUnit> units = GetUnitsInRange(botPosition, detectionRange, true);

            // Filter out allied minions from the list of units
            List<AttackableUnit> alliedMinions = units.Where(unit => unit is Minion && unit.Team == ChampionInstance.Team).ToList();

            if (alliedMinions.Count > 0)
            {
                // Calculate the average position of allied minions
                Vector2 averageMinionPosition = Vector2.Zero;
                foreach (var minion in alliedMinions)
                {
                    averageMinionPosition += minion.Position;
                }
                averageMinionPosition /= alliedMinions.Count;

                // Move the bot towards the average position of allied minions
                MoveToPosition(averageMinionPosition);
            }
            else
            {
                // If no allied minions are nearby, do nothing
            }
        }


        private bool IsUnderTower(Vector2 position)
        {
            // Determine if the position is under a tower based on its distance from towers
            // You can implement your logic here to check tower positions and their ranges
            // For simplicity, you can assume the bot is under a tower if it's within a certain range from a tower
            // Example: return true if the bot is within 1500 units of a tower
            // You can adjust this value based on your game's map and tower range
            // Here's a hypothetical implementation:
            float towerRange = 1500.0f;
            isUnderTower = true;
            return GetUnitsInRange(position, towerRange, true).Any(unit => unit is LaneTurret);

        }

        private AttackableUnit GetNearestTower(Vector2 position)
        {
            // Get all the towers in the game
            List<AttackableUnit> LaneTurret = GetTowers();

            // Find the nearest tower to the bot's position
            AttackableUnit nearestTower = LaneTurret.OrderBy(tower => Vector2.Distance(position, tower.Position)).First();

            return nearestTower;
        }

        private List<AttackableUnit> GetTowers()
        {
            // Get all the towers in the game
            // You can implement this method based on how towers are stored in your game
            // For example, if towers are stored in a list somewhere, you can return that list here
            // Make sure to filter out destroyed towers if necessary
            // Example:
            Vector2 botPosition = ChampionInstance.Position;


            List<AttackableUnit> units = GetUnitsInRange(botPosition, 2000.0f, true);

            List<AttackableUnit> towers = units.OfType<LaneTurret>().Select(tower => (AttackableUnit)tower).Where(tower => !tower.IsDead).ToList();
            return towers;
        }
        private void AttackTowers()
        {
            // Get the bot's current position
            Vector2 botPosition = ChampionInstance.Position;

            // Check if the bot is under a tower
            bool underTower = IsUnderTower(botPosition);

            if (underTower)
            {
                // Get the nearest enemy tower
                AttackableUnit nearestTower = GetNearestTower(botPosition);

                // Check if there are minions within the tower's range
                List<AttackableUnit> minionsInRange = GetUnitsInRange(nearestTower.Position, 750.0f, true)
                    .Where(unit => unit is Minion)
                    .ToList();

                if (minionsInRange.Count > 0)
                {
                    // Attack the tower
                    Attack(nearestTower);
                }
            }
        }
        private Vector2 GetClosestEnemyChampionPosition(AttackableUnit Champion)
        {
            // Get the bot's current position
            Vector2 botPosition = ChampionInstance.Position;

            // Scan for nearby units within the specified range
            List<AttackableUnit> units = GetUnitsInRange(botPosition, 2000.0f, true);

            // Filter out enemy champions from the list of units
            List<Champion> enemyChampions = units.OfType<Champion>().Where(champion => champion.Team != ChampionInstance.Team && !champion.IsDead).ToList();

            if (enemyChampions.Count > 0)
            {
                // Find the closest enemy champion position
                Champion closestEnemyChampion = enemyChampions.OrderBy(champion => Vector2.Distance(botPosition, champion.Position)).First();
                return closestEnemyChampion.Position;
            }
            else
            {
                // If no enemy champions found, return a default position
                return botPosition;
            }
        }

        private List<Champion> GetNearbyChampions()
        {
            // Get the bot's current position
            Vector2 botPosition = ChampionInstance.Position;

            // Scan for nearby units within the specified range
            List<AttackableUnit> units = GetUnitsInRange(botPosition, 2000.0f, true);

            // Filter out enemy champions from the list of units
            List<Champion> nearbyEnemyChampions = units.OfType<Champion>()
                .Where(champion => champion.Team != ChampionInstance.Team && !champion.IsDead)
                .ToList();

            return nearbyEnemyChampions;
        }

        private List<AttackableUnit> GetNearbyMinions()
        {
            // Get the bot's current position
            Vector2 botPosition = ChampionInstance.Position;

            // Scan for nearby units within the specified range
            List<AttackableUnit> units = GetUnitsInRange(botPosition, 2000.0f, true);

            // Filter out minions from the list of units
            List<AttackableUnit> nearbyMinions = units.Where(unit => unit is Minion).ToList();

            return nearbyMinions;
        }

        private void FarmMinions(List<AttackableUnit> minions)
        {
            // Iterate through the list of minions and attack them
            foreach (AttackableUnit minion in minions)
            {
                Attack(minion);
            }
        }

        private void AttackEnemyChampion()
        {
            // Get nearby enemy champions
            List<Champion> nearbyEnemyChampions = GetNearbyChampions();

            // If there are nearby enemy champions, attack the closest one
            if (nearbyEnemyChampions.Count > 0)
            {
                Champion closestEnemyChampion = nearbyEnemyChampions.OrderBy(champion => Vector2.Distance(ChampionInstance.Position, champion.Position)).First();
                Attack(closestEnemyChampion);
            }
        }


        private void KiteEnemies(List<Champion> nearbyEnemyChampions)
        {
            // Implement kiting behavior to retreat while attacking enemies
            // You can calculate movement direction based on enemy positions
            // and the bot's attack range
            // Here's a simplified example:
            foreach (Champion enemyChampion in nearbyEnemyChampions)
            {
                Vector2 retreatDirection = ChampionInstance.Position - enemyChampion.Position;
                Vector2 targetPosition = ChampionInstance.Position + retreatDirection;

                MoveToPosition(targetPosition);
            }
        }

        public void OnTakeDamage(DamageData damageData)
        {
            if (ChampionInstance != null)
            {
                combatTimer = 0.0f;
                isInCombat = true;
                Vector2 attackerPosition = damageData.Attacker.Position;
                TimeSinceLastDamage = 0.0f; // Reset the time since last damage

            }
        }


        public void Attack(AttackableUnit attacker)
        {
            // Check if the attacker is within vision range
            if (attacker != null && !attacker.IsDead)
            {
                // Get the position of the attacker
                Vector2 attackerPosition = attacker.Position;

                // Set move order to AttackMove towards the attacker's position
                ChampionInstance.UpdateMoveOrder(OrderType.AttackMove);
                ChampionInstance.SetWaypoints(new List<Vector2> { attackerPosition });

                // Optional: You may also want to set the target unit to the attacker
                ChampionInstance.SetTargetUnit(attacker);
            }
        }


        private Vector2 GetRandomPosition()
        {
            // Generate random target position within the game map bounds
            Random random = new Random();
            float targetX = random.Next(-100000, 100000);
            float targetY = random.Next(-100000, 100000);
            Vector2 randomPosition = new Vector2(targetX, targetY);

            return randomPosition;
        }


        public void MoveToPosition(Vector2 targetPosition)
        {
            // Get the bot's current position
            Vector2 botPosition = ChampionInstance.Position;

            // Create a list of waypoints with bot's current position and target position
            List<Vector2> waypoints = new List<Vector2> { botPosition, targetPosition };

            // Set move order to MoveTo and set the waypoints
            ChampionInstance.MoveOrder = OrderType.MoveTo;
            ChampionInstance.SetWaypoints(waypoints);
        }
    }
}