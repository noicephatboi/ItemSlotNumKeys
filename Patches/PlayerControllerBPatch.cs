using GameNetcodeStuff;
using HarmonyLib;
using BaseSlots;
using System;
using UnityEngine;
using BepInEx.Logging;
using BepInEx;

namespace NumberItemSlots.Patches
{


    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        private GrabbableObject currentlyGrabbingObject;
        static PlayerControllerB play;
        static GrabbableObject fillSlotWithItem;

        static internal ManualLogSource mls;

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void onSlotChangePress(PlayerControllerB __instance)
        {
            play = __instance;

            if (BaseSlots.ItemSlotBase.InputActionsInstance.SlotOneKey.triggered && !__instance.isPlayerDead && !__instance.isTypingChat && !__instance.inTerminalMenu && __instance == GameNetworkManager.Instance.localPlayerController)
            {
                HUDManager.Instance.itemSlotIconFrames[0].GetComponent<Animator>().SetBool("selectedSlot", true);
                __instance.currentItemSlot = 0;
                ItemSlotBase.Log.LogInfo("Changing to slot ONE");

                if (play.IsOwner)
                {
                    for (int index = 0; index < HUDManager.Instance.itemSlotIconFrames.Length; ++index)
                        HUDManager.Instance.itemSlotIconFrames[index].GetComponent<Animator>().SetBool("selectedSlot", false);
                    HUDManager.Instance.itemSlotIconFrames[0].GetComponent<Animator>().SetBool("selectedSlot", true);
                }

                if ((UnityEngine.Object)fillSlotWithItem != (UnityEngine.Object)null)
                {
                    play.ItemSlots[0] = fillSlotWithItem;
                    if (play.IsOwner)
                    {
                        HUDManager.Instance.itemSlotIcons[0].sprite = fillSlotWithItem.itemProperties.itemIcon;
                        HUDManager.Instance.itemSlotIcons[play.currentItemSlot].enabled = true;
                    }
                }

                if ((UnityEngine.Object)play.currentlyHeldObjectServer != (UnityEngine.Object)null)
                {
                    play.currentlyHeldObjectServer.playerHeldBy = play;
                    if (play.IsOwner)
                        SetSpecialGrabAnimationBool(false, play.currentlyHeldObjectServer);
                    play.currentlyHeldObjectServer.PocketItem();
                    if ((UnityEngine.Object)play.ItemSlots[0] != (UnityEngine.Object)null && !string.IsNullOrEmpty(play.ItemSlots[0].itemProperties.pocketAnim))
                        play.playerBodyAnimator.SetTrigger(play.ItemSlots[0].itemProperties.pocketAnim);
                }

                if ((UnityEngine.Object)play.ItemSlots[0] != (UnityEngine.Object)null)
                {
                    play.ItemSlots[0].playerHeldBy = play;
                    play.ItemSlots[0].EquipItem();
                    if (play.IsOwner)
                        SetSpecialGrabAnimationBool(true, play.ItemSlots[0]);
                    if ((UnityEngine.Object)play.currentlyHeldObjectServer != (UnityEngine.Object)null)
                    {
                        if (play.ItemSlots[0].itemProperties.twoHandedAnimation || play.currentlyHeldObjectServer.itemProperties.twoHandedAnimation)
                        {
                            play.playerBodyAnimator.ResetTrigger("SwitchHoldAnimationTwoHanded");
                            play.playerBodyAnimator.SetTrigger("SwitchHoldAnimationTwoHanded");
                        }
                        play.playerBodyAnimator.ResetTrigger("SwitchHoldAnimation");
                        play.playerBodyAnimator.SetTrigger("SwitchHoldAnimation");
                    }
                    play.twoHandedAnimation = play.ItemSlots[0].itemProperties.twoHandedAnimation;
                    play.twoHanded = play.ItemSlots[0].itemProperties.twoHanded;
                    play.playerBodyAnimator.SetBool("GrabValidated", true);
                    play.playerBodyAnimator.SetBool("cancelHolding", false);
                    play.isHoldingObject = true;
                    play.currentlyHeldObjectServer = play.ItemSlots[0];
                }
                else
                {
                    if (!play.IsOwner && (UnityEngine.Object)play.heldObjectServerCopy != (UnityEngine.Object)null)
                        play.heldObjectServerCopy.SetActive(false);
                    if (play.IsOwner)
                        HUDManager.Instance.ClearControlTips();
                    play.currentlyHeldObjectServer = (GrabbableObject)null;
                    play.currentlyHeldObject = (GrabbableObject)null;
                    play.isHoldingObject = false;
                    play.twoHanded = false;
                    play.playerBodyAnimator.SetBool("cancelHolding", true);
                }

                if (!play.IsOwner)
                    return;
                if (play.twoHanded)
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 0.1f, 0.13f, 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = true;
                }
                else
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 1.5f, endAlpha: 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = false;
                }
            }

            if (BaseSlots.ItemSlotBase.InputActionsInstance.SlotTwoKey.triggered && !__instance.isPlayerDead && !__instance.isTypingChat && !__instance.inTerminalMenu && __instance == GameNetworkManager.Instance.localPlayerController)
            {
                HUDManager.Instance.itemSlotIconFrames[1].GetComponent<Animator>().SetBool("selectedSlot", true);
                __instance.currentItemSlot = 1;
                ItemSlotBase.Log.LogInfo("Changing to slot TWO");


                if (play.IsOwner)
                {
                    for (int index = 0; index < HUDManager.Instance.itemSlotIconFrames.Length; ++index)
                        HUDManager.Instance.itemSlotIconFrames[index].GetComponent<Animator>().SetBool("selectedSlot", false);
                    HUDManager.Instance.itemSlotIconFrames[1].GetComponent<Animator>().SetBool("selectedSlot", true);
                }

                if ((UnityEngine.Object)fillSlotWithItem != (UnityEngine.Object)null)
                {
                    play.ItemSlots[1] = fillSlotWithItem;
                    if (play.IsOwner)
                    {
                        HUDManager.Instance.itemSlotIcons[1].sprite = fillSlotWithItem.itemProperties.itemIcon;
                        HUDManager.Instance.itemSlotIcons[play.currentItemSlot].enabled = true;
                    }
                }

                if ((UnityEngine.Object)play.currentlyHeldObjectServer != (UnityEngine.Object)null)
                {
                    play.currentlyHeldObjectServer.playerHeldBy = play;
                    if (play.IsOwner)
                        SetSpecialGrabAnimationBool(false, play.currentlyHeldObjectServer);
                    play.currentlyHeldObjectServer.PocketItem();
                    if ((UnityEngine.Object)play.ItemSlots[1] != (UnityEngine.Object)null && !string.IsNullOrEmpty(play.ItemSlots[1].itemProperties.pocketAnim))
                        play.playerBodyAnimator.SetTrigger(play.ItemSlots[1].itemProperties.pocketAnim);
                }

                if ((UnityEngine.Object)play.ItemSlots[1] != (UnityEngine.Object)null)
                {
                    play.ItemSlots[1].playerHeldBy = play;
                    play.ItemSlots[1].EquipItem();
                    if (play.IsOwner)
                        SetSpecialGrabAnimationBool(true, play.ItemSlots[1]);
                    if ((UnityEngine.Object)play.currentlyHeldObjectServer != (UnityEngine.Object)null)
                    {
                        if (play.ItemSlots[1].itemProperties.twoHandedAnimation || play.currentlyHeldObjectServer.itemProperties.twoHandedAnimation)
                        {
                            play.playerBodyAnimator.ResetTrigger("SwitchHoldAnimationTwoHanded");
                            play.playerBodyAnimator.SetTrigger("SwitchHoldAnimationTwoHanded");
                        }
                        play.playerBodyAnimator.ResetTrigger("SwitchHoldAnimation");
                        play.playerBodyAnimator.SetTrigger("SwitchHoldAnimation");
                    }
                    play.twoHandedAnimation = play.ItemSlots[1].itemProperties.twoHandedAnimation;
                    play.twoHanded = play.ItemSlots[1].itemProperties.twoHanded;
                    play.playerBodyAnimator.SetBool("GrabValidated", true);
                    play.playerBodyAnimator.SetBool("cancelHolding", false);
                    play.isHoldingObject = true;
                    play.currentlyHeldObjectServer = play.ItemSlots[1];
                }
                else
                {
                    if (!play.IsOwner && (UnityEngine.Object)play.heldObjectServerCopy != (UnityEngine.Object)null)
                        play.heldObjectServerCopy.SetActive(false);
                    if (play.IsOwner)
                        HUDManager.Instance.ClearControlTips();
                    play.currentlyHeldObjectServer = (GrabbableObject)null;
                    play.currentlyHeldObject = (GrabbableObject)null;
                    play.isHoldingObject = false;
                    play.twoHanded = false;
                    play.playerBodyAnimator.SetBool("cancelHolding", true);
                }

                if (!play.IsOwner)
                    return;
                if (play.twoHanded)
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 0.1f, 0.13f, 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = true;
                }
                else
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 1.5f, endAlpha: 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = false;
                }
            }

            if (BaseSlots.ItemSlotBase.InputActionsInstance.SlotThreeKey.triggered && !__instance.isPlayerDead && !__instance.isTypingChat && !__instance.inTerminalMenu && __instance == GameNetworkManager.Instance.localPlayerController)
            {
                HUDManager.Instance.itemSlotIconFrames[2].GetComponent<Animator>().SetBool("selectedSlot", true);
                __instance.currentItemSlot = 2;
                ItemSlotBase.Log.LogInfo("Changing to slot THREE");


                if (play.IsOwner)
                {
                    for (int index = 0; index < HUDManager.Instance.itemSlotIconFrames.Length; ++index)
                        HUDManager.Instance.itemSlotIconFrames[index].GetComponent<Animator>().SetBool("selectedSlot", false);
                    HUDManager.Instance.itemSlotIconFrames[2].GetComponent<Animator>().SetBool("selectedSlot", true);
                }

                if ((UnityEngine.Object)fillSlotWithItem != (UnityEngine.Object)null)
                {
                    play.ItemSlots[2] = fillSlotWithItem;
                    if (play.IsOwner)
                    {
                        HUDManager.Instance.itemSlotIcons[2].sprite = fillSlotWithItem.itemProperties.itemIcon;
                        HUDManager.Instance.itemSlotIcons[play.currentItemSlot].enabled = true;
                    }
                }

                if ((UnityEngine.Object)play.currentlyHeldObjectServer != (UnityEngine.Object)null)
                {
                    play.currentlyHeldObjectServer.playerHeldBy = play;
                    if (play.IsOwner)
                        SetSpecialGrabAnimationBool(false, play.currentlyHeldObjectServer);
                    play.currentlyHeldObjectServer.PocketItem();
                    if ((UnityEngine.Object)play.ItemSlots[2] != (UnityEngine.Object)null && !string.IsNullOrEmpty(play.ItemSlots[2].itemProperties.pocketAnim))
                        play.playerBodyAnimator.SetTrigger(play.ItemSlots[2].itemProperties.pocketAnim);
                }

                if ((UnityEngine.Object)play.ItemSlots[2] != (UnityEngine.Object)null)
                {
                    play.ItemSlots[2].playerHeldBy = play;
                    play.ItemSlots[2].EquipItem();
                    if (play.IsOwner)
                        SetSpecialGrabAnimationBool(true, play.ItemSlots[2]);
                    if ((UnityEngine.Object)play.currentlyHeldObjectServer != (UnityEngine.Object)null)
                    {
                        if (play.ItemSlots[2].itemProperties.twoHandedAnimation || play.currentlyHeldObjectServer.itemProperties.twoHandedAnimation)
                        {
                            play.playerBodyAnimator.ResetTrigger("SwitchHoldAnimationTwoHanded");
                            play.playerBodyAnimator.SetTrigger("SwitchHoldAnimationTwoHanded");
                        }
                        play.playerBodyAnimator.ResetTrigger("SwitchHoldAnimation");
                        play.playerBodyAnimator.SetTrigger("SwitchHoldAnimation");
                    }
                    play.twoHandedAnimation = play.ItemSlots[2].itemProperties.twoHandedAnimation;
                    play.twoHanded = play.ItemSlots[2].itemProperties.twoHanded;
                    play.playerBodyAnimator.SetBool("GrabValidated", true);
                    play.playerBodyAnimator.SetBool("cancelHolding", false);
                    play.isHoldingObject = true;
                    play.currentlyHeldObjectServer = play.ItemSlots[2];
                }
                else
                {
                    if (!play.IsOwner && (UnityEngine.Object)play.heldObjectServerCopy != (UnityEngine.Object)null)
                        play.heldObjectServerCopy.SetActive(false);
                    if (play.IsOwner)
                        HUDManager.Instance.ClearControlTips();
                    play.currentlyHeldObjectServer = (GrabbableObject)null;
                    play.currentlyHeldObject = (GrabbableObject)null;
                    play.isHoldingObject = false;
                    play.twoHanded = false;
                    play.playerBodyAnimator.SetBool("cancelHolding", true);
                }

                if (!play.IsOwner)
                    return;
                if (play.twoHanded)
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 0.1f, 0.13f, 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = true;
                }
                else
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 1.5f, endAlpha: 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = false;
                }
            }

            if (BaseSlots.ItemSlotBase.InputActionsInstance.SlotFourKey.triggered && !__instance.isPlayerDead && !__instance.isTypingChat && !__instance.inTerminalMenu && __instance == GameNetworkManager.Instance.localPlayerController)
            {
                HUDManager.Instance.itemSlotIconFrames[3].GetComponent<Animator>().SetBool("selectedSlot", true);
                __instance.currentItemSlot = 3;
                ItemSlotBase.Log.LogInfo("Changing to slot FOUR");


                if (play.IsOwner)
                {
                    for (int index = 0; index < HUDManager.Instance.itemSlotIconFrames.Length; ++index)
                        HUDManager.Instance.itemSlotIconFrames[index].GetComponent<Animator>().SetBool("selectedSlot", false);
                    HUDManager.Instance.itemSlotIconFrames[3].GetComponent<Animator>().SetBool("selectedSlot", true);
                }

                if ((UnityEngine.Object)fillSlotWithItem != (UnityEngine.Object)null)
                {
                    play.ItemSlots[3] = fillSlotWithItem;
                    if (play.IsOwner)
                    {
                        HUDManager.Instance.itemSlotIcons[3].sprite = fillSlotWithItem.itemProperties.itemIcon;
                        HUDManager.Instance.itemSlotIcons[play.currentItemSlot].enabled = true;
                    }
                }

                if ((UnityEngine.Object)play.currentlyHeldObjectServer != (UnityEngine.Object)null)
                {
                    play.currentlyHeldObjectServer.playerHeldBy = play;
                    if (play.IsOwner)
                        SetSpecialGrabAnimationBool(false, play.currentlyHeldObjectServer);
                    play.currentlyHeldObjectServer.PocketItem();
                    if ((UnityEngine.Object)play.ItemSlots[3] != (UnityEngine.Object)null && !string.IsNullOrEmpty(play.ItemSlots[3].itemProperties.pocketAnim))
                        play.playerBodyAnimator.SetTrigger(play.ItemSlots[3].itemProperties.pocketAnim);
                }

                if ((UnityEngine.Object)play.ItemSlots[3] != (UnityEngine.Object)null)
                {
                    play.ItemSlots[3].playerHeldBy = play;
                    play.ItemSlots[3].EquipItem();
                    if (play.IsOwner)
                        SetSpecialGrabAnimationBool(true, play.ItemSlots[3]);
                    if ((UnityEngine.Object)play.currentlyHeldObjectServer != (UnityEngine.Object)null)
                    {
                        if (play.ItemSlots[3].itemProperties.twoHandedAnimation || play.currentlyHeldObjectServer.itemProperties.twoHandedAnimation)
                        {
                            play.playerBodyAnimator.ResetTrigger("SwitchHoldAnimationTwoHanded");
                            play.playerBodyAnimator.SetTrigger("SwitchHoldAnimationTwoHanded");
                        }
                        play.playerBodyAnimator.ResetTrigger("SwitchHoldAnimation");
                        play.playerBodyAnimator.SetTrigger("SwitchHoldAnimation");
                    }
                    play.twoHandedAnimation = play.ItemSlots[3].itemProperties.twoHandedAnimation;
                    play.twoHanded = play.ItemSlots[3].itemProperties.twoHanded;
                    play.playerBodyAnimator.SetBool("GrabValidated", true);
                    play.playerBodyAnimator.SetBool("cancelHolding", false);
                    play.isHoldingObject = true;
                    play.currentlyHeldObjectServer = play.ItemSlots[3];
                }
                else
                {
                    if (!play.IsOwner && (UnityEngine.Object)play.heldObjectServerCopy != (UnityEngine.Object)null)
                        play.heldObjectServerCopy.SetActive(false);
                    if (play.IsOwner)
                        HUDManager.Instance.ClearControlTips();
                    play.currentlyHeldObjectServer = (GrabbableObject)null;
                    play.currentlyHeldObject = (GrabbableObject)null;
                    play.isHoldingObject = false;
                    play.twoHanded = false;
                    play.playerBodyAnimator.SetBool("cancelHolding", true);
                }

                if (!play.IsOwner)
                    return;
                if (play.twoHanded)
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 0.1f, 0.13f, 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = true;
                }
                else
                {
                    HUDManager.Instance.PingHUDElement(HUDManager.Instance.Inventory, 1.5f, endAlpha: 0.13f);
                    HUDManager.Instance.holdingTwoHandedItem.enabled = false;
                }
            }
        }

        private static void SetSpecialGrabAnimationBool(bool setTrue, GrabbableObject currentItem = null)
        {
            if ((UnityEngine.Object)currentItem == (UnityEngine.Object)null)
                currentItem = currentItem;
            if (!play.IsOwner)
                return;
            play.playerBodyAnimator.SetBool("Grab", setTrue);
            if (string.IsNullOrEmpty(currentItem.itemProperties.grabAnim))
                return;
            try
            {
                play.playerBodyAnimator.SetBool(currentItem.itemProperties.grabAnim, setTrue);
            }
            catch (Exception ex)
            {
                Debug.LogError((object)("An item tried to set an animator bool which does not exist: " + currentItem.itemProperties.grabAnim));
            }
        }

    }

    
}
