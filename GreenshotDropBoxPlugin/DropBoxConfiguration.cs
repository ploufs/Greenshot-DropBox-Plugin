/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2011  Francis  Noel
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on Sourceforge: http://sourceforge.net/projects/greenshot/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using GreenshotPlugin.Controls;
using GreenshotPlugin.Core;
using AppLimit.CloudComputing.SharpBox;

namespace GreenshotDropBoxPlugin {
	/// <summary>
	/// Description of ImgurConfiguration.
	/// </summary>
	[IniSection("DropBox", Description = "Greenshot DropBox Plugin configuration")]
	public class DropBoxConfiguration : IniSection {
		[IniProperty("DropBoxAccessToken", Description = "Token.", DefaultValue = "")]
		public ICloudStorageAccessToken DropBoxAccessToken;

		[IniProperty("UploadFormat", Description="What file type to use for uploading", DefaultValue="png")]
		public OutputFormat UploadFormat;

		[IniProperty("UploadJpegQuality", Description="JPEG file save quality in %.", DefaultValue="80")]
		public int UploadJpegQuality;

		[IniProperty("AfterUploadOpenHistory", Description = "After upload open history.", DefaultValue = "true")]
		public bool AfterUploadOpenHistory;

		[IniProperty("AfterUploadLinkToClipBoard", Description = "After upload send Dropbox link to clipboard.", DefaultValue = "true")]
		public bool AfterUploadLinkToClipBoard;

		[IniProperty("DropBoxUploadHistory", Description = "DropBox upload history (DropBoxUploadHistory.hash=deleteHash)")]
		public Dictionary<string, string> DropBoxUploadHistory;
		
		// Not stored, only run-time!
		public Dictionary<string, DropBoxInfo> runtimeDropBoxHistory = new Dictionary<string, DropBoxInfo>();

		/// <summary>
		/// Supply values we can't put as defaults
		/// </summary>
		/// <param name="property">The property to return a default for</param>
		/// <returns>object with the default value for the supplied property</returns>
		public override object GetDefault(string property) {
			switch(property) {
				case "DropBoxUploadHistory":
					return new Dictionary<string, string>();
			}
			return null;
		}
			/// <summary>
		/// A form for token
		/// </summary>
		/// <returns>bool true if OK was pressed, false if cancel</returns>
		public bool ShowConfigDialog() {
			SettingsForm settingsForm;
			ILanguage lang = Language.GetInstance();

			BackgroundForm backgroundForm = BackgroundForm.ShowAndWait(DropBoxPlugin.Attributes.Name, lang.GetString(LangKey.communication_wait));
			try {
				settingsForm = new SettingsForm(this);
			} finally {
				backgroundForm.CloseDialog();
			}
			settingsForm.AuthToken = this.DropBoxAccessToken;
			settingsForm.UploadFormat = this.UploadFormat.ToString();
			settingsForm.AfterUploadOpenHistory = this.AfterUploadOpenHistory;
			settingsForm.AfterUploadLinkToClipBoard  = this.AfterUploadLinkToClipBoard;
			DialogResult result = settingsForm.ShowDialog();
			if (result == DialogResult.OK)
			{

				this.DropBoxAccessToken = settingsForm.AuthToken;
				this.UploadFormat = (OutputFormat)Enum.Parse(typeof(OutputFormat), settingsForm.UploadFormat.ToLower());
				
				this.AfterUploadOpenHistory=settingsForm.AfterUploadOpenHistory;
				this.AfterUploadLinkToClipBoard=settingsForm.AfterUploadLinkToClipBoard;
				IniConfig.Save();

				// Save DropBoxAccessToken from file
				DropBoxUtils.SaveAccessToken();

				return true;
			}
			return false;
		}
	}
}
