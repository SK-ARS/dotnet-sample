#region



#endregion

namespace STP.Common.Configuration.Interface
{
    public interface IScriptConfigProvider
    {
        //void RegisterKnockout();
        //void RegisterScripts();
        /// <summary>
        ///     Populate JQuery
        /// </summary>
        /// <summary>
        ///     Populate All Scripts
        /// </summary>
        /// <summary>
        ///     <para>Populate Selected Scripts from Scripts Folders</para>
        ///     <para>Check the config files for the number you want to load the script</para>
        ///     <para>Usage  STPScriptManager.Provider.RegisterScripts("1,5,8");</para>
        /// </summary>
        /// <param name="ids">Script Ids from the config files which will load in runtime</param>
        void RegisterScripts(string ids);

        /// <summary>
        ///     Remove All Scripts
        /// </summary>
        void UnRegisterScripts(string ids);

        /// <summary>
        ///     Remove Scripts
        /// </summary>
        void UnRegisterScripts();

        /// <summary>
        ///     Register Resources
        /// </summary>
        void RegisterResourceScripts();

        //ScriptConfigCollection GetScriptCollection();
        //bool IsScriptEnabled(string fileName);
        //int TotalScripts();
        //string GetScript(string key);
        //string GetScriptFramework();
        //bool IsFrameworkEnabled();

        /// <summary>
        ///     Poplulate MultiForm Scripts
        ///     Created by Vijay warpe moving to Script Framework
        /// </summary>
        void RegisterMultiFormScripts();
    }
}