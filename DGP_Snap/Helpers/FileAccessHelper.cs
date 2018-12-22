
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DGP_Snap.Helpers
{
    public class FileAccessHelper
    {
        public static string GetFilePickerPath(string filter,string filename,string path= null)
        {
            SaveFileDialog openFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Filter = filter,
                FilterIndex = 1,
                RestoreDirectory = true,
                CheckFileExists=false,
                
                FileName=filename,
            };
            if (path != null)
            {
                openFileDialog.InitialDirectory = path;
            }

            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName openfilename);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class OpenFileName
        {
            public int structSize = 0;
            public IntPtr hwnd = IntPtr.Zero;
            public IntPtr hinst = IntPtr.Zero;
            public string filter = null;
            public string custFilter = null;
            public int custFilterMax = 0;
            public int filterIndex = 0;
            public string file = null;
            public int maxFile = 0;
            public string fileTitle = null;
            public int maxFileTitle = 0;
            public string initialDir = null;
            public string title = null;
            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtMax = 0;
            public string defExt = null;
            public int custData = 0;
            public IntPtr pHook = IntPtr.Zero;
            public string template = null;
        }

        public static string GetWin32FilePickerPath()
        {
            OpenFileName ofn = new OpenFileName();
            ofn.structSize = Marshal.SizeOf(ofn);
            ofn.filter = "Project files\0*.png";
            ofn.file = new string(new char[256]);
            ofn.maxFile = ofn.file.Length;
            ofn.fileTitle = new string(new char[64]);
            ofn.maxFileTitle = ofn.fileTitle.Length;
            ofn.initialDir = "C:\\";
            ofn.title = "Open Project";
            ofn.defExt = "xml";
            ofn.structSize = Marshal.SizeOf(ofn);

            if (GetOpenFileName(ofn))
            {
                //此处做你想做的事 ...=ofn.file; 
                
            }
            return null;
        }
    }
}
