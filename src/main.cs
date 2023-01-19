using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Management;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;


// NOTES:
//
// 1 - When final compile change the textbox borders to none. Currently they are on fixedSingle
// 2 -  NTLM Hash Support DONE -- TEST
// 3 - Add Console logging 
// 4 - Memory Map files for access speed
// 5 - System Memory and CPU 
// 6 - UI Diabled untill load is complete
// 7 - Make passwordFoundTextBox and statusBox Lable for easy access
// 8 - Add Animation for loading & waiting make giff
// 9 - Add Time where needed 
// 10 - Add Password Generation Feature


namespace PasswordCracker
{
    public partial class main : Form
    {
        // ################################################################### //
        // ##########            Global Variable Region              ######### //
        // ################################################################### //
        #region
        // Global Variables
        public string _IMPORTED_PASSWORDLIST;
        private string _HASH_TO_CRACK = null;
        public bool _LOADING_STATUS = false;
        public bool _THREAD_RUNNING_STATUS =false;

        // !!! IMPORTANT !!!
        // In later version concider the password file Length so that there are not too many threads.
        private int _THREAD_COUNT = 1;
        public int _PASSWORD_FILE_LINE_COUNT;

        // Font Objects
        private PrivateFontCollection _PRIV_FONT_MEMORY;
        private Font _MAIN_UI_FONT;

        // Main Threads
        Thread _MD5_THREAD_MAIN;
        Thread _SHA1_THREAD_MAIN;
        Thread _SHA256_THREAD_MAIN;
        Thread _NTLM_THREAD_MAIN;

        //Main Threads status
        bool _MD5_MAIN_THREAD_STATUS = false;
        bool _SHA1_MAIN_THREAD_STATUS = false;
        bool _SHA256_MAIN_THREAD_STATUS = false;
        bool _NTLM_MAIN_THREAD_STATUS = false;
        #endregion

        /// <summary>
        /// Main Entry Point of form
        /// </summary>
        public main()
        {
            InitializeComponent();
            onLoad();
            diableUI();

            Task checkLoadStatus = new Task(new Action(loadingLoop)); // Thread that will run until formClose() (It will check the _LOADING_STATUS and run or stop loading animation)
            checkLoadStatus.Start();

            _LOADING_STATUS = true; // Begin load animation 

            Task initCPUCheck = new Task(new Action(CPUCheck)); // Thread to check the CPU and work out core count to work out how much threads to use (password list lenght will also affect thread count)
            initCPUCheck.Start();
        }

        // ################################################################### //
        // ##########             Windows Event Region               ######### //
        // ################################################################### //
        #region

        /// <summary>
        /// The event will run when the loacte password file button is pressed and will write selected file name
        /// </summary>
        private void locateFile_Click(object sender, EventArgs e)
        {
            string passwordListFileName = "";

            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                passwordListFileName = Path.GetFileName(openFileDialog1.FileName);
                _IMPORTED_PASSWORDLIST = openFileDialog1.FileName;

            }
            selectedFileTextBox.Text = "";
            selectedFileTextBox.Text = passwordListFileName;
            Task fileCheck = new Task(new Action(checkFileLines));
            fileCheck.Start();


        }

