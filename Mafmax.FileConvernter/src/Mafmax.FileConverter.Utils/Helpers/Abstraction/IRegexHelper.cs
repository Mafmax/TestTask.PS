using System.Text.RegularExpressions;

namespace Mafmax.FileConverter.Utils.Helpers.Abstraction;

/// <summary>
/// Defines methods to work with regular expressions.
/// </summary>
public interface IRegexHelper
{

    /// <summary>
    /// Represents a rule for file id.
    /// </summary>
    Regex FileIdRegex { get; }
}
