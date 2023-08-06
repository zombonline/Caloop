using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SegmentSetup
{
    public PortionPowerType powerType;
    public int powerAmount;
}
[CreateAssetMenu(fileName = "New Level Info", menuName = "Level Info")]

public class LevelInfo : ScriptableObject
{
    [SerializeField] public int targetScore, actions;
    [SerializeField] public SegmentSetup[] segments;
    [SerializeField] public Wheel wheel;
    [TextArea(2, 5)]
    [SerializeField] public string hint;


}
