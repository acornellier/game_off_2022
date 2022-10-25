// Animancer // https://kybernetik.com.au/animancer // Copyright 2022 Kybernetik //

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// Various utility functions for accessing scripting define symbols.
    /// </summary>
    /// https://kybernetik.com.au/animancer/api/Animancer.Editor/ScriptingDefineSymbols
    /// 
    public static class ScriptingDefineSymbols
    {
        /************************************************************************************************************************/

        /// <summary>The platform currently selected in the Player Settings.</summary>
#if UNITY_2021_2_OR_NEWER
        private static NamedBuildTarget CurrentBuildTarget
        {
            get
            {
                var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
                return NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
            }
        }
#else
        private static BuildTargetGroup CurrentBuildTarget
            => EditorUserBuildSettings.selectedBuildTargetGroup;
#endif

        /************************************************************************************************************************/

        /// <summary>The symbols defined in the <see cref="CurrentBuildTarget"/>.</summary>
        public static string[] Symbols
        {
            get
            {
                var currentBuildTarget = CurrentBuildTarget;
#if UNITY_2021_2_OR_NEWER
                PlayerSettings.GetScriptingDefineSymbols(currentBuildTarget, out var symbols);
                return symbols;
#else
                var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentBuildTarget);
                return symbols.Split(';');
#endif
            }
            set
            {
                var currentBuildTarget = CurrentBuildTarget;
#if UNITY_2021_2_OR_NEWER
                PlayerSettings.SetScriptingDefineSymbols(currentBuildTarget, value);
#else
                var joined = string.Join(";", value);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentBuildTarget, joined);
#endif
            }
        }

        /************************************************************************************************************************/

        /// <summary>Is the `symbol` defined in the <see cref="CurrentBuildTarget"/>.</summary>
        public static bool IsSymbolDefined(string symbol)
        {
            return Array.IndexOf(Symbols, symbol) >= 0;
        }

        /************************************************************************************************************************/

        /// <summary>Defines or undefines the `symbol` in the <see cref="CurrentBuildTarget"/>.</summary>
        public static void SetSymbolDefined(string symbol, bool isDefined)
        {
            var symbols = Symbols;
            var index = Array.IndexOf(symbols, symbol);

            if (isDefined)
            {
                if (index >= 0)
                    return;

                var newSymbols = new List<string>();
                newSymbols.AddRange(symbols);
                newSymbols.Add(symbol);
                symbols = newSymbols.ToArray();
            }
            else
            {
                if (index < 0)
                    return;

                var newSymbols = new List<string>(symbols);
                newSymbols.RemoveAt(index);
                symbols = newSymbols.ToArray();
            }

            Symbols = symbols;
        }

        /************************************************************************************************************************/
    }
}

#endif

