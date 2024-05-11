using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used for objects that show localized texts.
/// </summary>
public interface TextBased
{
    string GetLocalizationId { get; }
    string GetShownString { get; }

    const string NoID = "TextBasedNoId";
    const string NoLocalization = "TextBasedNoLocalization";
    const string MissingTextField = "TextBasedMissingTextField";
}
