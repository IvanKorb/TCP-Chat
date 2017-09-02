using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Server.DataModel;

namespace Server
{
    public partial class History : Form
    {
        private readonly Main Main;
        public History(Main main)
        {
            InitializeComponent();
            Main = main;
        }

   

        private void btnHistory_Click(object sender, EventArgs e)
        {
            //подключаюсь к бд и не пойму почему так не работает
            using (MessageContext db = new MessageContext())
            {
                foreach (var item in db.Messages)
                {

                    for (int i = 0; i < listMessage.Items.Count; i++)
                    {
                        listMessage.Items[i].SubItems[1].Text = item.UserName;
                        listMessage.Items[i].SubItems[2].Text = item.DateTimeMessage.ToString();
                        listMessage.Items[i].SubItems[3].Text = item.UserMessage;
                    }
                    
                }
            


            }
        }

        private void listMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }
    }
}
