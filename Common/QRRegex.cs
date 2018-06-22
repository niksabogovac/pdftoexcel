namespace Common
{
    public static class QRRegex
    {
        public static  string FileNumber
        {
            get
            {
                return @"(RSRFBA)[0-9]{2}\-[0-9]{6}";
            }
        }

        public static int FileNumberLength
        {
            get
            {
                return 15;
            }
        }

    }
}
