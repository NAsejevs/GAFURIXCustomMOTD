using UnityEngine;

namespace GAFURIX;

public class CustomMOTD : IPuckMod
{
    public void OnEnable()
    {
        Debug.Log($"[{Constants.MOD_NAME}] enabled");

        EventManager.Instance.AddEventListener(
            "Event_Server_OnSynchronizeComplete",
            Event_Server_OnSynchronizeComplete
        );
    }

    public void OnDisable()
    {
        Debug.Log($"[{Constants.MOD_NAME}] disabled");

        EventManager.Instance.RemoveEventListener(
            "Event_Server_OnSynchronizeComplete",
            Event_Server_OnSynchronizeComplete
        );
    }

    private void Event_Server_OnSynchronizeComplete(Dictionary<string, object> message)
    {
        ulong clientId = (ulong)message["clientId"];

        UIChat.Instance.Server_SendSystemChatMessage("This is a custom MOTD message!", clientId);
    }
}
