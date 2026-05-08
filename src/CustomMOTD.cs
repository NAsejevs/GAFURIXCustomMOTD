using System;
using System.IO;
using HarmonyLib;

namespace GAFURIX
{
    public class CustomMOTD : IPuckPlugin
    {
        static readonly Logger Logger = new Logger(Constants.MOD_NAME);
        static readonly Harmony harmony = new Harmony("GAFURIX.CustomMOTD");
        static string motd = "Welcome to the server!";

        [HarmonyPatch(typeof(BaseGameMode<BaseGameModeConfig>), "OnPlayerJoined")]
        public class OnPlayerJoinedPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(BaseGameMode<BaseGameModeConfig> __instance, Player player)
            {
                string username = StringUtils.WrapInTeamColor(
                    player.Username.Value.ToString(),
                    player.Team
                );

                __instance.ChatManager.Server_BroadcastChatMessage(
                    $"{username} has joined the server"
                );
                __instance.ChatManager.Server_SendChatMessageToClients(
                    motd,
                    new ulong[] { player.OwnerClientId }
                );
                return false;
            }
        }

        public bool OnEnable()
        {
            try
            {
                string rootPath = Path.GetFullPath(".");
                string motdPath = Path.Combine(rootPath, "motd.txt");

                if (!File.Exists(motdPath))
                    File.WriteAllText(motdPath, motd);
                else
                    motd = File.ReadAllText(motdPath);

                harmony.PatchAll();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to enable: {e.Message}");
                return false;
            }
        }

        public bool OnDisable()
        {
            try
            {
                harmony.UnpatchSelf();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to disable: {e.Message}");
                return false;
            }
        }
    }
}
