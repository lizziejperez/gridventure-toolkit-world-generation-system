/*
* WorldFeatureSaveData.cs
* Gridventure Toolkit - World Save Data
* Author: Lizzie Perez
* Version: 0.0
*/
using System;

/// <summary>
/// Represents a saved world feature instance, including its type and world position.
/// </summary>
[Serializable]
public class WorldFeatureSaveData
{
    /// <summary>
    /// The unique identifier of the feature type.
    /// </summary>
    public string FeatureID;

    /// <summary>
    /// The world position X coordinate where the feature is placed.
    /// </summary>
    public float X;

    /// <summary>
    /// The world position Y coordinate where the feature is placed.
    /// </summary>
    public float Y;
}
