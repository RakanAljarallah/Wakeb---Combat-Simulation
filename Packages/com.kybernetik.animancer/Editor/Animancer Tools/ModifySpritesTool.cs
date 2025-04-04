// Animancer // https://kybernetik.com.au/animancer // Copyright 2018-2025 Kybernetik //

#if UNITY_EDITOR

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using UnityEditor;
using UnityEngine;

namespace Animancer.Editor.Tools
{
    /// <summary>[Editor-Only] [Pro-Only] 
    /// A <see cref="SpriteModifierTool"/> for modifying <see cref="Sprite"/> detauls.
    /// </summary>
    /// <remarks>
    /// <strong>Documentation:</strong>
    /// <see href="https://kybernetik.com.au/animancer/docs/manual/tools/modify-sprites">
    /// Modify Sprites</see>
    /// </remarks>
    /// https://kybernetik.com.au/animancer/api/Animancer.Editor.Tools/ModifySpritesTool
    /// 
    [Serializable]
    public class ModifySpritesTool : SpriteModifierTool
    {
        /************************************************************************************************************************/

        [SerializeField] private OffsetRectMode _RectMode;
        [SerializeField] private Rect _RectOffset;

        [SerializeField] private bool _SetPivot;
        [SerializeField] private Vector2 _Pivot;

        [SerializeField] private bool _SetAlignment;
        [SerializeField] private SpriteAlignment _Alignment;

        [SerializeField] private bool _SetBorder;
        [SerializeField] private RectOffset _Border;

        [SerializeField] private bool _ShowDetails;

        /************************************************************************************************************************/

        private enum OffsetRectMode { None, Add, Subtract }
        private static readonly string[] OffsetRectModes = { "None", "Add", "Subtract" };

        private SerializedProperty _SerializedProperty;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override int DisplayOrder => 1;

        /// <inheritdoc/>
        public override string Name => "Modify Sprites";

        /// <inheritdoc/>
        public override string HelpURL => Strings.DocsURLs.ModifySprites;

        /// <inheritdoc/>
        public override string Instructions
        {
            get
            {
                if (Sprites.Count == 0)
                    return "Select the Sprites you want to modify.";

                if (!IsValidModification())
                    return "The current Rect Offset would move some Sprites outside the texture bounds.";

                return "Enter the desired modifications and click Apply.";
            }
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override void OnEnable(int index)
        {
            base.OnEnable(index);

            _SerializedProperty = AnimancerToolsWindow.Instance.FindSerializedPropertyForTool(this);
            _SerializedProperty = _SerializedProperty.FindPropertyRelative(nameof(_RectMode));
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override void DoBodyGUI()
        {
            base.DoBodyGUI();

            var area = AnimancerGUI.LayoutSingleLineRect();
            area.xMin += 4;

            using (var label = PooledGUIContent.Acquire("Offset Rects", null))
                area = EditorGUI.PrefixLabel(area, label);

            AnimancerToolsWindow.BeginChangeCheck();
            var selected = (OffsetRectMode)GUI.Toolbar(area, (int)_RectMode, OffsetRectModes);
            AnimancerToolsWindow.EndChangeCheck(ref _RectMode, selected);

            using (var property = _SerializedProperty.Copy())
            {
                property.serializedObject.Update();

                var depth = property.depth;
                while (property.Next(false) && property.depth >= depth)
                {
                    EditorGUILayout.PropertyField(property, true);
                }

                property.serializedObject.ApplyModifiedProperties();
            }

            GUI.enabled = false;
            for (int i = 0; i < Sprites.Count; i++)
            {
                if (_ShowDetails)
                    GUILayout.BeginVertical(GUI.skin.box);

                var sprite = Sprites[i] = AnimancerGUI.DoObjectFieldGUI("", Sprites[i], false);

                if (_ShowDetails)
                {
                    if (_RectMode != OffsetRectMode.None)
                        EditorGUILayout.RectField("Rect", sprite.rect);

                    if (_SetPivot)
                        EditorGUILayout.Vector2Field("Pivot", sprite.pivot);

                    if (_SetBorder)
                        EditorGUILayout.Vector4Field("Border", sprite.border);

                    GUILayout.EndVertical();
                }
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                GUI.enabled = Sprites.Count > 0 && IsValidModification();

                if (GUILayout.Button("Apply"))
                {
                    AnimancerGUI.Deselect();
                    AskAndApply();
                }
            }
            GUILayout.EndHorizontal();
        }

        /************************************************************************************************************************/

        private bool IsValidModification()
        {
            switch (_RectMode)
            {
                default:
                case OffsetRectMode.None:
                    return true;

                case OffsetRectMode.Add:
                case OffsetRectMode.Subtract:
                    break;
            }

            var offset = GetOffset();

            var sprites = Sprites;
            for (int i = 0; i < sprites.Count; i++)
            {
                var sprite = sprites[i];
                var rect = Add(sprite.rect, offset);
                if (rect.xMin < 0 ||
                    rect.yMin < 0 ||
                    rect.xMax >= sprite.texture.width ||
                    rect.xMax >= sprite.texture.height)
                {
                    return false;
                }
            }

            return true;
        }

        /************************************************************************************************************************/

        private Rect GetOffset()
        {
            return _RectMode switch
            {
                OffsetRectMode.Add
                    => _RectOffset,
                OffsetRectMode.Subtract
                    => new(-_RectOffset.x, -_RectOffset.y, -_RectOffset.width, -_RectOffset.height),
                _
                    => throw new InvalidOperationException($"Can't {nameof(GetOffset)} when the mode is {_RectMode}."),
            };
        }

        private static Rect Add(Rect a, Rect b)
        {
            a.x += b.x;
            a.y += b.y;
            a.width += b.width;
            a.height += b.height;
            return a;
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        protected override string AreYouSure => "Are you sure you want to modify the borders of these Sprites?";

        /************************************************************************************************************************/

        /// <inheritdoc/>
        protected override void Modify(SpriteDataEditor data, int index, Sprite sprite)
        {
            switch (_RectMode)
            {
                default:
                case OffsetRectMode.None:
                    break;

                case OffsetRectMode.Add:
                case OffsetRectMode.Subtract:
                    var rect = data.GetRect(index);
                    rect = Add(rect, GetOffset());
                    data.SetRect(index, rect);
                    break;
            }

            if (_SetPivot)
                data.SetPivot(index, _Pivot);

            if (_SetAlignment)
                data.SetAlignment(index, _Alignment);

            if (_SetBorder)
                data.SetBorder(index, new(_Border.left, _Border.bottom, _Border.right, _Border.top));
        }

        /************************************************************************************************************************/
    }
}

#endif

