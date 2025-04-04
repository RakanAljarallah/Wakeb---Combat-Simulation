﻿using Gaia.Internal;
using PWCommon5;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Gaia
{
    [CustomEditor(typeof(TerrainLoader))]
    public class TerrainLoaderEditor : PWEditor, IPWEditor
    {
        private TerrainLoader m_gaiaTerrainLoader;
        private EditorUtils m_editorUtils;

        public void OnSceneGUI()
        {
            //Do not perform any loading / unloading actions while the mouse key is down
            if (Event.current.isMouse)
            {
                if (Event.current.button == 0 && Event.current.type == EventType.MouseDrag)
                {
                    m_gaiaTerrainLoader.m_beingDragged = true;
                }
                else
                {
                    m_gaiaTerrainLoader.m_beingDragged = false;
                }
            }
        }

        public void OnEnable()
        {
            m_gaiaTerrainLoader = (TerrainLoader)target;
            m_gaiaTerrainLoader.m_isSelected = true;
            //Init editor utils
            if (m_editorUtils == null)
            {
                // Get editor utils for this
                m_editorUtils = PWApp.GetEditorUtils(this);
            }
        }

        public void OnDisable()
        {
            if (m_gaiaTerrainLoader != null)
            {
                m_gaiaTerrainLoader.m_isSelected = false;
                m_gaiaTerrainLoader.UpdateTerrains();
            }
        }

        public override void OnInspectorGUI()
        {
            m_editorUtils.Initialize();
            m_editorUtils.Panel("TerrainLoader", TerrainLoader, false, true, true);
            //DrawDefaultInspector();


        }

        private void TerrainLoader(bool helpEnabled)
        {
            bool currentGUIState = GUI.enabled;
            if (!TerrainLoaderManager.Instance.TerrainSceneStorage.m_terrainLoadingEnabled)
            {
                EditorGUILayout.HelpBox(m_editorUtils.GetTextValue("AutoLoadTerrainsDisabled"), MessageType.Warning);
                GUI.enabled = false;
            }

            if (TerrainLoaderManager.ColliderOnlyLoadingActive)
            {
                EditorGUILayout.HelpBox(m_editorUtils.GetTextValue("ColliderOnlyLoadingWarning"), MessageType.Warning);
            }

            EditorGUI.BeginChangeCheck();

            m_gaiaTerrainLoader.LoadMode = (LoadMode)m_editorUtils.EnumPopup("LoadMode", m_gaiaTerrainLoader.LoadMode, helpEnabled);
            if (m_gaiaTerrainLoader.LoadMode == LoadMode.RuntimeAlways)
            {
                m_gaiaTerrainLoader.m_frustumCamera = (Camera)m_editorUtils.ObjectField("FrustumCamera", m_gaiaTerrainLoader.m_frustumCamera, typeof(Camera), true, helpEnabled);
            }

            if (m_gaiaTerrainLoader.LoadMode == LoadMode.RuntimeAlways && !TerrainLoaderManager.Instance.m_assumeGridLayout)
            {
                EditorGUI.indentLevel++;
                m_editorUtils.InlineHelp("RefreshRates", helpEnabled);
                m_gaiaTerrainLoader.m_minRefreshDistance = m_editorUtils.FloatField("MinRefreshDistance", m_gaiaTerrainLoader.m_minRefreshDistance, helpEnabled);
                m_gaiaTerrainLoader.m_maxRefreshDistance = m_editorUtils.FloatField("MaxRefreshDistance", m_gaiaTerrainLoader.m_maxRefreshDistance, helpEnabled);
                m_gaiaTerrainLoader.m_minRefreshMS = m_editorUtils.FloatField("MinRefreshMS", m_gaiaTerrainLoader.m_minRefreshMS, helpEnabled);
                m_gaiaTerrainLoader.m_maxRefreshMS = m_editorUtils.FloatField("MaxRefreshMS", m_gaiaTerrainLoader.m_maxRefreshMS, helpEnabled);
                EditorGUI.indentLevel--;
            }
            m_editorUtils.Heading("LoadRange");
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            m_gaiaTerrainLoader.m_followTransform = m_editorUtils.Toggle("FollowTransform", m_gaiaTerrainLoader.m_followTransform, helpEnabled);
            if (TerrainLoaderManager.ColliderOnlyLoadingActive)
            {
                if (!m_gaiaTerrainLoader.m_followTransform)
                {
                    m_gaiaTerrainLoader.m_loadingBoundsCollider.center = m_editorUtils.Vector3Field("LoadingBoundsColliderCenter", m_gaiaTerrainLoader.m_loadingBoundsCollider.center, helpEnabled);
                }
                m_gaiaTerrainLoader.m_loadingBoundsCollider.extents = m_editorUtils.Vector3Field("LoadingBoundsColliderExtents", m_gaiaTerrainLoader.m_loadingBoundsCollider.extents, helpEnabled);
            }
            else
            {
                if (!m_gaiaTerrainLoader.m_followTransform)
                {
                    m_gaiaTerrainLoader.m_loadingBoundsRegular.center = m_editorUtils.Vector3Field("LoadingBoundsCenter", m_gaiaTerrainLoader.m_loadingBoundsRegular.center, helpEnabled);
                }
                m_gaiaTerrainLoader.m_loadingBoundsRegular.extents = m_editorUtils.Vector3Field("LoadingBoundsExtents", m_gaiaTerrainLoader.m_loadingBoundsRegular.extents, helpEnabled);
                if (!m_gaiaTerrainLoader.m_followTransform)
                {
                    m_gaiaTerrainLoader.m_loadingBoundsImpostor.center = m_editorUtils.Vector3Field("LoadingBoundsImpostorCenter", m_gaiaTerrainLoader.m_loadingBoundsImpostor.center, helpEnabled);
                }
                m_gaiaTerrainLoader.m_loadingBoundsImpostor.extents = m_editorUtils.Vector3Field("LoadingBoundsImpostorExtents", m_gaiaTerrainLoader.m_loadingBoundsImpostor.extents, helpEnabled);
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (m_gaiaTerrainLoader.LoadMode == LoadMode.RuntimeAlways && Application.isPlaying)
                {
                    m_gaiaTerrainLoader.CalculcateNextUpdateBounds();
                }
            }

            EditorGUI.indentLevel--;

            m_editorUtils.Panel("Gizmos", DrawGizmos, false, false, false);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(m_gaiaTerrainLoader); 
            }

            m_gaiaTerrainLoader.UpdateTerrains();
            GUI.enabled = currentGUIState;
        }

        private void DrawGizmos(bool helpEnabled)
        {
            EditorGUI.indentLevel++;
            m_gaiaTerrainLoader.m_drawLoadRanges = m_editorUtils.Toggle("DrawLoadRanges", m_gaiaTerrainLoader.m_drawLoadRanges, helpEnabled);
            bool originalGUIState = GUI.enabled;
            if (!m_gaiaTerrainLoader.m_drawLoadRanges)
            {
                GUI.enabled = false;
            }
            EditorGUI.indentLevel++;
            m_gaiaTerrainLoader.m_regularLoadRangeActiveColor = m_editorUtils.ColorField("RegularLoadRangeActiveColor", m_gaiaTerrainLoader.m_regularLoadRangeActiveColor, helpEnabled);
            m_gaiaTerrainLoader.m_regularLoadRangeInActiveColor = m_editorUtils.ColorField("RegularLoadRangeInActiveColor", m_gaiaTerrainLoader.m_regularLoadRangeInActiveColor, helpEnabled);
            m_gaiaTerrainLoader.m_impostorLoadRangeActiveColor = m_editorUtils.ColorField("ImpostorLoadRangeActiveColor", m_gaiaTerrainLoader.m_impostorLoadRangeActiveColor, helpEnabled);
            m_gaiaTerrainLoader.m_impostorLoadRangeInActiveColor = m_editorUtils.ColorField("ImpostorLoadRangeInActiveColor", m_gaiaTerrainLoader.m_impostorLoadRangeInActiveColor, helpEnabled);
            EditorGUI.indentLevel--;
            GUI.enabled = originalGUIState;
            m_gaiaTerrainLoader.m_drawUpdateRanges = m_editorUtils.Toggle("DrawUpdateRanges", m_gaiaTerrainLoader.m_drawUpdateRanges, helpEnabled);
            if (!m_gaiaTerrainLoader.m_drawUpdateRanges)
            {
                GUI.enabled = false;
            }
            EditorGUI.indentLevel++;
            m_gaiaTerrainLoader.m_updateBoundsRegularColor = m_editorUtils.ColorField("UpdateBoundsRegularColor", m_gaiaTerrainLoader.m_updateBoundsRegularColor, helpEnabled);
            m_gaiaTerrainLoader.m_updateBoundsImpostorColor = m_editorUtils.ColorField("UpdateBoundsImpostorColor", m_gaiaTerrainLoader.m_updateBoundsImpostorColor, helpEnabled);
            EditorGUI.indentLevel--;
            GUI.enabled = originalGUIState;
            EditorGUI.indentLevel--;
        }
    }
}