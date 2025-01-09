using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptable Objects/GameInfo")]
public class GameInfo : ScriptableObject
{
    [field: SerializeField] public List<string> Players { get; set; }
}
