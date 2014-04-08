namespace outcold.MoMaSy.Core.Settings
{
	public class SettingsStore : ISettingsStore
	{
		public object Get(string index)
		{
			return Properties.Settings.Default[index];
		}

		public object Get(string index, object defaultValue)
		{
			return Get(index) ?? defaultValue;
		}

		public void Set(string index, object value)
		{
			Properties.Settings.Default[index] = value;
		}

		public void Save()
		{
			Properties.Settings.Default.Save();
		}
	}
}