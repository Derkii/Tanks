using System;
using Bot;
using TagScripts;
using UnityEngine;

namespace Game.Settings
{
    public static class GameSettings
    {
        public static GameSettingsData Settings = new GameSettingsData();

        public static int GetStartHealth(Component component)
        {
            return component.TryGetComponent(out BotComponent _) ? Settings.BotHealth :
                component.TryGetComponent(out Player _) ? Settings.PlayerHealth : throw new NotImplementedException();
        }
    }
    [Serializable]
    public struct GameSettingsData
    {
        public float Volume;
        public int PlayerHealth, BotHealth, MaxBotsCount;
    }
}