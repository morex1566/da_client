
namespace NetCommon
{
    /// <summary>
    /// Singleton base class
    /// </summary>
    public class Singleton<T> where T : class, new()
    {
        public static T GetInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }


        protected static T _instance;
    }
}
