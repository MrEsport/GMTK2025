using UnityEngine;

public static class ColorExtension
{
    public static Color pink { get => new Color(1f, .373f, .784f); }
    public static Color lightRed { get => new Color(1f, .196f, .196f); }
    public static Color red => Color.red;
    public static Color darkRed { get => new Color(.392f, 0f, 0f); }
    public static Color brown { get => new Color(.392f, .196f, 0f); }
    public static Color orange { get => new Color(1f, .392f, 0f); }
    public static Color yellow => Color.yellow;
    public static Color lime { get => new Color(.335f, 1f, .078f); }
    public static Color lightGreen { get => new Color(.35f, .982f, .35f); }
    public static Color green { get => new Color(0f, .784f, .078f); }
    public static Color darkGreen { get => new Color(0f, .392f, 0f); }
    public static Color teal { get => new Color(0f, 1f, .588f); }
    public static Color cyan => Color.cyan;
    public static Color lightBlue { get => new Color(0f, .392f, 1f); }
    public static Color blue { get => new Color(0f, .078f, 1f); }
    public static Color darkBlue { get => new Color(0f, 0f, .392f); }
    public static Color indigo { get => new Color(.157f, .02f, .626f); }
    public static Color purple { get => new Color(.51f, 0f, 1f); }
    public static Color violet { get => new Color(.706f, 0f, .902f); }
    public static Color magenta => Color.magenta;
    public static Color white => Color.white;
    public static Color lightGrey { get => new Color(.75f, .75f, .75f); }
    public static Color grey => Color.grey;
    public static Color darkGrey { get => new Color(.2f, .2f, .2f); }
    public static Color black => Color.black;
    public static Color transparent { get => new Color(1f, 1f, 1f, 0.333f); }
    public static Color clear => Color.clear;

    /// <summary>
    /// Returns this color with an alpha of 0
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color Clear(this Color color)
    {
        return new Color(color.r, color.g, color.b, 0f);
    }

    /// <summary>
    /// Returns the color as a hexadecimal string
    /// </summary>
    /// <param name="color"></param>
    /// <param name="alpha">change hex format between "RRGGBB" -<i>false</i>- to "RRGGBBAA" -<i>true</i>-</param>
    /// <param name="symbol">add "#" before the hex code</param>
    /// <returns></returns>
    public static string Hex(this Color color, bool alpha = false, bool symbol = true)
    {
        string hex = alpha ?
            ColorUtility.ToHtmlStringRGBA(color) :
            ColorUtility.ToHtmlStringRGB(color);

        return symbol ? string.Concat("#", hex) : hex;
    }

    /// <summary>
    /// Format used in <see cref="ColorToString"/>
    /// </summary>
    public enum ColorStringFormat
    {
        [Tooltip("RGB(255, 255, 255)")] RGB,
        [Tooltip("RGBA(255, 255, 255, 255)")] RGBA,
        [Tooltip("RGB(1.000, 1.000, 1.000)")] RGBf,
        [Tooltip("RGBA(1.000, 1.000, 1.000, 1.000)")] RGBAf,
        [Tooltip("#RRGGBB")] HexRGB,
        [Tooltip("#RRGGBBAA")] HexRGBA
    }

    /// <summary>
    /// Returns RGB values in a string
    /// </summary>
    /// <param name="color"></param>
    /// <param name="format">string format</param>
    /// <returns></returns>
    public static string ColorToString(Color color, ColorStringFormat format)
    {
        return (format) switch
        {
            ColorStringFormat.RGB => $"RGB({Mathf.RoundToInt(color.r * 255f)}, {Mathf.RoundToInt(color.g * 255f)}, {Mathf.RoundToInt(color.b * 255f)})",
            ColorStringFormat.RGBA => $"RGBA({Mathf.RoundToInt(color.r * 255f)}, {Mathf.RoundToInt(color.g * 255f)}, {Mathf.RoundToInt(color.b * 255f)}, {Mathf.RoundToInt(color.a * 255f)})",
            ColorStringFormat.RGBf => $"RGB({color.r:F3}, {color.g:F3}, {color.b:F3})",
            ColorStringFormat.RGBAf => $"RGBA({color.r:F3}, {color.g:F3}, {color.b:F3}, {color.a:F3})",
            ColorStringFormat.HexRGB => color.Hex(),
            ColorStringFormat.HexRGBA => color.Hex(true),

            _ => $"RGB({Mathf.RoundToInt(color.r / 255f)}, {Mathf.RoundToInt(color.g / 255f)}, {Mathf.RoundToInt(color.b / 255f)})"
        }  + " " + color.ColorIconString();
    }

    private static string ColorIconString(this Color color)
    {
        return $"<color={color.Hex()}>█</color>";
    }
}
