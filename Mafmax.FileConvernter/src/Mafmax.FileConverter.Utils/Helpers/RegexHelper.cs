using System.Text.RegularExpressions;
using Mafmax.FileConverter.Utils.Helpers.Abstraction;

namespace Mafmax.FileConverter.Utils.Helpers;
public class RegexHelper : IRegexHelper
{
    /// <inheritdoc />
    public Regex FileIdRegex { get; } = new("[0-9A-F]{24}",
    RegexOptions.Compiled | RegexOptions.IgnoreCase);
}
