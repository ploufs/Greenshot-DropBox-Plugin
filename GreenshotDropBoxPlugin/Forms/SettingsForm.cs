﻿/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2011  Francis Noel
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
using System.Drawing;
using System.Windows.Forms;

using GreenshotDropBoxPlugin.Forms;
using GreenshotPlugin.Core;
using DropBox = AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;
using AppLimit.CloudComputing.SharpBox;

namespace GreenshotDropBoxPlugin {
	/// <summary>
	/// Description of PasswordRequestForm.
	/// </summary>
	public partial class SettingsForm : Form {
		private ILanguage lang = Language.GetInstance();
	 private  DropBox.DropBoxConfiguration config = null;
	   private DropBox.DropBoxRequestToken requestToken = null;


		public SettingsForm(DropBoxConfiguration config) {
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			InitializeTexts();
			
			combobox_uploadimageformat.Items.Clear();
			foreach(OutputFormat format in Enum.GetValues(typeof(OutputFormat))) {
				combobox_uploadimageformat.Items.Add(format.ToString());
			}

			DropBoxUtils.LoadHistory();

			if (config.runtimeDropBoxHistory.Count > 0) {
				historyButton.Enabled = true;
			} else {
				historyButton.Enabled = false;
			}
		}
				
		private void InitializeTexts() {
			this.label_AuthToken.Text = lang.GetString(LangKey.label_AuthToken);
			this.buttonOK.Text = lang.GetString(LangKey.OK);
			this.buttonAuthenticate.Text = lang.GetString(LangKey.buttonAuthenticate);
			this.buttonCancel.Text = lang.GetString(LangKey.CANCEL);
			this.Text = lang.GetString(LangKey.settings_title);
			this.label_upload_format.Text = lang.GetString(LangKey.label_upload_format);
			this.label_AfterUpload.Text = lang.GetString(LangKey.label_AfterUpload);
			this.checkboxAfterUploadOpenHistory.Text = lang.GetString(LangKey.label_AfterUploadOpenHistory);
			this.checkboxAfterUploadLinkToClipBoard.Text = lang.GetString(LangKey.label_AfterUploadLinkToClipBoard);
		}

		public bool AfterUploadOpenHistory {
			get { return checkboxAfterUploadOpenHistory.Checked; }
			set { checkboxAfterUploadOpenHistory.Checked = value; }
		}
		public bool AfterUploadLinkToClipBoard
		{
			get { return checkboxAfterUploadLinkToClipBoard.Checked; }
			set { checkboxAfterUploadLinkToClipBoard.Checked = value; }
		}

		private ICloudStorageAccessToken authToken;
		public ICloudStorageAccessToken AuthToken
		{
			get { return this.authToken; }
			set
			{
				this.authToken = value;
				textBoxAuthToken.Text = AuthToken.ToString();
			}
		}
		
		public string UploadFormat {
			get {return combobox_uploadimageformat.Text;}
			set {combobox_uploadimageformat.Text = value;}
		}

		void ButtonOKClick(object sender, EventArgs e)
		{

			if (config != null)
			{
				// create the access token 
				AuthToken = DropBox.DropBoxStorageProviderTools.ExchangeDropBoxRequestTokenIntoAccessToken(config,
DropBoxUtils.DROPBOX_APP_KEY, DropBoxUtils.DROPBOX_APP_SECRET, requestToken);

				this.DialogResult = DialogResult.OK;
			}
			else
			{
				this.DialogResult = DialogResult.OK;
			}
		}
		
		void ButtonCancelClick(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
		}
		
		void ButtonHistoryClick(object sender, EventArgs e) {
			DropBoxHistory.ShowHistory();
		}

	   
		private void buttonAuthenticate_Click(object sender, EventArgs e)
		{
			if (config == null)
			{
				// get the config of dropbox 
				config =
				CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox) as
				DropBox.DropBoxConfiguration;

				// set your own callback which will be called from DropBox after successful  
				// authorization 
				config.AuthorizationCallBack = new Uri("http://getgreenshot.org/");

				// create a request token 
				requestToken =
				DropBox.DropBoxStorageProviderTools.GetDropBoxRequestToken(config,
				DropBoxUtils.DROPBOX_APP_KEY,
				DropBoxUtils.DROPBOX_APP_SECRET);

				// call the authorization url via WebBrowser Plugin 
				String AuthorizationUrl =
				DropBox.DropBoxStorageProviderTools.GetDropBoxAuthorizationUrl(config, requestToken);

				System.Diagnostics.Process.Start(AuthorizationUrl);
			}
			else
			{
				 // create the access token 
				AuthToken = DropBox.DropBoxStorageProviderTools.ExchangeDropBoxRequestTokenIntoAccessToken(config,
DropBoxUtils.DROPBOX_APP_KEY, DropBoxUtils.DROPBOX_APP_SECRET, requestToken);
			}
		}
	}
}