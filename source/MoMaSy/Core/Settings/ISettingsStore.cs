namespace outcold.MoMaSy.Core.Settings
{
	internal interface ISettingsStore
	{
		object Get(string index);
		object Get(string index, object defaultValue);
		void Set(string index, object value);
		void Save();
	}
}