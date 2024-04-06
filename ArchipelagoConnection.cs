using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using System;
using System.Threading.Tasks;

namespace HaakRandomizer
{
    class ArchipelagoConnection
    {
        public static ArchipelagoSession session;


        private string slot;
        private string server;

        public ArchipelagoConnection(string host, string slot)
        {
            string[] hostSplit = host.Split(':');
            session = ArchipelagoSessionFactory.CreateSession(host);
            this.slot = slot;
            this.server = host;
        }

        public void Connect()
        {
            LoginResult result = session.TryConnectAndLogin("Haak", this.slot, ItemsHandlingFlags.AllItems);

            if (!result.Successful)
            {
                LoginFailure failure = (LoginFailure)result;
                string errorMessage = $"Failed to Connect to {server} as {slot}:";
                foreach (string error in failure.Errors)
                {
                    errorMessage += $"\n    {error}";
                }
                foreach (ConnectionRefusedError error in failure.ErrorCodes)
                {
                    errorMessage += $"\n    {error}";
                }

                Plugin.PatchLogger.LogInfo(errorMessage);
                return; // Did not connect, show the user the contents of `errorMessage`
            }

            //var slotData = session.DataStorage.GetSlotData(ArchipelagoConnection.session.ConnectionInfo.Slot);
            //requiredEndings = int.Parse(slotData["endings"].ToString());
            //gemsRandomized = int.Parse(slotData["randomizeGems"].ToString());
            //gemsAmount = int.Parse(slotData["gemsInPool"].ToString());
            //gemsRequired = int.Parse(slotData["gemsRequired"].ToString());
            //treasureRoomSword = int.Parse(slotData["treasureSword"].ToString());

            // Successfully connected, `ArchipelagoSession` (assume statically defined as `session` from now on) can now be used to interact with the server and the returned `LoginSuccessful` contains some useful information about the initial connection (e.g. a copy of the slot data as `loginSuccess.SlotData`)
            var loginSuccess = (LoginSuccessful)result;
        }

        public static void SendLocation(string name)
        {
            long id = session.Locations.GetLocationIdFromName("Haak", name);
            session.Locations.CompleteLocationChecks(id);
        }

        public static async void Check_Send_completion()
        {
            var statusUpdatePacket = new StatusUpdatePacket();
            statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
            await session.Socket.SendPacketAsync(statusUpdatePacket);
        }
    }
}
