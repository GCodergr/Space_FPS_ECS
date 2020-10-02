using UnityEngine;
using TMPro;

namespace SpaceFpsEcs
{
    public class CreditsView : MonoBehaviour
    {
        [Header("Text Component - Set using Inspector")]
        public TextMeshProUGUI creditsText;
        
        private PlayerManager playerManager;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            playerManager.OnCreditsChanged += HandleCreditsChanged;
        }

        private void Start()
        {
            HandleCreditsChanged(playerManager.Credits);
        }

        private void HandleCreditsChanged(int credits)
        {
            creditsText.SetText($"Credits: {credits}");
        }
        
        private void OnDisable()
        {
            if (playerManager != null)
            {
                playerManager.OnCreditsChanged -= HandleCreditsChanged;
            }
        }
    }
}