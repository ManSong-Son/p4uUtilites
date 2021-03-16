namespace p4uUtilities
{
    public interface ISendable
    {
        void Init();
        string Name { get; set; }

    }
    public static class UtilitiesManager
    {

        static PathInfo _pathInfo = new PathInfo();
        static Log _log = new Log();

        public static Log Log { get => _log;}
        public static PathInfo PathInfo { get => _pathInfo; }

        public static void Init()
        {
            _pathInfo.Init();
            _log.Init();
        }
    }
}
