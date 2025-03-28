﻿using PWCommon5;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Gaia
{
    //This class is not a full editor class by itself, but used to collect reusable methods
    //for editing collision masks in a reorderable list.
    public class CollisionMaskListEditor : PWEditor, IPWEditor
    {
        public static float OnElementHeight(int index, CollisionMask[] collisionMasks)
        {
            if (collisionMasks[index].m_type == BakedMaskType.RadiusTree)
            {
                if (CollisionMask.m_allTreeSpawnRuleNames.Length == 0)
                {
                    return EditorGUIUtility.singleLineHeight;
                }
                else
                {
                    return Mathf.CeilToInt((float)CollisionMask.m_allTreeSpawnRuleNames.Length / 32f) * EditorGUIUtility.singleLineHeight;
                }
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

        public static CollisionMask[] OnRemoveMaskListEntry(CollisionMask[] oldList, int index)
        {
            if (index < 0 || index >= oldList.Length)
                return null;
            CollisionMask toRemove = oldList[index];
            CollisionMask[] newList = new CollisionMask[oldList.Length - 1];
            for (int i = 0; i < newList.Length; ++i)
            {
                if (i < index)
                {
                    newList[i] = oldList[i];
                }
                else if (i >= index)
                {
                    newList[i] = oldList[i + 1];
                }
            }
            return newList;
        }

        public static CollisionMask[] OnAddMaskListEntry(CollisionMask[] oldList)
        {
            CollisionMask[] newList = new CollisionMask[oldList.Length + 1];
            for (int i = 0; i < oldList.Length; ++i)
            {
                newList[i] = oldList[i];
            }
            newList[newList.Length - 1] = new CollisionMask();
            return newList;
        }

        public static bool DrawFilterListHeader(Rect rect, bool currentFoldOutState, CollisionMask[] maskList, EditorUtils editorUtils)
        {
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            rect.xMin += 8f;
            bool newFoldOutState = EditorGUI.Foldout(rect, currentFoldOutState, PropertyCount("MaskRespectedCollisions", maskList, editorUtils), true);
            EditorGUI.indentLevel = oldIndent;
            return newFoldOutState;
        }

        public static void DrawMaskList(ref bool listExpanded, ReorderableList list, EditorUtils editorUtils, Rect rect)
        {
            if (list == null)
            {
                return;
            }

            Rect maskRect;
            if (listExpanded)
            {
                maskRect = rect;
                list.DoList(maskRect);
            }
            else
            {
                int oldIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 1;
                listExpanded = EditorGUI.Foldout(rect,listExpanded, PropertyCount("MaskRespectedCollisions", (CollisionMask[])list.list, editorUtils), true);
                maskRect = rect;
                EditorGUI.indentLevel = oldIndent;
            }

            //editorUtils.Panel("MaskBaking", DrawMaskBaking, false);
        }

        public static void DrawMaskListElement(Rect rect, int index, CollisionMask collisionMask, EditorUtils m_editorUtils, Terrain currentTerrain, GaiaConstants.FeatureOperation operation)
        {
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            //Active Label
            Rect fieldRect = new Rect(rect.x, rect.y, rect.width * 0.1f, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(fieldRect, m_editorUtils.GetContent("MaskActive"));
            //Active Checkbox
            fieldRect.x += rect.width * 0.1f;
            fieldRect.width = rect.width * 0.05f;
            collisionMask.m_active = EditorGUI.Toggle(fieldRect, collisionMask.m_active);
            //Invert Label
            fieldRect.x += rect.width * 0.05f;
            fieldRect.width = rect.width * 0.1f;
            EditorGUI.LabelField(fieldRect, m_editorUtils.GetContent("MaskInvert"));
            //Invert Checkbox
            fieldRect.x += rect.width * 0.1f;
            fieldRect.width = rect.width * 0.05f;
            collisionMask.m_invert = EditorGUI.Toggle(fieldRect, collisionMask.m_invert);
            //Type dropdown
            fieldRect.x += rect.width * 0.05f;
            fieldRect.width = rect.width * 0.2f;
            BakedMaskType oldType = collisionMask.m_type;
            collisionMask.m_type = (BakedMaskType)EditorGUI.EnumPopup(fieldRect, collisionMask.m_type);
            EditorGUI.LabelField(fieldRect, m_editorUtils.GetContent("CollisionMaskTypeTooltip"));
            switch (collisionMask.m_type)
            {
                case BakedMaskType.RadiusTree:
                    //Tree dropdown
                    List<int> oldTreeMaskValues = new List<int>(collisionMask.RadiusTreeMaskValues);
                    fieldRect.x += rect.width * 0.2f;
                    //using this local variable to prevent unnecessary conversions between mask int values and the stored tree spawn rule GUID list
                    List<int> currentTreeMaskValue = collisionMask.RadiusTreeMaskValues;
                    //if (collisionMask.m_selectedTreeSpawnRuleGUIDs != null && collisionMask.m_selectedTreeSpawnRuleGUIDs.Count>0)
                    //{
                    //    selectedGUIDIndices = CollisionMask.GetRadiusTreeMaskValue();
                    //}
                    if (CollisionMask.m_allTreeSpawnRuleNames != null && CollisionMask.m_allTreeSpawnRuleNames.Length > 0)
                    {
                        float originalY = fieldRect.y;
                        for (int i = 0; i < Mathf.CeilToInt((float)CollisionMask.m_allTreeSpawnRuleNames.Length/32f); i++)
                        {
                            if(currentTreeMaskValue.Count<i+1)
                            {
                                currentTreeMaskValue.Add(0);
                            }
                            int numberOfNames = Mathf.Min(32, CollisionMask.m_allTreeSpawnRuleNames.Length - (32 * i));
                            string[] ruleNamesToDisplay = new string[numberOfNames];
                            for (int k = 0; k < numberOfNames; k++)
                            {
                                ruleNamesToDisplay[k] = CollisionMask.m_allTreeSpawnRuleNames[i * 32 + k];

                            }
                            currentTreeMaskValue[i] = EditorGUI.MaskField(fieldRect, currentTreeMaskValue[i], ruleNamesToDisplay);
                            fieldRect.y+=EditorGUIUtility.singleLineHeight;
                        }
                        fieldRect.y = originalY;
                    }
                    else
                    {
                        bool currentGUIState = GUI.enabled;
                        GUI.enabled = false;
                        EditorGUI.IntPopup(fieldRect, 0, new string[1] { "No Tree Spawn Rules" }, new int[1] { 0});
                    }

                    bool selectionChanged = false;
                    if (oldTreeMaskValues.Count != currentTreeMaskValue.Count())
                    {
                        selectionChanged = true;
                    }
                    else
                    {
                        for (int i = 0; i < oldTreeMaskValues.Count; i++)
                        {
                            if (oldTreeMaskValues[i] != currentTreeMaskValue[i])
                            {
                                selectionChanged = true;
                                break;
                            }
                        }
                    }
                    
                    //selectedGUIDIndex = EditorGUI.IntPopup(fieldRect, selectedGUIDIndex, CollisionMask.m_allTreeSpawnRuleNames, CollisionMask.m_allTreeSpawnRuleIndices);
                    //if (selectedGUIDIndices >= 0 && selectedGUIDIndices < CollisionMask.m_allTreeSpawnRules.Length)
                    //{
                    //    collisionMask.m_treeSpawnRuleGUID = CollisionMask.m_allTreeSpawnRules[selectedGUIDIndices].GUID;
                    //}
                    if (oldType != collisionMask.m_type || selectionChanged)
                    {
                        //only now we update the actual tree GUID list
                        collisionMask.RadiusTreeMaskValues = currentTreeMaskValue;
                        if (collisionMask.m_Radius == 0.0f && collisionMask.m_selectedTreeSpawnRuleGUIDs.Count > 0)
                        {
                            SpawnRule selectedRule = CollisionMask.m_allTreeSpawnRules.FirstOrDefault(x => x.GUID == collisionMask.m_selectedTreeSpawnRuleGUIDs[0]);
                            if (selectedRule != null)
                            {
                                Spawner spawner = CollisionMask.m_allTreeSpawners.FirstOrDefault(x => x.m_settings.m_spawnerRules.Contains(selectedRule));
                                if (spawner != null)
                                {
                                    GameObject treePrefab = spawner.m_settings.m_resources.m_treePrototypes[selectedRule.m_resourceIdx].m_desktopPrefab;
                                    collisionMask.m_Radius = GaiaUtils.GetTreeRadius(treePrefab);
                                }
                            }
                        }
                        if (selectionChanged)
                        {
                            GaiaSessionManager.GetSessionManager().m_bakedMaskCache.BakeAllTreeCollisions(collisionMask, collisionMask.m_Radius);
                        }
                    }

                    fieldRect.x += rect.width * 0.2f;
                    fieldRect.width = rect.width * 0.1f;
                    collisionMask.m_Radius = EditorGUI.FloatField(fieldRect, collisionMask.m_Radius);
                    fieldRect.x += rect.width * 0.1f;
                    fieldRect.width = rect.width * 0.2f;
                    break;
                case BakedMaskType.RadiusTag:
                    //Tree dropdown
                    fieldRect.x += rect.width * 0.2f;
                    ////Building up a value array of incrementing ints of the size of the available tags this array will then match the displayed string selection in the popup
                    //int[] tagValueArray = Enumerable
                    //                    .Repeat(0, (int)((currentTerrain.terrainData.treePrototypes.Length - 0) / 1) + 1)
                    //                    .Select((tr, ti) => tr + (1 * ti))
                    //                    .ToArray();
                    string oldTag = collisionMask.m_tag;
                    collisionMask.m_tag = EditorGUI.TagField(fieldRect, collisionMask.m_tag);

                    if (oldType != collisionMask.m_type || oldTag != collisionMask.m_tag)
                    {
                        collisionMask.m_Radius = GaiaUtils.GetBoundsForTaggedObject(collisionMask.m_tag);
                    }


                    fieldRect.x += rect.width * 0.2f;
                    fieldRect.width = rect.width * 0.1f;
                    collisionMask.m_Radius = EditorGUI.FloatField(fieldRect, collisionMask.m_Radius);
                    fieldRect.x += rect.width * 0.1f;
                    fieldRect.width = rect.width * 0.2f;
                    break;
                case (BakedMaskType.LayerGameObject):
                    //Layer mask selection
                    fieldRect.x += rect.width * 0.2f;
                    EditorGUI.BeginChangeCheck();
                    collisionMask.m_layerMask = GaiaEditorUtils.LayerMaskFieldRect(fieldRect, new GUIContent(""), collisionMask.m_layerMask);
                    if (EditorGUI.EndChangeCheck())
                    {
                        collisionMask.m_layerMaskLayerNames = GaiaUtils.LayerMaskToString(collisionMask.m_layerMask);
                    }
                    EditorGUI.LabelField(fieldRect, m_editorUtils.GetContent("CollisionMaskLayerSelectionTooltip"));
                    fieldRect.x += rect.width * 0.2f;
                    fieldRect.width = rect.width * 0.1f;
                    collisionMask.m_growShrinkDistance = EditorGUI.FloatField(fieldRect, collisionMask.m_growShrinkDistance);
                    EditorGUI.LabelField(fieldRect, m_editorUtils.GetContent("CollisionMaskGrowShrinkDistanceTooltip"));
                    fieldRect.x += rect.width * 0.1f;
                    fieldRect.width = rect.width * 0.2f;
                    
                    break;
                case (BakedMaskType.LayerTree):
                    //Layer mask selection
                    fieldRect.x += rect.width * 0.2f;
                    EditorGUI.BeginChangeCheck();
                    collisionMask.m_layerMask = GaiaEditorUtils.LayerMaskFieldRect(fieldRect, new GUIContent(""), collisionMask.m_layerMask);
                    if (EditorGUI.EndChangeCheck())
                    {
                        collisionMask.m_layerMaskLayerNames = GaiaUtils.LayerMaskToString(collisionMask.m_layerMask);
                    }
                    EditorGUI.LabelField(fieldRect, m_editorUtils.GetContent("CollisionMaskLayerSelectionTooltip"));
                    fieldRect.x += rect.width * 0.2f;
                    fieldRect.width = rect.width * 0.1f;
                    collisionMask.m_growShrinkDistance = EditorGUI.FloatField(fieldRect, collisionMask.m_growShrinkDistance);
                    EditorGUI.LabelField(fieldRect, m_editorUtils.GetContent("CollisionMaskGrowShrinkDistanceTooltip"));
                    fieldRect.x += rect.width * 0.1f;
                    fieldRect.width = rect.width * 0.2f;

                    break;

            }

            if (GUI.Button(fieldRect, m_editorUtils.GetContent("MaskCollisionBake")))
            {
                switch (collisionMask.m_type)
                {
                    case BakedMaskType.RadiusTree:
                        GaiaSessionManager.GetSessionManager().m_bakedMaskCache.BakeAllTreeCollisions(collisionMask, collisionMask.m_Radius);
                        break;
                    case BakedMaskType.RadiusTag:
                        GaiaSessionManager.GetSessionManager().m_bakedMaskCache.BakeAllTagCollisions(collisionMask.m_tag, collisionMask.m_Radius);
                        break;
                    case BakedMaskType.LayerGameObject:
                        GaiaSessionManager.GetSessionManager().m_bakedMaskCache.BakeAllLayerGameObjectCollisions(collisionMask.m_layerMask, collisionMask.m_growShrinkDistance); //, collisionMask.m_Radius);
                        break;
                    case BakedMaskType.LayerTree:
                        GaiaSessionManager.GetSessionManager().m_bakedMaskCache.BakeAllLayerTreeCollisions(collisionMask.m_layerMask, collisionMask.m_growShrinkDistance); //, collisionMask.m_Radius);
                        break;
                }
            }
            EditorGUI.indentLevel = oldIndent;
        }

        public static GUIContent PropertyCount(string key, Array array, EditorUtils editorUtils)
        {
            GUIContent content = editorUtils.GetContent(key);
            content.text += " [" + array.Length + "]";
            return content;
        }
    }
}