        /// <summary>
        /// This event will trigger the whole password cracking process
        /// It will trigger the main parent thread for the selected algorithm 
        /// </summary>
        private void _launch_Click(object sender, EventArgs e)
        {
            _HASH_TO_CRACK += hashTextBox.Text;
            statusBox.Visible = false;
            //progressConsole.Text = "";

            if (_HASH_TO_CRACK != "") // Check if user has entered a hash value 
            {
                if (algorithComboBox.SelectedIndex != -1) // Check if the user has selcted an algorithm
                {
                    scroll();
                    //MD5
                    if (algorithComboBox.SelectedIndex == 0)
                    {
                        progressConsole.Text += "#> MD5 Selected..." + Environment.NewLine;
                        _MD5_THREAD_MAIN = new Thread(() => crackMD5());
                        _MD5_THREAD_MAIN.Start();
                        _MD5_MAIN_THREAD_STATUS = true;
                        _LOADING_STATUS = true;
                    }
                    //SHA1
                    if (algorithComboBox.SelectedIndex == 1)
                    {
                        progressConsole.Text += "#> SHA1 Selected..." + Environment.NewLine;
                        _SHA1_THREAD_MAIN = new Thread(() => crackSHA1());
                        _SHA1_THREAD_MAIN.Start();
                        _SHA1_MAIN_THREAD_STATUS = true;
                        _LOADING_STATUS = true;
                    }
                    //SHA256
                    if (algorithComboBox.SelectedIndex == 2)
                    {
                        progressConsole.Text += "#> SHA256 Selected..." + Environment.NewLine;
                        
                        _SHA256_THREAD_MAIN = new Thread(() => crackSHA256());
                        _SHA256_THREAD_MAIN.Start();
                        _LOADING_STATUS = true;
                        _SHA1_MAIN_THREAD_STATUS = true;

                    }
                    //NTLM
                    if (algorithComboBox.SelectedIndex == 3)
                    {
                        progressConsole.Text += "#> NTLM Selected..." + Environment.NewLine;
                        _NTLM_THREAD_MAIN = new Thread(() => crackNTLM());
                        _NTLM_THREAD_MAIN.Start();
                        _NTLM_MAIN_THREAD_STATUS = true;
                        _LOADING_STATUS = true;
                    }
                }
                else
                {
                    scroll();
                    statusBox.Visible = true;
                    statusBox.Image = global::PasswordCracker.Properties.Resources.errorStatus;
                    progressConsole.Text += "#> ERROR Please Select An Hash Algorith" + Environment.NewLine;

                }

            }
            else
            {
                scroll();
                statusBox.Visible = true;
                statusBox.Image = global::PasswordCracker.Properties.Resources.errorStatus;
                progressConsole.Text += "#> ERROR Hash Box Is Empty" + Environment.NewLine;

            }
        }
        #endregion

