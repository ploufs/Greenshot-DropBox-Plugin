/*
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;

using Greenshot.Plugin;
using GreenshotDropBoxPlugin.Forms;
using GreenshotPlugin.Controls;
using GreenshotPlugin.Core;

namespace GreenshotDropBoxPlugin
{
    /// <summary>
    /// This is the DropBox base code
    /// </summary>
    public class DropBoxPlugin : IGreenshotPlugin
    {
        private static readonly log4net.ILog LOG = log4net.LogManager.GetLogger(typeof(DropBoxPlugin));
        private static DropBoxConfiguration config;
        public static PluginAttribute Attributes;
        private ILanguage lang = Language.GetInstance();
        private IGreenshotPluginHost host;
        private ICaptureHost captureHost = null;
        private ComponentResourceManager resources;

        public DropBoxPlugin()
        {
        }

        /// <summary>
        /// Implementation of the IGreenshotPlugin.Initialize
        /// </summary>
        /// <param name="host">Use the IGreenshotPluginHost interface to register events</param>
        /// <param name="captureHost">Use the ICaptureHost interface to register in the MainContextMenu</param>
        /// <param name="pluginAttribute">My own attributes</param>
        public virtual void Initialize(IGreenshotPluginHost pluginHost, ICaptureHost captureHost, PluginAttribute myAttributes)
        {
            this.host = (IGreenshotPluginHost)pluginHost;
            this.captureHost = captureHost;
            Attributes = myAttributes;
            host.OnImageEditorOpen += new OnImageEditorOpenHandler(ImageEditorOpened);

            // Register configuration (don't need the configuration itself)
            config = IniConfig.GetIniSection<DropBoxConfiguration>();
            resources = new ComponentResourceManager(typeof(DropBoxPlugin));

            // load DropBoxAccessToken from file
            DropBoxUtils.LoadAccessToken();

        }

        public virtual void Shutdown()
        {
            LOG.Debug("Dropbox Plugin shutdown.");
            host.OnImageEditorOpen -= new OnImageEditorOpenHandler(ImageEditorOpened);
        }

        /// <summary>
        /// Implementation of the IPlugin.Configure
        /// </summary>
        public virtual void Configure()
        {
            config.ShowConfigDialog();
        }

        /// <summary>
        /// This will be called when Greenshot is shutting down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Closing(object sender, FormClosingEventArgs e)
        {
            LOG.Debug("Application closing, de-registering Dropbox Plugin!");
            Shutdown();
        }

        /// <summary>
        /// Implementation of the OnImageEditorOpen event
        /// Using the ImageEditor interface to register in the plugin menu
        /// </summary>
        private void ImageEditorOpened(object sender, ImageEditorOpenEventArgs eventArgs)
        {
            ToolStripMenuItem itemFile = new ToolStripMenuItem();
            itemFile.Text = lang.GetString(LangKey.upload_menu_item); //"Upload to DropBox";
            itemFile.Tag = eventArgs.ImageEditor;
            itemFile.Click += new System.EventHandler(EditMenuClick);
            itemFile.ShortcutKeys = ((Keys)((Keys.Control | Keys.D)));
            itemFile.Image = (Image)resources.GetObject("Dropbox");
            PluginUtils.AddToFileMenu(eventArgs.ImageEditor, itemFile);

            ToolStripMenuItem itemPlugInRoot = new ToolStripMenuItem();
            itemPlugInRoot.Text = "Dropbox";
            itemPlugInRoot.Tag = eventArgs.ImageEditor;
            itemPlugInRoot.Image = (Image)resources.GetObject("Dropbox");

            ToolStripMenuItem itemPlugInUplaoad = new ToolStripMenuItem();
            itemPlugInUplaoad.Text = lang.GetString(LangKey.Upload);
            itemPlugInUplaoad.Tag = eventArgs.ImageEditor;
            itemPlugInUplaoad.Click += new System.EventHandler(EditMenuClick);
            itemPlugInRoot.DropDownItems.Add(itemPlugInUplaoad);

            ToolStripMenuItem itemPlugInHistory = new ToolStripMenuItem();
            itemPlugInHistory.Text = lang.GetString(LangKey.History);
            itemPlugInHistory.Tag = eventArgs.ImageEditor;
            itemPlugInHistory.Click += new System.EventHandler(HistoryMenuClick);
            itemPlugInRoot.DropDownItems.Add(itemPlugInHistory);

            ToolStripMenuItem itemPlugInConfig = new ToolStripMenuItem();
            itemPlugInConfig.Text = lang.GetString(LangKey.Configure);
            itemPlugInConfig.Tag = eventArgs.ImageEditor;
            itemPlugInConfig.Click += new System.EventHandler(ConfigMenuClick);
            itemPlugInRoot.DropDownItems.Add(itemPlugInConfig);

            PluginUtils.AddToPluginMenu(eventArgs.ImageEditor, itemPlugInRoot);
        }

        public void HistoryMenuClick(object sender, EventArgs eventArgs)
        {
            DropBoxUtils.LoadHistory();
            DropBoxHistory.ShowHistory();
        }

        public void ConfigMenuClick(object sender, EventArgs eventArgs)
        {
            config.ShowConfigDialog();
        }

        /// <summary>
        /// This will be called when the menu item in the Editor is clicked
        /// </summary>
        public void EditMenuClick(object sender, EventArgs eventArgs)
        {
            if (config.DropBoxAccessToken==null)
            {
                MessageBox.Show(lang.GetString(LangKey.TokenNotSet), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                IImageEditor imageEditor = (IImageEditor)item.Tag;
                using (MemoryStream stream = new MemoryStream())
                {
                    BackgroundForm backgroundForm = BackgroundForm.ShowAndWait(Attributes.Name, lang.GetString(LangKey.communication_wait));

                    imageEditor.SaveToStream(stream, config.UploadFormat, config.UploadJpegQuality);
                    byte[] buffer = stream.GetBuffer();
                    try
                    {
                        string filename = Path.GetFileName(host.GetFilename(config.UploadFormat, imageEditor.CaptureDetails));
                        DropBoxInfo DropBoxInfo = DropBoxUtils.UploadToDropBox(buffer, imageEditor.CaptureDetails.Title, filename);

                        if (config.DropBoxUploadHistory == null)
                        {
                            config.DropBoxUploadHistory = new Dictionary<string, string>();
                        }

                        if (DropBoxInfo.ID != null)
                        {
                            LOG.InfoFormat("Storing Dropbox upload for id {0}", DropBoxInfo.ID);

                            config.DropBoxUploadHistory.Add(DropBoxInfo.ID, DropBoxInfo.ID);
                            config.runtimeDropBoxHistory.Add(DropBoxInfo.ID, DropBoxInfo);
                        }

                        DropBoxInfo.Image = DropBoxUtils.CreateThumbnail(imageEditor.GetImageForExport(), 90, 90);
                        // Make sure the configuration is save, so we don't lose the deleteHash
                        IniConfig.Save();
                        // Make sure the history is loaded, will be done only once
                        DropBoxUtils.LoadHistory();

                        // Show
                        if (config.AfterUploadOpenHistory)
                        {
                            DropBoxHistory.ShowHistory();
                        }

                        if (config.AfterUploadLinkToClipBoard)
                        {
                            Clipboard.SetText(DropBoxInfo.WebUrl);
                        }

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(lang.GetString(LangKey.upload_failure) + " " + e.ToString());
                    }
                    finally
                    {
                        backgroundForm.CloseDialog();
                    }
                }
            }
        }
    }
}
