﻿// Copyright © 2018 Procedural Worlds Pty Limited.  All Rights Reserved.
using UnityEngine;
using System.Collections.Generic;
using static Gaia.GaiaConstants;
using System.Linq;

/*
 * Scriptable Object containing the stored terrain metadata for loading terrain scenes plus additional world metadata like terrain tilesize etc.
 */

namespace Gaia
{
    [System.Serializable]
    public class TerrainSceneStorage : ScriptableObject
    {
        /// <summary>
        /// Enables / Disables Terrain Loading on a global level.
        /// </summary>
        public bool m_terrainLoadingEnabled = true;

        /// <summary>
        /// Controls if a warning popup will be shown if terrain loading is disabled but loading a terrain is still attempted.
        /// </summary>
        public bool m_showTerrainLoadingDisabledWarning = true;
        
                /// <summary>
        /// Uses the unity addressable system to manage terrain loading during runtime. This can make it easier to update the terrain content of the project after release. Requires the unity addressable package to be installed in the project.
        /// </summary>
        public bool m_useAddressables = false;

        /// <summary>
        /// Uses the unity addressable system to manage terrain loading during runtime. This can make it easier to update the terrain content of the project after release. Requires the unity addressable package to be installed in the project.
        /// </summary>
        public bool m_preloadAddressablesWithImpostors = true;

        /// <summary>
        /// Enables a special mode where the terrain loading will only load in collider terrain scenes instead of the regular scenes.
        /// These collider scenes can be built with the terrain mesh exporter. This allows to run a scene with collisions only without terrain rendering e.g. for a server application
        /// </summary>
        public bool m_colliderOnlyLoading = false;

        /// <summary>
        /// The number of terrain tiles on the X axis
        /// </summary>
        public int m_terrainTilesX = 1;

        /// <summary>
        /// The number of terrain tiles on the Z axis
        /// </summary>
        public int m_terrainTilesZ = 1;

        /// <summary>
        /// The size of the individual terrain tiles
        /// </summary>
        public int m_terrainTilesSize;

        /// <summary>
        /// Set to true if the scene uses the floating point fix
        /// </summary>
        public bool m_useFloatingPointFix;

        /// <summary>
        /// Set to true if the scene has a world map. Used to not unneccessarily look for a world map in the scene.
        /// </summary>
        public bool m_hasWorldMap;

        /// <summary>
        /// The relative size of the world map in comparison to the actual terrain, e.g. 0.25% of the size of the full terrain.
        /// This info is used to calculate things like the relative sea level on the world map.
        /// </summary>
        public float m_worldMaprelativeSize = 0.5f;

        /// <summary>
        /// The relative size to heightmap pixel ratio between world map and local map
        /// This info is used to calculate heightmap relative things between world map and local map, e.g. stamper size
        /// </summary>
        public float m_worldMapRelativeHeightmapPixels = 1f;

        /// <summary>
        /// The heightmap resolution for the world map preview
        /// </summary>
        public int m_worldMapPreviewHeightmapResolution = 2049;

        /// <summary>
        /// The Range in unity units for the world map preview
        /// </summary>
        public float m_worldMapPreviewRange = 1024;

        /// <summary>
        /// The hypothetical terrain height on the world map preview
        /// </summary>
        public float m_worldMapPreviewTerrainHeight = 1024;

        /// <summary>
        /// Holds all terrain scenes in a multi-terrain scenario with exported terrains.
        /// </summary>
        public List<TerrainScene> m_terrainScenes = new List<TerrainScene>();

        /// <summary>
        /// Toggles if the terrain loader should deactivate the player at runtime
        /// </summary>
        public bool  m_deactivateRuntimePlayer = false;

        /// <summary>
        /// Toggles if the terrain loader should deactivate the lighting at runtime
        /// </summary>
        public bool m_deactivateRuntimeLighting = false;

        /// <summary>
        /// Toggles if the terrain loader should deactivate the audio at runtime
        /// </summary>
        public bool m_deactivateRuntimeAudio = false;

        /// <summary>
        /// Toggles if the terrain loader should deactivate the weather at runtime
        /// </summary>
        public bool m_deactivateRuntimeWeather = false;

        /// <summary>
        /// Toggles if the terrain loader should deactivate the water at runtime
        /// </summary>
        public bool m_deactivateRuntimeWater = false;

        /// <summary>
        /// Toggles if the terrain loader should deactivate the ScreenShotter at runtime
        /// </summary>
        public bool m_deactivateRuntimeScreenShotter = false;

        /// <summary>
        /// X world space position of the terrain with coordinates 0_0 of the world setup (when using a grid setup).
        /// </summary>
        public double m_pos00X = double.MinValue;

        /// <summary>
        /// Z world space position of the terrain with coordinates 0_0 of the world setup (when using a grid setup).
        /// </summary>
        public double m_pos00Z = double.MinValue;

    }
}
