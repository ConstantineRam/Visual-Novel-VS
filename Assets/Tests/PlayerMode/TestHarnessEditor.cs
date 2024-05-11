using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class TestHarnessEditor : MonoBehaviour
{
    [Required] [SerializeField] private RectTransform parent;

    public RectTransform GetTestParent => parent;
}
