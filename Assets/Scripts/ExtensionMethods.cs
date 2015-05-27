using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ExtensionMethods
{
    // We extending some classes yo

    private static ColorBlock defaultColors = new ColorBlock
    {
        normalColor = new Color(1, 1, 1, 0.68f),
        highlightedColor = new Color(1, 1, 1, 0.9f),
        colorMultiplier = 1f,
        disabledColor = new Color(1, 1, 1, 0.5f),
        fadeDuration = 0.1f,
        pressedColor = new Color(1, 1, 1, 0.6f)
    };
    private static ColorBlock highlightColors = new ColorBlock
    {
        normalColor = new Color(1, 1, 1, 1),
        highlightedColor = new Color(1, 1, 1, 0.9f),
        colorMultiplier = 1f,
        disabledColor = new Color(1, 1, 1, 0.5f),
        fadeDuration = 0.1f,
        pressedColor = new Color(1, 1, 1, 0.7f)
    };

    // TODO: Add palette argument
    public static void Highlight(this Button button, bool high = true)
    {
        button.colors = high ? highlightColors : defaultColors;
    }
}