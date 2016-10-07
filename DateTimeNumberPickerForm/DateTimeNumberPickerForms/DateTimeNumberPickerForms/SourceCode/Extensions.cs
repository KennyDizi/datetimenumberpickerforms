namespace DateTimeNumberPickerForms.SourceCode
{
    public static class Extensions
    {
        public static int StringToInt(this string number)
        {
            int intnum;
            int.TryParse(number, out intnum);
            return intnum;
        }
    }
}
