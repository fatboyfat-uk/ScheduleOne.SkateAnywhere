/*
    ScheduleOne Skate Anywhere Mod
    Copyright (C) 2025 fatboyfat_uk (email github@fatboyfat.uk)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using HarmonyLib;
using ScheduleOne.DevUtilities;
using ScheduleOne.Skating;
using UnityEngine;

namespace ScheduleOne.SkateAnywhere.Mono
{
    internal class SkateboardSpeedo
    {
        private static Skateboard _currentSkateboard = null;
        private static GUIStyle _displayStyle;

        private const int _speedoWidth = 200;
        private const int _speedoHeight = 50;

        private static readonly Rect _labelPos = new Rect(Screen.width - _speedoWidth - 200, Screen.height - 250, _speedoWidth, _speedoHeight);

        public static void OnGUI()
        {
            _displayStyle ??= new GUIStyle("label")
                {
                    fontSize = 36,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleRight,
                    normal = new GUIStyleState
                    {
                        textColor = Color.white,
                        background = new Texture2D(_speedoWidth, _speedoHeight, TextureFormat.RGBA32, false)
                    }
                };

            if (_currentSkateboard != null)
            {
                GUI.Label(_labelPos, CalculateSpeed(), _displayStyle);
            }
        }

        private static string CalculateSpeed()
        {
            var unitConversionFactor =  Settings.Instance.unitType == Settings.UnitType.Metric ? 3.6f : 2.23694f;
            return Mathf.Abs(_currentSkateboard.VelocityCalculator.Velocity.magnitude * unitConversionFactor * 1.4f).ToString("0") + 
                (Settings.Instance.unitType == Settings.UnitType.Metric ? " km/h" : " mph");
        }

            [HarmonyPatch(typeof(Skateboard_Equippable), nameof(Skateboard_Equippable.Mount))]
        static class SkateboardEquippableMountPatch
        {
            static void Postfix(Skateboard_Equippable __instance)
            {
                _currentSkateboard = __instance.IsRiding ? __instance.ActiveSkateboard : null;
            }
        }

        [HarmonyPatch(typeof(Skateboard_Equippable), nameof(Skateboard_Equippable.Dismount))]
        static class SkateboardEquippableDismountPatch
        {
            static void Postfix(Skateboard_Equippable __instance)
            {
                _currentSkateboard = __instance.IsRiding ? __instance.ActiveSkateboard : null;
            }
        }
    }
}
