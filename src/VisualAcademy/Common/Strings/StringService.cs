namespace Common.Strings;

public class StringService : IStringService
{
    /// <summary>
    /// 유니코드 이모티콘을 포함한 문자열 자르기
    /// From Dul.StringLibrary.cs
    /// </summary>
    /// <param name="str">한글, 영문, 유니코드 문자열</param>
    /// <param name="length">자를 문자열의 길이</param>
    /// <returns>잘라진 문자열: 안녕하세요. => 안녕...</returns>
    public string CutStringUnicode(string str, int length, string suffix = "...")
    {
        string result = str;

        if (length > 4) // 마이너스 값 들어오는 경우 제외
        {
            var si = new System.Globalization.StringInfo(str);
            var l = si.LengthInTextElements;

            if (l > (length - 3))
            {
                result = si.SubstringByTextElements(0, length - 3) + "" + suffix;
            }
        }

        return result;
    }
}
