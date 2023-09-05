using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Extensions
{
    private static readonly Dictionary<DirectionType, Vector2> _directions;
    private static readonly Dictionary<DirectionType, Vector3> _rotations;

    static Extensions()
    {
        _directions = new Dictionary<DirectionType, Vector2>
        {
            { DirectionType.Top, Vector2.up },
            { DirectionType.Right, Vector2.right },
            { DirectionType.Bottom, Vector2.down },
            { DirectionType.Left, Vector2.left }
        };
        _rotations = new Dictionary<DirectionType, Vector3>
        {
            { DirectionType.Top, new Vector3(0f, 0f, 0f) },
            { DirectionType.Right, new Vector3(0f, 0f, 270f) },
            { DirectionType.Bottom, new Vector3(0f, 0f, 180f) },
            { DirectionType.Left, new Vector3(0f, 0f, 90f) }
        };
    }

    public static Vector2 ConvertTypeToDirection(this DirectionType type)
    {
        return _directions[type];
    }

    public static Vector3 ConvertTypeToRotation(this DirectionType type)
    {
        return _rotations[type];
    }

    public static DirectionType ConvertDirectionFromType(this Vector2 direction)
    {
        return _directions.First(t => t.Value == direction).Key;
    }

    public static DirectionType ConvertRotationFromType(this Vector3 rotation)
    {
        if (_rotations.ContainsValue(rotation) == false) return DirectionType.None;
        return _rotations.First(t => t.Value == rotation).Key;
    }

    private static DirectionType RandomType()
    {
        var random = Random.Range(1, 5);
        return (DirectionType)Enum.GetValues(typeof(DirectionType)).GetValue(random);
    }

    public static DirectionType RandomType(this DirectionType dir)
    {
        return RandomType();
    }
}

public enum DirectionType : byte
{
    None,
    Top,
    Bottom,
    Right,
    Left
}

public enum SideType : byte
{
    None,
    Player,
    Enemy
}