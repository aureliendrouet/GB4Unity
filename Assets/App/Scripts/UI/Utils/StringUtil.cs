using System;

public static class StringUtil
{
    public static float ToFloat (string value)
    {
        float ret;
        bool parsed = float.TryParse (value, out ret);
        if (!parsed) {
            ret = 0f;
        }
        return ret;
    }

    public static byte HexToByte (string value)
    {
        byte ret;
        try {
            ret = Convert.ToByte (value, 16);
        } catch {
            ret = 0;
        }
        return ret;
    }
}
