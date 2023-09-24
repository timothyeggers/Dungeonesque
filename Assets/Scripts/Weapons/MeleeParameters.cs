using Dungeonesque.Core;
using UnityEngine;

namespace Dungeonesque.Weapons
{
    public abstract class MeleeParameters : ScriptableObject, IDamage
    {
        public float AttackSpeed = 1f;

        public float Agility = 9f;

        #region IDamage declaration

        public DamageType Type => DamageType.Slash;
        public float Amount => 10f;

        #endregion
    }
}