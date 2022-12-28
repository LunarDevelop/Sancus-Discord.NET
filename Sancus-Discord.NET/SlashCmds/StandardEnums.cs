using System.ComponentModel;
using Discord.Interactions;

namespace Sancus_Discord.NET.SlashCmds;

public static class StandardEnums {

    public enum TrueFalseChoice
    {
        [ChoiceDisplay("False"), ]
        False,
        [ChoiceDisplay("True")]
        True
    }

}

public static class EnumUtils
{
    /// <summary>
    /// Converts a True, False enum to bool objects
    /// </summary>
    /// <returns></returns>
    public static bool Value(this StandardEnums.TrueFalseChoice value)
    {
        return value switch
        {
            StandardEnums.TrueFalseChoice.True => true,
            StandardEnums.TrueFalseChoice.False => false,
            _ => throw new ArgumentOutOfRangeException("value")
        };
    }
}