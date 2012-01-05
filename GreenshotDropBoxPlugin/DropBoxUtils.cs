/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2011-2012  Francis Noel
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using GreenshotPlugin.Controls;
using GreenshotPlugin.Core;

using AppLimit.CloudComputing.SharpBox;
using Dropbox=AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;

namespace GreenshotDropboxPlugin {
	/// <summary>
	/// Description of ImgurUtils.
	/// </summary>
	public class DropboxUtils {
		private static readonly log4net.ILog LOG = log4net.LogManager.GetLogger(typeof(DropboxUtils));
		public static string DROPBOX_APP_KEY = "uenz04wo9b4jrc8";
		public static string DROPBOX_APP_SECRET = "im1609dep8eq8o4";
		private static DropBoxConfiguration config = IniConfig.GetIniSection<DropBoxConfiguration>();

		private DropboxUtils() {
		}

		public static void LoadHistory() {
			if (config.runtimeDropboxHistory == null) {
				return;
			}
			if (config.DropboxUploadHistory == null)
			{
				return;
			}

			if (config.runtimeDropboxHistory.Count == config.DropboxUploadHistory.Count) {
				return;
			}
			// Load the Dropbox history
			List<string> hashes = new List<string>();
			foreach (string hash in config.DropboxUploadHistory.Keys)
			{
				hashes.Add(hash);
			}
			
			bool saveNeeded = false;

			foreach(string hash in hashes) {
				if (config.runtimeDropboxHistory.ContainsKey(hash)) {
					// Already loaded
					continue;
				}
				try {
					DropboxInfo imgurInfo = DropboxUtils.RetrieveDropboxInfo(hash);
					if (imgurInfo != null) {
						DropboxUtils.RetrieveDropboxThumbnail(imgurInfo);
						config.runtimeDropboxHistory.Add(hash, imgurInfo);
					} else {
						LOG.DebugFormat("Deleting not found Dropbox {0} from config.", hash);
						config.DropboxUploadHistory.Remove(hash);
						saveNeeded = true;
					}
				} catch (Exception e) {
					LOG.Error("Problem loading Dropbox history for hash " + hash, e);
				}
			}
			if (saveNeeded) {
				// Save needed changes
				IniConfig.Save();
			}
		}

		/// <summary>
		/// Do the actual upload to Dropbox
		/// For more details on the available parameters, see: http://sharpbox.codeplex.com/
		/// </summary>
		/// <param name="imageData">byte[] with image data</param>
		/// <returns>DropboxResponse</returns>
		public static DropboxInfo UploadToDropbox(byte[] imageData, string title, string filename)
		{

			// get the config of dropbox 
			Dropbox.DropBoxConfiguration dropBoxConfig =
			CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox) as
			Dropbox.DropBoxConfiguration;
			
			// instanciate the cloudstorage manager
			CloudStorage storage = new CloudStorage();

			// open the connection to the storage
			storage.Open(dropBoxConfig, config.DropboxAccessToken);

			// get the root entry of the cloud storage 
			ICloudDirectoryEntry root = storage.GetRoot();
			if (root == null)
			{
				Console.WriteLine("No root object found");
			}
			else
			{
				// create the file
				ICloudFileSystemEntry file = storage.CreateFile(root, filename);

				// build the data stream
				Stream data = new MemoryStream(imageData);

				// reset stream
				data.Position = 0;

				// upload data
				file.GetDataTransferAccessor().Transfer(data, nTransferDirection.nUpload, null, null);

			}

			// close the cloud storage connection
			if (storage.IsOpened)
			{
				storage.Close();
			}


			return RetrieveDropboxInfo(filename);
		}

