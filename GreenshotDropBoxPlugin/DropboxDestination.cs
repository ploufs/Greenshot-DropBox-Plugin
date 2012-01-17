using System.ComponentModel;
using System.Drawing;
using Greenshot.Plugin;
using GreenshotPlugin.Core;
using IniFile;

namespace GreenshotDropboxPlugin
{
	class DropboxDestination:AbstractDestination
	{
		private static log4net.ILog LOG = log4net.LogManager.GetLogger(typeof(DropboxDestination));
		private static DropboxConfiguration config = IniConfig.GetIniSection<DropboxConfiguration>();
		private ILanguage lang = Language.GetInstance();

		private DropboxPlugin plugin = null;
		public DropboxDestination(DropboxPlugin plugin)
		{
			this.plugin = plugin;
		}
		
		public override string Designation {
			get {
				return "Picasa";
			}
		}

		public override string Description {
			get {
				return lang.GetString(LangKey.upload_menu_item);
			}
		}

		public override Image DisplayIcon {
			get {
				ComponentResourceManager resources = new ComponentResourceManager(typeof(DropboxPlugin));
				return (Image)resources.GetObject("Dropbox");
			}
		}

		public override bool ExportCapture(ISurface surface, ICaptureDetails captureDetails) {
			using (Image image = surface.GetImageForExport()) {
				bool uploaded = plugin.Upload(captureDetails, image);
				if (uploaded) {
					surface.SendMessageEvent(this, SurfaceMessageTyp.Info, "Exported to Dropbox");
					surface.Modified = false;
				}
				return uploaded;
			}
		}
	}
}
