using System.Text.RegularExpressions;

namespace Mafmax.FileConverter.Utils.Helpers.Abstraction;
public interface IRegexHelper
{
    Regex FileIdRegex { get; }
}