		public static Image CreateThumbnail(Image image, int thumbWidth, int thumbHeight) {
			int srcWidth=image.Width;
			int srcHeight=image.Height; 
			Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);  
			using (Graphics gr = System.Drawing.Graphics.FromImage(bmp)) {
				gr.SmoothingMode = SmoothingMode.HighQuality  ; 
				gr.CompositingQuality = CompositingQuality.HighQuality; 
				gr.InterpolationMode = InterpolationMode.High; 
				System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
				gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);  
			}
			return bmp;
		}

		public static void RetrieveDropboxThumbnail(DropboxInfo imgurInfo) {
			LOG.InfoFormat("Retrieving Dropbox image for {0} with url {1}", imgurInfo.ID, imgurInfo);
			HttpWebRequest webRequest = (HttpWebRequest)NetworkHelper.CreatedWebRequest(imgurInfo.WebUrl);
			webRequest.Method = "GET";
			webRequest.ServicePoint.Expect100Continue = false;

			using (WebResponse response = webRequest.GetResponse()) {
				Stream responseStream = response.GetResponseStream();
				imgurInfo.Image = Image.FromStream(responseStream);
			}
			return;
		}

		public static DropboxInfo RetrieveDropboxInfo(string filename)
		{

			LOG.InfoFormat("Retrieving Dropbox info for {0}", filename);

			DropboxInfo dropBoxInfo = new DropboxInfo();

			dropBoxInfo.ID = filename;
			dropBoxInfo.Title = filename;
			dropBoxInfo.Timestamp = DateTime.Now;
			dropBoxInfo.WebUrl = string.Empty;

			// get the config of dropbox 
			Dropbox.DropBoxConfiguration dropBoxConfig =
			CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox) as
			Dropbox.DropBoxConfiguration;

			// instanciate the cloudstorage manager
			CloudStorage storage = new CloudStorage();

			// get the root entry of the cloud storage 
			ICloudDirectoryEntry root = storage.GetRoot();

			// open the connection to the storage
			storage.Open(dropBoxConfig, config.DropboxAccessToken);
			dropBoxInfo.WebUrl = storage.GetFileSystemObjectUrl(dropBoxInfo.ID, root).ToString();

			ICloudFileSystemEntry fileSystemEntry = storage.GetFileSystemObject(dropBoxInfo.ID, root);
			if (fileSystemEntry != null)
			{
				dropBoxInfo.Title = fileSystemEntry.Name;
				dropBoxInfo.Timestamp = fileSystemEntry.Modified;
			}

			// close the cloud storage connection
			if (storage.IsOpened)
			{
				storage.Close();
			}

			return dropBoxInfo;
		}

		public static void DeleteDropboxImage(DropboxInfo dropBoxInfo)
		{
			// Make sure we remove it from the history, if no error occured
			config.runtimeDropboxHistory.Remove(dropBoxInfo.ID);
			config.DropboxUploadHistory.Remove(dropBoxInfo.ID);

			// get the config of dropbox 
			Dropbox.DropBoxConfiguration dropBoxConfig =
			CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox) as
			Dropbox.DropBoxConfiguration;

			// instanciate the cloudstorage manager
			CloudStorage storage = new CloudStorage();

			// open the connection to the storage
			storage.Open(dropBoxConfig, config.DropboxAccessToken);

			// get the root entry of the cloud storage 
			ICloudDirectoryEntry root = storage.GetRoot();

			// delete a file
			ICloudFileSystemEntry fileSystemEntry = storage.GetFileSystemObject(dropBoxInfo.ID, root);
			if (fileSystemEntry != null)
			{
				storage.DeleteFileSystemEntry(fileSystemEntry);
			}
			// close the cloud storage connection
			if (storage.IsOpened)
			{
				storage.Close();
			}

			dropBoxInfo.Image = null;
		}

		internal static void LoadAccessToken()
		{

			string fileFullPath = Path.Combine(Environment.CurrentDirectory,Environment.UserName + "-Dropbox.tok");
			if (File.Exists(fileFullPath))
			{
				CloudStorage storage = new CloudStorage();

				StreamReader tokenStream = new StreamReader(fileFullPath);

				config.DropboxAccessToken = storage.DeserializeSecurityToken(tokenStream.BaseStream);

				// close the cloud storage connection
				if (storage.IsOpened)
				{
					storage.Close();
				}
			}
		}

		internal static void SaveAccessToken()
		{
			
			if (config.DropboxAccessToken != null)
			{
				// get the config of dropbox 
				Dropbox.DropBoxConfiguration dropBoxConfig =
				CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox) as
				Dropbox.DropBoxConfiguration;

				CloudStorage storage = new CloudStorage();

				// open the connection to the storage
				storage.Open(dropBoxConfig, config.DropboxAccessToken);

				Stream tokenStream = storage.SerializeSecurityToken(config.DropboxAccessToken);

				string fileFullPath = Path.Combine(Environment.CurrentDirectory, Environment.UserName + "-Dropbox.tok");

				// Create a FileStream object to write a stream to a file
				using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)tokenStream.Length))
				{
					// Fill the bytes[] array with the stream data
					byte[] bytesInStream = new byte[tokenStream.Length];
					tokenStream.Read(bytesInStream, 0, (int)bytesInStream.Length);

					// Use FileStream object to write to the specified file
					fileStream.Write(bytesInStream, 0, bytesInStream.Length);
				}

				// close the cloud storage connection
				if (storage.IsOpened)
				{
					storage.Close();
				}
			}
		}
	}
}