        // ################################################################### //
        // ##########      Brute Force Stage & File Digest Region    ######### //
        // ################################################################### //
        #region 
        private void crackMD5()
        {
            string passwordFromFile = "";
            bool foundMatch = false;
            Thread thisThread = Thread.CurrentThread;

            progressConsole.Text += "#> Beginning Password File Ingest." + Environment.NewLine;

            using (FileStream fs = File.Open(_IMPORTED_PASSWORDLIST, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader file = new StreamReader(bs))
            {
                progressConsole.Text += "#> Finished Password File Ingest." + Environment.NewLine;
                progressConsole.Text += "#> Beginning Brute Force Attack" + Environment.NewLine;
                scroll();
                while (foundMatch == false && (passwordFromFile = file.ReadLine()) != null)
                {

                    if (compute(passwordFromFile).ToUpper() == _HASH_TO_CRACK || compute(passwordFromFile).ToLower() == _HASH_TO_CRACK)
                    {
                        statusBox.Visible = true;
                        statusBox.Image = global::PasswordCracker.Properties.Resources.successStatus;
                        progressConsole.Text += "#> Success Password was found" + Environment.NewLine;
                        passwordFoundTextBox.Text = passwordFromFile;
                        scroll();
                        file.Close();
                        _LOADING_STATUS = false;
                        foundMatch = true;
                        
                        thisThread.Join();
                    }
                    else
                    {
                        
                        foundMatch = false;
                    }

                }
                if(foundMatch == false)
                {
                    statusBox.Visible = true;
                    statusBox.Image = global::PasswordCracker.Properties.Resources.errorStatus;
                    progressConsole.Text += "#> Error No Password Was Found" + Environment.NewLine;
                    scroll();
                }
                
                file.Close();
                thisThread.Join();
                _LOADING_STATUS = false;
            }

        }
        private void crackSHA1()
        {
            string passwordFromFile = "";
            bool foundMatch = false;
            Thread thisThread = Thread.CurrentThread;

            progressConsole.Text += "#> Beginning Password File Ingest." + Environment.NewLine;

            using (FileStream fs = File.Open(_IMPORTED_PASSWORDLIST, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader file = new StreamReader(bs))
            {
                progressConsole.Text += "#> Finished Password File Ingest." + Environment.NewLine;
                progressConsole.Text += "#> Beginning Brute Force Attack" + Environment.NewLine;

                scroll();

                while (foundMatch == false && (passwordFromFile = file.ReadLine()) != null)
                {

                    if (computeSHA1Hash(passwordFromFile).ToUpper() == _HASH_TO_CRACK || computeSHA1Hash(passwordFromFile).ToLower() == _HASH_TO_CRACK)
                    {
                        statusBox.Visible = true;
                        statusBox.Image = global::PasswordCracker.Properties.Resources.successStatus;
                        progressConsole.Text += "#> Success Password was found" + Environment.NewLine ;
                        passwordFoundTextBox.Text = "";
                        passwordFoundTextBox.Text = passwordFromFile;
                        scroll();
                        file.Close();
                        _LOADING_STATUS = false;
                        foundMatch = true;                  
                        thisThread.Join();
                    }
                    else
                    {
                        
                        foundMatch = false;
                    }

                }
                if (foundMatch == false)
                {
                    statusBox.Visible = true;
                    statusBox.Image = global::PasswordCracker.Properties.Resources.errorStatus;
                    progressConsole.Text += "#> Error No Password Was Found" + Environment.NewLine;
                    scroll();
                }
                
                file.Close();
                thisThread.Join();
                _LOADING_STATUS = false;
            }

        }
        private void crackSHA256()
        {
            string passwordFromFile = "";
            bool foundMatch = false;
            Thread thisThread = Thread.CurrentThread;

            progressConsole.Text += "#> Beginning Password File Ingest." + Environment.NewLine;

            using (FileStream fs = File.Open(_IMPORTED_PASSWORDLIST, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader file = new StreamReader(bs))
            {
                progressConsole.Text += "#> Finished Password File Ingest." + Environment.NewLine;
                progressConsole.Text += "#> Beginning Brute Force Attack" + Environment.NewLine;
                scroll();

                while (foundMatch == false && (passwordFromFile = file.ReadLine()) != null)
                {

                    if (computeSHA256Hash(passwordFromFile).ToUpper() == _HASH_TO_CRACK || computeSHA256Hash(passwordFromFile).ToLower() == _HASH_TO_CRACK)
                    {
                        statusBox.Visible = true;
                        statusBox.Image = global::PasswordCracker.Properties.Resources.successStatus;
                        progressConsole.Text += "#> Success Password was found" + Environment.NewLine;
                        passwordFoundTextBox.Text = "";
                        passwordFoundTextBox.Text = passwordFromFile;
                        scroll();
                        file.Close();
                        _LOADING_STATUS = false;
                        foundMatch = true;
                        thisThread.Join();
                    }
                    else
                    {
                        
                        foundMatch = false;
                    }

                }
                if (foundMatch == false)
                {
                    statusBox.Visible = true;
                    statusBox.Image = global::PasswordCracker.Properties.Resources.errorStatus;
                    progressConsole.Text += "#> Error No Password Was Found" + Environment.NewLine;
                    scroll();
                }
                file.Close();
                thisThread.Join();
                _LOADING_STATUS = false;
            }

        }
        private void crackNTLM()
        {
            string passwordFromFile = "";
            bool foundMatch = false;
            Thread thisThread = Thread.CurrentThread;

            progressConsole.Text += "#> Beginning Password File Ingest." + Environment.NewLine;

            using (FileStream fs = File.Open(_IMPORTED_PASSWORDLIST, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader file = new StreamReader(bs))
            {
                progressConsole.Text += "#> Finished Password File Ingest." + Environment.NewLine;
                progressConsole.Text += "#> Beginning Brute Force Attack" + Environment.NewLine;

                scroll();

                while (foundMatch == false && (passwordFromFile = file.ReadLine()) != null)
                {

                    if (computeNTLMHash(passwordFromFile).ToUpper() == _HASH_TO_CRACK || computeNTLMHash(passwordFromFile).ToLower() == _HASH_TO_CRACK)
                    {
                        
                        statusBox.Visible = true;
                        statusBox.Image = global::PasswordCracker.Properties.Resources.successStatus;
                        passwordFoundTextBox.Text = "";
                        passwordFoundTextBox.Text = passwordFromFile;
                        progressConsole.Text += "#> Success Password was found" + Environment.NewLine;
                        scroll();
                        file.Close();
                        foundMatch = true;
                        thisThread.Join();
                    }
                    else
                    {
                        
                        foundMatch = false;
                    }

                }
                if (foundMatch == false)
                {
                    
                    statusBox.Visible = true;
                    statusBox.Image = global::PasswordCracker.Properties.Resources.errorStatus;
                    progressConsole.Text += "#> Error No Password Was Found" + Environment.NewLine;
                    scroll();
                }
               
                file.Close(); 
                thisThread.Join();
                _LOADING_STATUS = false;
            }

        }
        #endregion

        // ################################################################### //
        // ##########            Generate Hash Region                ######### //
        // ################################################################### //
        #region 
        public static string computeNTLMHash(string passwordFromFile)
        {
            // IMPORTANT !
            // Credits to Mustafa Chelik (Original Code in c/c++)

            const uint INIT_A = 0x67452301;
            const uint INIT_B = 0xefcdab89;
            const uint INIT_C = 0x98badcfe;
            const uint INIT_D = 0x10325476;

            const uint SQRT_2 = 0x5a827999;
            const uint SQRT_3 = 0x6ed9eba1;

            char[] itoa16 = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

            uint[] nt_buffer = new uint[16];
            uint[] output = new uint[4];
            char[] hex_format = new char[32];


            int i = 0;
            int length = passwordFromFile.Length;

            for (; i < length / 2; i++)
            {
                nt_buffer[i] = (passwordFromFile[2 * i] | ((uint)passwordFromFile[2 * i + 1] << 16));
            }


            if (length % 2 == 1)
            {
                nt_buffer[i] = (uint)passwordFromFile[length - 1] | 0x800000;
            }
            else
            {
                nt_buffer[i] = 0x80;
            }


            nt_buffer[14] = (uint)length << 4;


            uint a = INIT_A;
            uint b = INIT_B;
            uint c = INIT_C;
            uint d = INIT_D;


            a += (d ^ (b & (c ^ d))) + nt_buffer[0]; a = (a << 3) | (a >> 29);
            d += (c ^ (a & (b ^ c))) + nt_buffer[1]; d = (d << 7) | (d >> 25);
            c += (b ^ (d & (a ^ b))) + nt_buffer[2]; c = (c << 11) | (c >> 21);
            b += (a ^ (c & (d ^ a))) + nt_buffer[3]; b = (b << 19) | (b >> 13);

            a += (d ^ (b & (c ^ d))) + nt_buffer[4]; a = (a << 3) | (a >> 29);
            d += (c ^ (a & (b ^ c))) + nt_buffer[5]; d = (d << 7) | (d >> 25);
            c += (b ^ (d & (a ^ b))) + nt_buffer[6]; c = (c << 11) | (c >> 21);
            b += (a ^ (c & (d ^ a))) + nt_buffer[7]; b = (b << 19) | (b >> 13);

            a += (d ^ (b & (c ^ d))) + nt_buffer[8]; a = (a << 3) | (a >> 29);
            d += (c ^ (a & (b ^ c))) + nt_buffer[9]; d = (d << 7) | (d >> 25);
            c += (b ^ (d & (a ^ b))) + nt_buffer[10]; c = (c << 11) | (c >> 21);
            b += (a ^ (c & (d ^ a))) + nt_buffer[11]; b = (b << 19) | (b >> 13);

            a += (d ^ (b & (c ^ d))) + nt_buffer[12]; a = (a << 3) | (a >> 29);
            d += (c ^ (a & (b ^ c))) + nt_buffer[13]; d = (d << 7) | (d >> 25);
            c += (b ^ (d & (a ^ b))) + nt_buffer[14]; c = (c << 11) | (c >> 21);
            b += (a ^ (c & (d ^ a))) + nt_buffer[15]; b = (b << 19) | (b >> 13);


            a += ((b & (c | d)) | (c & d)) + nt_buffer[0] + SQRT_2; a = (a << 3) | (a >> 29);
            d += ((a & (b | c)) | (b & c)) + nt_buffer[4] + SQRT_2; d = (d << 5) | (d >> 27);
            c += ((d & (a | b)) | (a & b)) + nt_buffer[8] + SQRT_2; c = (c << 9) | (c >> 23);
            b += ((c & (d | a)) | (d & a)) + nt_buffer[12] + SQRT_2; b = (b << 13) | (b >> 19);

            a += ((b & (c | d)) | (c & d)) + nt_buffer[1] + SQRT_2; a = (a << 3) | (a >> 29);
            d += ((a & (b | c)) | (b & c)) + nt_buffer[5] + SQRT_2; d = (d << 5) | (d >> 27);
            c += ((d & (a | b)) | (a & b)) + nt_buffer[9] + SQRT_2; c = (c << 9) | (c >> 23);
            b += ((c & (d | a)) | (d & a)) + nt_buffer[13] + SQRT_2; b = (b << 13) | (b >> 19);

            a += ((b & (c | d)) | (c & d)) + nt_buffer[2] + SQRT_2; a = (a << 3) | (a >> 29);
            d += ((a & (b | c)) | (b & c)) + nt_buffer[6] + SQRT_2; d = (d << 5) | (d >> 27);
            c += ((d & (a | b)) | (a & b)) + nt_buffer[10] + SQRT_2; c = (c << 9) | (c >> 23);
            b += ((c & (d | a)) | (d & a)) + nt_buffer[14] + SQRT_2; b = (b << 13) | (b >> 19);

            a += ((b & (c | d)) | (c & d)) + nt_buffer[3] + SQRT_2; a = (a << 3) | (a >> 29);
            d += ((a & (b | c)) | (b & c)) + nt_buffer[7] + SQRT_2; d = (d << 5) | (d >> 27);
            c += ((d & (a | b)) | (a & b)) + nt_buffer[11] + SQRT_2; c = (c << 9) | (c >> 23);
            b += ((c & (d | a)) | (d & a)) + nt_buffer[15] + SQRT_2; b = (b << 13) | (b >> 19);


            a += (d ^ c ^ b) + nt_buffer[0] + SQRT_3; a = (a << 3) | (a >> 29);
            d += (c ^ b ^ a) + nt_buffer[8] + SQRT_3; d = (d << 9) | (d >> 23);
            c += (b ^ a ^ d) + nt_buffer[4] + SQRT_3; c = (c << 11) | (c >> 21);
            b += (a ^ d ^ c) + nt_buffer[12] + SQRT_3; b = (b << 15) | (b >> 17);

            a += (d ^ c ^ b) + nt_buffer[2] + SQRT_3; a = (a << 3) | (a >> 29);
            d += (c ^ b ^ a) + nt_buffer[10] + SQRT_3; d = (d << 9) | (d >> 23);
            c += (b ^ a ^ d) + nt_buffer[6] + SQRT_3; c = (c << 11) | (c >> 21);
            b += (a ^ d ^ c) + nt_buffer[14] + SQRT_3; b = (b << 15) | (b >> 17);

            a += (d ^ c ^ b) + nt_buffer[1] + SQRT_3; a = (a << 3) | (a >> 29);
            d += (c ^ b ^ a) + nt_buffer[9] + SQRT_3; d = (d << 9) | (d >> 23);
            c += (b ^ a ^ d) + nt_buffer[5] + SQRT_3; c = (c << 11) | (c >> 21);
            b += (a ^ d ^ c) + nt_buffer[13] + SQRT_3; b = (b << 15) | (b >> 17);

            a += (d ^ c ^ b) + nt_buffer[3] + SQRT_3; a = (a << 3) | (a >> 29);
            d += (c ^ b ^ a) + nt_buffer[11] + SQRT_3; d = (d << 9) | (d >> 23);
            c += (b ^ a ^ d) + nt_buffer[7] + SQRT_3; c = (c << 11) | (c >> 21);
            b += (a ^ d ^ c) + nt_buffer[15] + SQRT_3; b = (b << 15) | (b >> 17);

            output[0] = a + INIT_A;
            output[1] = b + INIT_B;
            output[2] = c + INIT_C;
            output[3] = d + INIT_D;


            for (i = 0; i < 4; i++)
            {
                int j = 0;
                uint n = output[i];
                for (; j < 4; j++)
                {
                    uint convert = n % 256;
                    hex_format[i * 8 + j * 2 + 1] = itoa16[convert % 16];
                    convert = convert / 16;
                    hex_format[i * 8 + j * 2 + 0] = itoa16[convert % 16];
                    n = n / 256;
                }
            }

            return string.Join(string.Empty, hex_format);
        }

        private static string computeSHA256Hash(string passwordFromFile)
        {
            if (String.IsNullOrEmpty(passwordFromFile))
            {
                return String.Empty;
            }

            using (var sha = new SHA256Managed())
            {
                byte[] textData = Encoding.UTF8.GetBytes(passwordFromFile);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }

        }

        private static string compute(string passwordFromFile)
        {
            string md5Return = "";
            using (MD5 md5 = MD5.Create())
            {
                byte[] passwordByteArray = Encoding.ASCII.GetBytes(passwordFromFile);
                byte[] hashBytes = md5.ComputeHash(passwordByteArray);

                md5Return = ByteArrayToString(hashBytes);
                return md5Return;
            }
        }

        private static string computeSHA1Hash(string passwordFromFile)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(passwordFromFile));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString());
                }

                return sb.ToString();
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        #endregion

        // ################################################################### //
        // ##########       Form Animation & Graphics Region         ######### //
        // ################################################################### //
        #region 
        private void initMainFont()
        {
            _PRIV_FONT_MEMORY = new PrivateFontCollection();

            int fontLength = Properties.Resources.vt323.Length;
            byte[] fontdata = Properties.Resources.vt323;


            IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);

            _PRIV_FONT_MEMORY.AddMemoryFont(data, fontLength);
            _MAIN_UI_FONT = new Font(_PRIV_FONT_MEMORY.Families[0], 18.0F);
            progressConsole.Font = new Font(_PRIV_FONT_MEMORY.Families[0], 10.0F);
            locateFile.Font = new Font(_PRIV_FONT_MEMORY.Families[0], 12.0F);
        }
        private void onLoad()
        {
            this.DoubleBuffered = true;
            initMainFont();
            hashTextBox.Font = _MAIN_UI_FONT;
            selectedFileTextBox.Font = _MAIN_UI_FONT;
            algorithComboBox.Font = _MAIN_UI_FONT;
            passwordFoundTextBox.Font = _MAIN_UI_FONT;

            statusBox.Visible = false;

            _launch.Visible = false;
            _abort.Visible = false;
        }
        private void _launch_MouseLeave(object sender, EventArgs e)
        {
            _launch.Visible = false;
            _launchOnLoad.Visible = true;
        }
        private void _launchOnLoad_MouseEnter(object sender, EventArgs e)
        {
            _launch.Visible = true;
            _launchOnLoad.Visible = false;
        }
        private void _abort_MouseLeave(object sender, EventArgs e)
        {
            _abort.Visible = false;
            _abortOnLoad.Visible = true;
        }
        private void _abortOnLoad_MouseEnter(object sender, EventArgs e)
        {
            _abort.Visible = true;
            _abortOnLoad.Visible = false;
        }
        private void diableUI()
        {
            locateFile.Enabled = false;
            algorithComboBox.Enabled = false;
            _launch.Enabled = false;
            _abort.Enabled = false;
            hashTextBox.Enabled = false;
            algorithComboBox.Enabled = false;
        }
        private void enableUI()
        {
            locateFile.Enabled = true;
            algorithComboBox.Enabled = true;
            _launch.Enabled = true;
            _abort.Enabled = true;
            hashTextBox.Enabled = true;
            algorithComboBox.Enabled = true;
        }
        private void loadingLoop()
        {
            while (true)
            {
                if (_LOADING_STATUS == true)
                {
                    loadingGIF.Visible = true;
                }
                else
                {
                    loadingGIF.Visible = false;
                }
                Thread.Sleep(100);
            }
        }
        #endregion

