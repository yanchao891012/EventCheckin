using EventCheckin.Common;
using EventCheckin.DB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YC.UCTool.MessageBoxs;
using YC.UtilTool;

namespace EventCheckin.View
{
    /// <summary>
    /// SalesManAddWin.xaml 的交互逻辑
    /// </summary>
    public partial class SalesManAddWin
    {
        public SalesManAddWin()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 原来的图片地址
        /// </summary>
        string oldFilePath = string.Empty;

        private void Btn_path_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "图片|*.jpg;*.png;*.jpeg";
                if (openFileDialog.ShowDialog() == true)
                {
                    oldFilePath = openFileDialog.FileName;
                    img.Source = ImageHelper.GetImageSourceFromString(oldFilePath);
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowInfoMessage("读取图片失败！\r\n失败原因：" + ex.ToString());
            }
        }

        private void Btn_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(oldFilePath) || string.IsNullOrEmpty(tb_name.Text) || string.IsNullOrEmpty(tb_tableno.Text))
                {
                    CustomMessageBox.ShowInfoMessage("图片、姓名、桌号缺一不可！");
                    return;
                }
                //文件夹不存在的话，创建文件夹
                if (!Directory.Exists(CommonValues.IMAGEPATH))
                {
                    Directory.CreateDirectory(CommonValues.IMAGEPATH);
                }
                //复制图片到指定的目录
                string imgName = Guid.NewGuid().ToString("N") + ".jpg";
                File.Copy(oldFilePath, CommonValues.IMAGEPATH + "\\" + imgName);

                DBHelper.InsertSalesMans(tb_name.Text.Trim(), imgName, tb_tableno.Text.Trim());

                CustomMessageBox.ShowInfoMessage("添加业务员成功！");
                oldFilePath = string.Empty;
                tb_name.Text = string.Empty;
                tb_tableno.Text = string.Empty;
                img.Source = null;
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowInfoMessage("添加业务员失败！\r\n失败原因：" + ex.ToString());
            }


        }
    }
}
