using System;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Handles pets waking up monsters
    /// </summary>
    partial class Creature
    {
        /// <summary>
        /// Wakes up any monsters within the applicable range
        /// </summary>
        public void PetCheckMonsters(float rangeSquared = RadiusAwarenessSquared)
        {
            //if (!Attackable) return;

            var visibleObjs = PhysicsObj.ObjMaint.VisibleObjectTable.Values;

            foreach (var obj in visibleObjs)
            {
                if (PhysicsObj == obj) continue;

                var monster = obj.WeenieObj.WorldObject as Creature;

                if (monster == null || monster is Player) continue;

                if (Location.SquaredDistanceTo(monster.Location) < rangeSquared)
                    PetAlertMonster(monster);
            }
        }

        /// <summary>
        /// Wakes up a monster if it can be alerted
        /// </summary>
        private bool PetAlertMonster(Creature monster)
        {
            if (monster.Attackable && monster.MonsterState == State.Idle && monster.Tolerance == Tolerance.None)
            {
                monster.AttackTarget = this;
                monster.WakeUp();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when a combat pet attacks a monster
        /// </summary>
        public void PetOnAttackMonster(Creature monster)
        {
            /*Console.WriteLine($"{Name}.PetOnAttackMonster({monster.Name})");
            Console.WriteLine($"Attackable: {monster.Attackable}");
            Console.WriteLine($"Tolerance: {monster.Tolerance}");*/

            if (monster.MonsterState == State.Idle && !monster.Tolerance.HasFlag(Tolerance.NoAttack))
            {
                monster.AttackTarget = this;
                monster.WakeUp();
            }
        }
    }
}
