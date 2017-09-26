using System;

public static class StringUtil
{
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
