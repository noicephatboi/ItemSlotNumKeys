using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalCompanyInputUtils.Api;
using NumberItemSlots.Patches;
using System.Xml.Linq;
using UnityEngine.InputSystem;

namespace BaseSlots
{
    public class Keybinds : LcInputActions
    {
        [InputAction("<Keyboard>/1", Name = "SlotOne", ActionType = InputActionType.Button)]
        public InputAction SlotOneKey { get; set; }
        [InputAction("<Keyboard>/2", Name = "SlotTwo", ActionType = InputActionType.Button)]
        public InputAction SlotTwoKey { get; set; }
        [InputAction("<Keyboard>/3", Name = "SlotThree", ActionType = InputActionType.Button)]
        public InputAction SlotThreeKey { get; set; }
        [InputAction("<Keyboard>/4", Name = "SlotFour", ActionType = InputActionType.Button)]
        public InputAction SlotFourKey { get; set; }

    }

    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.HardDependency)]

    public class ItemSlotBase : BaseUnityPlugin
    {
        private const string modGUID = "phatisfat.NumberItemSlots";
        private const string modName = "NumberItemSlots";
        private const string modVersion = "1.0.0";

        internal static Keybinds InputActionsInstance = new Keybinds();

        private readonly Harmony harmony = new Harmony(modGUID);

        private static ItemSlotBase Instance;

        internal static new ManualLogSource Log;



        void Awake()
        {

            if (Instance == null)
            {
                Instance = this;
            }

            Log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            Log.LogInfo(modName + " has loaded. VERSION: " + modVersion);
            

            harmony.PatchAll(typeof(ItemSlotBase));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
        }
    }
}