        // ################################################################### //
        // ##########                   System Check                 ######### //
        // ################################################################### //
        #region    
        private void CPUCheck()
        {
            CPUModel();
            CPUArch();
            

            progressConsole.Text += $"#> Working Out Thread Count... {Environment.NewLine}";
            //Work Out how many threads based on CPU cores
            //Below line is an example
            CPUNumCores();
            progressConsole.Text += $"#> {_THREAD_COUNT} Theads Will Be Used... {Environment.NewLine}";
            progressConsole.Text += $"#> Program Loaded... {Environment.NewLine}";
            loadInstructions();
            enableUI(); 
            _LOADING_STATUS = false;
        }
        private void loadInstructions()
        {
            progressConsole.AppendText($"#> Enter a Valid Hash {Environment.NewLine}");
            progressConsole.AppendText($"#> Locate a Valid Password File {Environment.NewLine}");
            progressConsole.AppendText($"#> Select a Valid Algorithm To Match the Hash {Environment.NewLine}");
            scroll();

        }
        void scroll()
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                progressConsole.SelectionStart = progressConsole.Text.Length;
                progressConsole.ScrollToCaret();
            }));
        }
        private void CPUModel()
        {
            Task scrollOnLoad = new Task(new Action(scroll));
            scrollOnLoad.Start();
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    progressConsole.Text += $"#> Detected CPU: {queryObj["Name"]} {Environment.NewLine}";
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        private void CPUArch()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {

                    progressConsole.Text += $"#> Address Width: {queryObj["AddressWidth"]} bit {Environment.NewLine}";
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        public void CPUNumCores()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    progressConsole.Text += $"#> Number Of CPU Cores: {queryObj["NumberOfCores"]} {Environment.NewLine}";
                    _THREAD_COUNT = Convert.ToInt32(queryObj["NumberOfCores"]) * 2;


                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
               
            }

        }
        private void threadToFileCheck()
        {
            //Check if the current number of threads it too big for the file length
            if (_PASSWORD_FILE_LINE_COUNT / _THREAD_COUNT <= 1000)
            {
                _THREAD_COUNT = 1;
            }
            else if (_PASSWORD_FILE_LINE_COUNT / _THREAD_COUNT > 1000 && _PASSWORD_FILE_LINE_COUNT / _THREAD_COUNT <= 2000)
            {
                _THREAD_COUNT = 2; // File Split into 2 first half thread0 second halfthread1
            }
            else if (_PASSWORD_FILE_LINE_COUNT / _THREAD_COUNT > 2000 && _PASSWORD_FILE_LINE_COUNT / _THREAD_COUNT <= 3000)
            {
                _THREAD_COUNT = 3; // File Split into 3 first half thread0 second halfthread1 third to thread2
            }
            else
            {
                _THREAD_COUNT = 4; //Use 4 threads if file is over
            }
        }
        #endregion

        // ################################################################### //
        // ##########                   File Check                   ######### //
        // ################################################################### //
        #region
        private void checkFileLines()
        { 
            _PASSWORD_FILE_LINE_COUNT = 0;
            progressConsole.Text += $"#> File \"{_IMPORTED_PASSWORDLIST}\" Loaded {Environment.NewLine}";
            progressConsole.Text += $"#> Calculating File Length... {Environment.NewLine}";
            using (StreamReader r = new StreamReader(_IMPORTED_PASSWORDLIST))
            {
                while (r.ReadLine() != null)
                {
                    _PASSWORD_FILE_LINE_COUNT++;
                }
            }
            
            progressConsole.Text += $"#> File has {_PASSWORD_FILE_LINE_COUNT} Passwords {Environment.NewLine}";
            scroll();
        }
        #endregion

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_THREAD_RUNNING_STATUS == true && _MD5_MAIN_THREAD_STATUS == true)
            {
                _MD5_THREAD_MAIN.Join();
            }
            if (_THREAD_RUNNING_STATUS == true && _SHA1_MAIN_THREAD_STATUS == true)
            {
                _SHA1_THREAD_MAIN.Join();
            }
            if (_THREAD_RUNNING_STATUS == true && _SHA256_MAIN_THREAD_STATUS == true)
            {
                _SHA256_THREAD_MAIN.Join();
            }
            if (_THREAD_RUNNING_STATUS == true && _NTLM_MAIN_THREAD_STATUS == true)
            {
                _NTLM_THREAD_MAIN.Join();
            }
        }
    }

}
