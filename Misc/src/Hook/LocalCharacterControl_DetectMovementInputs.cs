using HarmonyLib;
using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Misc.Hook
{
    [HarmonyPatch(typeof(LocalCharacterControl), nameof(LocalCharacterControl.DetectMovementInputs))]
    class LocalCharacterControl_DetectMovementInputs
    {

        static void Postfix(LocalCharacterControl __instance)
        {
            var characterControl = __instance;
            var character = characterControl.Character;
            if (characterControl.InputLocked || character.CharacterUI.ChatPanel.IsChatFocused)
            {
                return;
            }
            
            if (Misc.Sprint)
            {
                characterControl.m_sprintTime += Time.deltaTime;
                if (characterControl.m_character.Sneaking)
                {
                    characterControl.m_character.StealthInput(false);
                }
                if (characterControl.m_character.CurrentlyChargingAttack && !characterControl.m_character.CancelChargingSent)
                {
                    characterControl.m_character.CancelCharging();
                }
                characterControl.m_modifMoveInput.Normalize();
                characterControl.m_modifMoveInput *= characterControl.m_character.Speed * 1.75f * (float)((!characterControl.m_character.IsSuperSpeedActive) ? 1 : 4);
                characterControl.m_sprintFacing = true;
                characterControl.m_character.SprintInput(true);
                if (characterControl.m_character.BlockDesired)
                {
                    characterControl.m_character.BlockInput(false);
                }
            }

        }

    }
}
