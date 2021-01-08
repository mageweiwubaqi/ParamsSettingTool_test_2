using ITL.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace ITL.ParamsSettingTool
{
    public partial class WaitForm : Form
    {
        public WaitForm()
        {
            InitializeComponent();
            Oninit();
        }

        public void Oninit()
        {
            //   HintProvider.ShowAutoCloseDialog(null, string.Format("正在加载..."), HintIconType.OK, 3000);


            //  Thread.Sleep(3000);
           


            //System.Timers.Timer t = new System.Timers.Timer(3000);//设置间隔时间为10000毫秒；
            //t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//
            //t.AutoReset = false;//设置是执行一次（false）
            //t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }
        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            //  this.Hide();
            // this.DialogResult = DialogResult.OK;
            //this.Invoke(new Action(()=>this.Close()));
            // this.Close();
            //System.Timers.Timer t = new System.Timers.Timer(3000);//设置间隔时间为10000毫秒；
            //t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//
            //t.AutoReset = false;//设置是执行一次（false）
            //t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
          //  this.DialogResult = DialogResult.OK;
            // this.Close();
         //   this.Invoke(new Action(()=>this.Close()));

        }

        private void WaitForm_Load(object sender, EventArgs e)
        {
            //Thread.Sleep(3000);
            //this.DialogResult = DialogResult.OK;
            // this.Close();

            System.DateTime sTime = System.DateTime.Now;
            System.DateTime eTime = sTime.AddSeconds(5);
            while (sTime > eTime)
            {
                sTime = System.DateTime.Now;
               
                break;

            }

             this.DialogResult = DialogResult.OK;
                this.Close();

        }
    }
}
