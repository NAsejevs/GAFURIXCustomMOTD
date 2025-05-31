using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace GAFURIX
{
    public class CustomMOTD : IPuckMod
    {
        static readonly Harmony harmony = new Harmony("GAFURIX.CustomMOTD");
        static string motd = "Welcome to the server!";

        [HarmonyPatch(typeof(UIChatController), "Event_Server_OnSynchronizeComplete")]
        public class UIChatControllerPatch
        {
            [HarmonyPrefix]
            public static bool Prefix(Dictionary<string, object> message)
            {
                ulong clientId = (ulong)message["clientId"];

                UIChat.Instance.Server_SendSystemChatMessage(motd, clientId);
                return false;
            }
        }

        public void OnEnable()
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

                Debug.Log($"[{Constants.MOD_NAME}] enabled");
            }
            catch (Exception e)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] failed to enable: {e.Message}");
            }
        }

        public void OnDisable()
        {
            try
            {
                harmony.UnpatchSelf();

                Debug.Log($"[{Constants.MOD_NAME}] disabled");
            }
            catch (Exception e)
            {
                Debug.LogError($"[{Constants.MOD_NAME}] failed to disable: {e.Message}");
            }
        }
    }
}
