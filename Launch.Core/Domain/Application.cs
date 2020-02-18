using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
//using System.Windows;
//using System.Windows.Media;

namespace Launch.Core.Domain
{
    public class Application
    {
        // [Key]
        public string Name { get; set; }
        public string Target { get; set; }
        public string WorkingDirectory { get; set; }
        public string IconPath { get; set; }
        public bool RunAsAdmin { get; set; }

        public Application() { }
        public Application(string target)
        {
            Target = target;
        }

        //private ImageSource _iconImage;
        //public ImageSource IconImage
        //{
        //    get
        //    {
        //        if (_iconImage == null && File.Exists(IconFilePath))
        //        {
        //            using (Icon sysicon = Icon.ExtractAssociatedIcon(IconFilePath))
        //            {
        //                _iconImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
        //                                sysicon.Handle,
        //                                System.Windows.Int32Rect.Empty,
        //                                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()
        //                            );
        //            }
        //        }

        //        return _iconImage;
        //    }
        //}
    }
}
