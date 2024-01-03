#region



#endregion

namespace STP.Common.Configuration.Interface
{
    public interface IJQueryConfigProvider
    {
        #region JQuery

        /// <summary>
        ///     Register JQuery
        /// </summary>
        void RegisterJQuery();

        /// <summary>
        ///     Register JQuery
        /// </summary>
        void RegisterJQuery10();

        //void RegisterJQueryPluggins();
        /// <summary>
        ///     Populate All JQuery Pluggins from config
        /// </summary>
        /// <summary>
        ///     <para>Populate Selected JQuery Pluggins</para>
        ///     <para>Check the config files for the number you want to load the script</para>
        ///     <para>Usage: STPScriptManager.Provider.RegisterJQueryPluggins("3,23");</para>
        /// </summary>
        /// <param name="ids">Pluggin Ids from the config files which will load in runtime</param>
        void RegisterJQueryPluggins(string ids);

        /// <summary>
        ///     <para>Populate Selected JQuery v 1.10 Pluggins</para>
        ///     <para>Check the config files for the number you want to load the script</para>
        ///     <para>Usage: STPScriptManager.Provider.RegisterJQueryPluggins("3,23");</para>
        /// </summary>
        /// <param name="ids">Pluggin Ids from the config files which will load in runtime</param>
        void RegisterJQuery10Pluggins(string ids);

        /// <summary>
        ///     Remove JQuery Pluggins
        /// </summary>
        void UnRegisterJQueryPluggins(string ids);

        /// <summary>
        ///     Remove All JQuery Pluggins
        /// </summary>
        void UnRegisterJQueryPluggins();

        //JQueryPlugginCollection GetJQueryPlugginCollection();
        //int TotalJQueryPluggins();
        //string GetJQueryPluggin(string key);
        //bool IsJQueryPlugginEnabled(string fileName);
        //string GetJQuery();
        //bool IsJQueryEnabled();

        #endregion
    }
}