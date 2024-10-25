using System.Diagnostics;

namespace Merchanter.Classes {
    public static class Log
    {
        public static string LastMessage { get; set; }
        public static DateTime LastDate { get; set; }

        public static void It(string _m3ssage)
        {
            Debug.WriteLine(_m3ssage);
            LastMessage = _m3ssage;
            LastDate = DateTime.Now;
        }
        public static void It(Exception _ex)
        {
            Console.WriteLine(_ex.ToString());
            Debug.WriteLine(_ex.ToString());
            LastMessage = _ex.ToString();
            LastDate = DateTime.Now;
        }
    }
}
