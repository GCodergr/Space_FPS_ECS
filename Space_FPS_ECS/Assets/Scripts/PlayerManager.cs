using UnityEngine;
using System;

namespace SpaceFpsEcs
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Transform bulletSpawnTransform = default;

        public event Action<int> OnCreditsChanged;

        private int credits = 0;

        public int Credits => credits;

        public Transform MyTransform { get; private set; }

        public Transform BulletSpawnTransform => bulletSpawnTransform;

        private void Awake()
        {
            MyTransform = transform;
        }

        public void AddCredits(int creditsToAdd)
        {
            credits += creditsToAdd;
            
            OnCreditsChanged?.Invoke(credits);
        }
    }
}