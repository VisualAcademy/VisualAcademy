namespace Common.Strings;

public interface IStringService
{
    string CutStringUnicode(string str, int length, string suffix = "...");
}
