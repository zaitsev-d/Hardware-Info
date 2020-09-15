using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Management;
using System.Drawing;

namespace HardInfo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void GetHardwareInfo(string key, ListView list)
        {
            list.Items.Clear();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + key);

            try
            {
                foreach(ManagementObject obj in searcher.Get())
                {
                    ListViewGroup listViewGroup;

                    try
                    {
                        listViewGroup = list.Groups.Add(obj["Name"].ToString(), obj["Name"].ToString());
                    }
                    catch
                    {
                        listViewGroup = list.Groups.Add(obj.ToString(), obj.ToString());
                    }

                    if(obj.Properties.Count == 0)
                    {
                        MessageBox.Show("Could not get information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach(PropertyData data in obj.Properties)
                    {
                        ListViewItem item = new ListViewItem(listViewGroup);

                        if (list.Items.Count % 2 != 0) item.BackColor = Color.White;
                        else item.BackColor = Color.WhiteSmoke;

                        item.Text = data.Name;

                        if(data.Value != null && !string.IsNullOrEmpty(data.Value.ToString()))
                        {
                            switch(data.Value.GetType().ToString())
                            {
                                case "System.String[]":

                                    string[] stringData = data.Value as string[];
                                    string resStr1 = string.Empty;

                                    foreach(string s in stringData)
                                    {
                                        resStr1 += $"{s} ";
                                    }
                                    item.SubItems.Add(resStr1);

                                    break;

                                case "System.UInt16[]":

                                    ushort[] ushortData = data.Value as ushort[];
                                    string resStr2 = string.Empty;

                                    foreach(ushort u in ushortData)
                                    {
                                        resStr2 += $"{Convert.ToString(u)} ";
                                    }

                                    item.SubItems.Add(resStr2);

                                    break;

                                default:

                                    item.SubItems.Add(data.Value.ToString());

                                    break;
                            }

                            list.Items.Add(item);
                        }
                    }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = string.Empty;

            switch(toolStripComboBox1.SelectedItem.ToString())
            {
                case "Processor":
                    key = "Win32_Processor";
                    break;
                case "Graphics Card":
                    key = "Win32_VideoController";
                    break;
                case "Chipset":
                    key = "Win32_IDEController";
                    break;
                case "Battery":
                    key = "Win32_Battery";
                    break;
                case "BIOS":
                    key = "Win32_BIOS";
                    break;
                case "RAM":
                    key = "Win32_PhysicalMemory";
                    break;
                case "Cache":
                    key = "Win32_CacheMemory";
                    break;
                case "USB":
                    key = "Win32_USBController";
                    break;
                case "Disk":
                    key = "Win32_DiskDrive";
                    break;
                case "Logical Disks":
                    key = "Win32_LogicalDisk";
                    break;
                case "Keyboard":
                    key = "Win32_Keyboard";
                    break;
                case "Network":
                    key = "Win32_NetworkAdapter";
                    break;
                case "Users":
                    key = "Win32_Account";
                    break;
                default:
                    key = "Win32_Processor";
                    break;
            }

            GetHardwareInfo(key, listView1);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            toolStripComboBox1.SelectedIndex = 0;
        }
    }
}
