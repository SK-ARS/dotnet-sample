namespace STP.Common.Configuration.Interface
{
    public interface IFrameworkConfigProvider
    {
        #region Framework and Pluggins

        /// <summary>
        ///     Register Framework
        /// </summary>
        void RegisterFramework();

        //void RegisterFrameworkPluggins();
        /// <summary>
        ///     Populate All Framework Pluggins
        /// </summary>
        /// <summary>
        ///     Populate Selected Framework Pluggins
        /// </summary>
        void RegisterFrameworkPluggins(string ids);

        /// <summary>
        ///     Remove Selected Framework Pluggins
        /// </summary>
        void UnRegisterFrameworkPluggins(string ids);

        /// <summary>
        ///     Remove All Framework Pluggins
        /// </summary>
        void UnRegisterFrameworkPluggins();

        #endregion

        //FrameworkPlugginCollection GetPlugginCollection();
        //int TotalPluggins();
        //string GetPluggin(string key);
        //bool IsPlugginEnabled(string fileName);
        //string GetFramework();
        //bool IsFrameworkEnabled();
    }
}