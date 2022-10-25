// Animancer // https://kybernetik.com.au/animancer // Copyright 2022 Kybernetik //

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System;

#if UNITY_2D_SPRITE
using UnityEditor.U2D.Sprites;
#endif

namespace Animancer.Editor.Tools
{
    /// <summary>A wrapper around the '2D Sprite' package features for editing Sprite data.</summary>
    public class SpriteDataEditor
    {
        /************************************************************************************************************************/

        /// <summary>Is the '2D Sprite' package currently in the project?</summary>
        public static bool Is2dPackagePresent;

        static SpriteDataEditor()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name == "Unity.2D.Sprite.Editor")
                {
                    Is2dPackagePresent = true;
                    break;
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>The scripting define symbol used to enable the features from the '2D Sprite' package.</summary>
        public const string SpritePackageDefineSymbol = "UNITY_2D_SPRITE";

        /// <summary>Is the <see cref="SpritePackageDefineSymbol"/> defined in the current platform?</summary>
        public static bool IsSpritePackageSymbolDefined
        {
            get => ScriptingDefineSymbols.IsSymbolDefined(SpritePackageDefineSymbol);
            set => ScriptingDefineSymbols.SetSymbolDefined(SpritePackageDefineSymbol, value);
        }

        /************************************************************************************************************************/
#if UNITY_2D_SPRITE
        /************************************************************************************************************************/

        private static SpriteDataProviderFactories _Factories;

        private static SpriteDataProviderFactories Factories
        {
            get
            {
                if (_Factories == null)
                {
                    _Factories = new SpriteDataProviderFactories();
                    _Factories.Init();
                }

                return _Factories;
            }
        }

        /************************************************************************************************************************/

        private readonly ISpriteEditorDataProvider Provider;
        private readonly SpriteRect[] SpriteRects;

        /************************************************************************************************************************/

        /// <summary>The number of sprites in the target data.</summary>
        public int SpriteCount => SpriteRects.Length;

        /// <summary>Returns the name of the sprite at the specified `index`.</summary>
        public string GetName(int index) => SpriteRects[index].name;

        /// <summary>Sets the name of the sprite at the specified `index`.</summary>
        public void SetName(int index, string name) => SpriteRects[index].name = name;

        /// <summary>Returns the rect of the sprite at the specified `index`.</summary>
        public Rect GetRect(int index) => SpriteRects[index].rect;

        /// <summary>Sets the rect of the sprite at the specified `index`.</summary>
        public void SetRect(int index, Rect rect) => SpriteRects[index].rect = rect;

        /// <summary>Returns the pivot of the sprite at the specified `index`.</summary>
        public Vector2 GetPivot(int index) => SpriteRects[index].pivot;

        /// <summary>Sets the pivot of the sprite at the specified `index`.</summary>
        public void SetPivot(int index, Vector2 pivot) => SpriteRects[index].pivot = pivot;

        /// <summary>Returns the alignment of the sprite at the specified `index`.</summary>
        public SpriteAlignment GetAlignment(int index) => SpriteRects[index].alignment;

        /// <summary>Sets the alignment of the sprite at the specified `index`.</summary>
        public void SetAlignment(int index, SpriteAlignment alignment) => SpriteRects[index].alignment = alignment;

        /// <summary>Returns the border of the sprite at the specified `index`.</summary>
        public Vector4 GetBorder(int index) => SpriteRects[index].border;

        /// <summary>Sets the border of the sprite at the specified `index`.</summary>
        public void SetBorder(int index, Vector4 border) => SpriteRects[index].border = border;

        /************************************************************************************************************************/
#else
        /************************************************************************************************************************/

        private readonly SpriteMetaData[] SpriteSheet;

        /************************************************************************************************************************/

        /// <summary>The number of sprites in the target data.</summary>
        public int SpriteCount => SpriteSheet.Length;

        /// <summary>Returns the name of the sprite at the specified `index`.</summary>
        public string GetName(int index) => SpriteSheet[index].name;

        /// <summary>Sets the name of the sprite at the specified `index`.</summary>
        public void SetName(int index, string name) => SpriteSheet[index].name = name;

        /// <summary>Returns the rect of the sprite at the specified `index`.</summary>
        public Rect GetRect(int index) => SpriteSheet[index].rect;

        /// <summary>Sets the rect of the sprite at the specified `index`.</summary>
        public void SetRect(int index, Rect rect) => SpriteSheet[index].rect = rect;

        /// <summary>Returns the pivot of the sprite at the specified `index`.</summary>
        public Vector2 GetPivot(int index) => SpriteSheet[index].pivot;

        /// <summary>Sets the pivot of the sprite at the specified `index`.</summary>
        public void SetPivot(int index, Vector2 pivot) => SpriteSheet[index].pivot = pivot;

        /// <summary>Returns the alignment of the sprite at the specified `index`.</summary>
        public SpriteAlignment GetAlignment(int index) => (SpriteAlignment)SpriteSheet[index].alignment;

        /// <summary>Sets the alignment of the sprite at the specified `index`.</summary>
        public void SetAlignment(int index, SpriteAlignment alignment) => SpriteSheet[index].alignment = (int)alignment;

        /// <summary>Returns the border of the sprite at the specified `index`.</summary>
        public Vector4 GetBorder(int index) => SpriteSheet[index].border;

        /// <summary>Sets the border of the sprite at the specified `index`.</summary>
        public void SetBorder(int index, Vector4 border) => SpriteSheet[index].border = border;

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/

        private readonly TextureImporter Importer;

        /************************************************************************************************************************/

        /// <summary>Creates a new <see cref="SpriteDataEditor"/>.</summary>
        public SpriteDataEditor(TextureImporter importer)
        {
            Importer = importer;

#if UNITY_2D_SPRITE
            Provider = Factories.GetSpriteEditorDataProviderFromObject(importer);
            Provider.InitSpriteEditorDataProvider();

            SpriteRects = Provider.GetSpriteRects();
#else
            SpriteSheet = importer.spritesheet;
#endif
        }

        /************************************************************************************************************************/

        /// <summary>Tries to find the index of the data matching the `sprite`.</summary>
        /// <remarks>
        /// Returns -1 if there is no data matching the <see cref="UnityEngine.Object.name"/>.
        /// <para></para>
        /// Returns -2 if there is more than one data matching the <see cref="UnityEngine.Object.name"/> but no
        /// <see cref="Sprite.rect"/> match.
        /// </remarks>
        public int IndexOf(Sprite sprite)
        {
            var nameMatchIndex = -1;

            var count = SpriteCount;
            for (int i = 0; i < count; i++)
            {
                if (GetName(i) == sprite.name)
                {
                    if (GetRect(i) == sprite.rect)
                        return i;

                    if (nameMatchIndex == -1)// First name match.
                        nameMatchIndex = i;
                    else
                        nameMatchIndex = -2;// Already found 2 name matches.
                }
            }

            if (nameMatchIndex == -1)
            {
                Debug.LogError($"No {nameof(SpriteMetaData)} for '{sprite.name}' was found.", sprite);
            }
            else if (nameMatchIndex == -2)
            {
                Debug.LogError($"More than one {nameof(SpriteMetaData)} for '{sprite.name}' was found" +
                    $" but none of them matched the {nameof(Sprite)}.{nameof(Sprite.rect)}." +
                    $" If the texture's Max Size is smaller than its actual size, increase the Max Size before performing this" +
                    $" operation so that the {nameof(Rect)}s can be used to identify the correct data.", sprite);
            }

            return nameMatchIndex;
        }

        /************************************************************************************************************************/

        /// <summary>Logs an error and returns false if the data at the specified `index` is out of the texture bounds.</summary>
        public bool ValidateBounds(int index, Sprite sprite)
        {
            var rect = GetRect(index);
            var widthScale = rect.width / sprite.rect.width;
            var heightScale = rect.height / sprite.rect.height;
            if (rect.xMin < 0 ||
                rect.yMin < 0 ||
                rect.xMax > sprite.texture.width * widthScale ||
                rect.yMax > sprite.texture.height * heightScale)
            {
                var path = AssetDatabase.GetAssetPath(sprite);
                Debug.LogError($"This modification would have put '{sprite.name}' out of bounds" +
                    $" so '{path}' was not modified.", sprite);

                return false;
            }

            return true;
        }

        /************************************************************************************************************************/

        /// <summary>Applies any modifications to the target asset.</summary>
        public void Apply()
        {
#if UNITY_2D_SPRITE
            Provider.SetSpriteRects(SpriteRects);
            Provider.Apply();
#else
            Importer.spritesheet = SpriteSheet;
            EditorUtility.SetDirty(Importer);
#endif

            Importer.SaveAndReimport();
        }

        /************************************************************************************************************************/
    }
}

#endif

