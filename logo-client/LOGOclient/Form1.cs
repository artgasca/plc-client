using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Sharp7;

namespace LOGOclient
{
    public partial class Form1 : Form
    {
        private S7Client Client;
        private byte[] Buffer = new byte[65536];
        private bool[] Outputs = new bool[64];
        private bool[] Inputs = new bool[64];
        


        private void ShowResult(int Result)
        {
            // This function returns a textual explaination of the error code
            txt_box_error.Text = Client.ErrorText(Result);
            if (Result == 0)
                txt_box_error.Text = txt_box_error.Text + " (" + Client.ExecutionTime.ToString() + " ms)";
        }
        private void ReadOutputs()
        {
            int Result;
            int SizeRead = 0;
            Result = Client.ReadArea(S7Consts.S7AreaPA, 0, 0, 2, S7Consts.S7WLByte, Buffer,ref SizeRead);
            ShowResult(Result);
            lbl_bytes.Text = SizeRead.ToString();
            if (Result == 0) {
                Console.WriteLine("todo ok");
                for (int i = 0; i < 8; i++)
                {
                    Outputs[i] = GetBit(Buffer[0], i);
                }
                for (int i = 0; i < 8; i++)
                {
                    Outputs[i + 8] = GetBit(Buffer[1], i);
                }
            }

                
        }

        private void ReadInputs()
        {
            int Result;
            int SizeRead = 0;
            // Result = Client.ReadArea(S7Consts.S7AreaPA, 0, 0, 2, S7Consts.S7WLByte, Buffer, ref SizeRead);
            Result = Client.DBRead(1, 1024, 1, Buffer);
            ShowResult(Result);
            lbl_bytes.Text = SizeRead.ToString();
            if (Result == 0)
            {
                Console.WriteLine("todo ok");
                for (int i = 0; i < 8; i++)
                {
                    Inputs[i] = GetBit(Buffer[0], i);
                }
                for (int i = 0; i < 8; i++)
                {
                    Inputs[i + 8] = GetBit(Buffer[1], i);
                }
            }


        }
        private void WriteOutputs(int output,byte status)
        {
            Buffer[0] = status;
            int Result;
            
            Result = Client.WriteArea(S7Consts.S7AreaPA, 1, output, 1, S7Consts.S7WLBit, Buffer);
            if (Result ==0)
            {
                Console.WriteLine("escritura correcta");
            }
        }
        

        public Form1()
        {
            InitializeComponent();
            Client = new S7Client();
            btn_Q0.Click += new EventHandler(ActivateOutputEvent);
            btn_Q1.Click += new EventHandler(ActivateOutputEvent);
            btn_Q2.Click += new EventHandler(ActivateOutputEvent);
            btn_Q3.Click += new EventHandler(ActivateOutputEvent);
            btn_Q4.Click += new EventHandler(ActivateOutputEvent);
            btn_Q5.Click += new EventHandler(ActivateOutputEvent);
            btn_Q6.Click += new EventHandler(ActivateOutputEvent);
            btn_Q7.Click += new EventHandler(ActivateOutputEvent);
            btn_Q8.Click += new EventHandler(ActivateOutputEvent);
            btn_Q9.Click += new EventHandler(ActivateOutputEvent);
            btn_Q10.Click += new EventHandler(ActivateOutputEvent);
            btn_Q11.Click += new EventHandler(ActivateOutputEvent);
            //Aqui inizilizar elementos
        }

        void ActivateOutputEvent(object sender, EventArgs e)
        {
            Button buttonOut = sender as Button;
            int outputRelay =0;
            switch (buttonOut.Name)
            {
                case "btn_Q0":
                    outputRelay = 0;
                    
                    break;
                case "btn_Q1":
                    outputRelay = 1;
                    break;
                    
                case "btn_Q2":
                    outputRelay = 2;
                    break;
                case "btn_Q3":
                    outputRelay = 3;
                    break;
                case "btn_Q4":
                    outputRelay = 4;
                    break;
                case "btn_Q5":
                    outputRelay = 5;
                    break;
                case "btn_Q6":
                    outputRelay = 6;
                    break;
                case "btn_Q7":
                    outputRelay = 7;
                    break;
                case "btn_Q8":
                    outputRelay = 8;
                    break;
                case "btn_Q9":
                    outputRelay = 9;
                    break;
                case "btn_Q10":
                    outputRelay = 10;
                    break;
                case "btn_Q11":
                    outputRelay = 11;
                    break;

            }

            if (buttonOut.Text == "OFF")
            {
                WriteOutputs(outputRelay, 1);

            }
            else if (buttonOut.Text == "ON")
            {
                WriteOutputs(outputRelay, 0);
            }
            Console.WriteLine("listo");
            Thread.Sleep(250);
            ReadOutputs();
            SetOutButtons();
        }
       
        private void btn_conectar_Click(object sender, EventArgs e)
        {
            int Result;
            //int Rack = 0;
            //int Slot = 2;
            //ushort tsap_local = System.Convert.ToUInt16(txt_box_tsap_local.Text);
            //ushort tsap_remote = System.Convert.ToUInt16(txt_box_tsap_remote.Text);

            //Client.SetConnectionParams(txt_box_ip.Text, tsap_local, tsap_remote);
            Client.SetConnectionParams(txt_box_ip.Text, 0x1000, 0x0200);
            Result = Client.Connect();
            ShowResult(Result);
            if (Result == 0)
            {
                txt_box_error.Text = txt_box_error.Text + "PDU NEgotiated : " + Client.PduSizeNegotiated.ToString();
                txt_box_ip.Enabled = false;
                tabControl_connectionType.Enabled = false;
                groupBox_main.Enabled = true;
                btn_desconectar.Enabled = true;
                btn_conectar.Enabled = false;
                Thread.Sleep(500);
                ReadOutputs();
                SetOutButtons();

            }
        }
        
        private void label1_Click(object sender, EventArgs e)
        {
           
        }
        
        private void btn_desconectar_Click(object sender, EventArgs e)
        {
            Client.Disconnect();
            txt_box_error.Text = "Disconnected";
            txt_box_ip.Enabled = true;
            tabControl_connectionType.Enabled = true;
            btn_desconectar.Enabled = false;
            groupBox_main.Enabled = false;
            btn_conectar.Enabled = true;
        }

        private void btn_refreshOutputs_Click(object sender, EventArgs e)
        {
            ReadOutputs();
            SetOutButtons();

        }
        private void SetOutButtons()
        {
            Button[] btns =
             {
                btn_Q0,
                btn_Q1,
                btn_Q2,
                btn_Q3,
                btn_Q4,
                btn_Q5,
                btn_Q6,
                btn_Q7,
                btn_Q8,
                btn_Q9,
                btn_Q10,
                btn_Q11,
            };
            for (int i = 0; i < 12; i++)
            {
                if (Outputs[i])
                {
                    btns[i].Text = "ON";
                }else
                {
                    btns[i].Text = "OFF";
                }
            }
        }

        private void SetInputs()
        {
            CheckBox[] entradas =
             {
                cb_input1,
                cb_input2,
                cb_input3,
                cb_input4,
                cb_input5,
                cb_input6,
                cb_input7,
                cb_input8,
              
            };
            for (int i = 0; i < 8; i++)
            {
                if (Inputs[i])
                {
                    
                    entradas[i].Checked = true;
                   
                }
                else
                {
                    entradas[i].Checked = false;
                }
            }
        }

        public static bool GetBit( byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        private void btn_refreshInputs_Click(object sender, EventArgs e)
        {
            ReadInputs();
            SetInputs();
        }

        private void btn_UpdateSensor_Click(object sender, EventArgs e)
        {
            int Result;
            int SizeRead = 0;
            int value;
            //Result = Client.ReadArea(S7Consts.S7AreaPA, 0, 0, 2, S7Consts.S7WLByte, Buffer, ref SizeRead);
            Result = Client.DBRead(1, 1118, 2, Buffer);
            ShowResult(Result);
            value = Buffer[1] + (Buffer[0] << 8);
            lbl_sensorValue.Text = value.ToString();


            
        }

        private void btn_setPoint_Click(object sender, EventArgs e)
        {
            int Result;
            int value;

            value = Int16.Parse(txt_box_setPoint.Text);
            Buffer[0] = (byte)(value >> 8);
            Buffer[1] = (byte)(value & 0xff);
            //Console.WriteLine("listo");

            Result = Client.DBWrite(1, 1120, 2, Buffer);
            //Result = Client.WriteArea(S7Consts.S7AreaDB, 1, 1119, 2, S7Consts.S7WLWord, Buffer);
            ShowResult(Result);
        }

        private void btn_startProcess_Click(object sender, EventArgs e)
        {
            int Result;                    
            Buffer[1] = 0x01;
     

            Result = Client.DBWrite(1, 1110, 2, Buffer);
            //Result = Client.WriteArea(S7Consts.S7AreaDB,1,1)
            ShowResult(Result);

        }

        private void btnUpdate485_Click(object sender, EventArgs e)
        {
            int Result;
            int SizeRead = 0;
            int value;
            //Result = Client.ReadArea(S7Consts.S7AreaPA, 0, 0, 2, S7Consts.S7WLByte, Buffer, ref SizeRead);
            Result = Client.DBRead(1, 0, 2, Buffer);
            ShowResult(Result);
            value = Buffer[1] + (Buffer[0] << 8);
            lbl_data1.Text = value.ToString();

            Result = Client.DBRead(1, 10, 2, Buffer);
            ShowResult(Result);
            value = Buffer[1] + (Buffer[0] << 8);
            lbl_data2.Text = value.ToString();

            Result = Client.DBRead(1, 20, 2, Buffer);
            ShowResult(Result);
            value = Buffer[1] + (Buffer[0] << 8);
            lbl_data3.Text = value.ToString();
        }

        private void btn_Q0_Click(object sender, EventArgs e)
        {

        }

        private void btn_update_rtb_Click(object sender, EventArgs e)
        {
            int Result;
            int SizeRead = 0;
            int value;
            float valor;
            byte[] datos = new byte[4];
            //Result = Client.ReadArea(S7Consts.S7AreaPA, 0, 0, 2, S7Consts.S7WLByte, Buffer, ref SizeRead);
            Result = Client.DBRead(1,5228,4 , Buffer);
            ShowResult(Result);

        
            Array.Copy(Buffer, 0, datos, 0, 4); // Copia desde posicion 0, 4 bytes
            //Invertir para formato Little Endian (s7-200 usa big Endian)
            Array.Reverse(datos);

            valor = BitConverter.ToSingle(datos, 0);

            Console.WriteLine("Valor float: " + valor);
            lbl_rtb_arena.Text = valor.ToString();




            Result = Client.DBRead(1, 5200, 4, Buffer);
            ShowResult(Result);


            Array.Copy(Buffer, 0, datos, 0, 4); // Copia desde posicion 0, 4 bytes
            //Invertir para formato Little Endian (s7-200 usa big Endian)
            Array.Reverse(datos);

            valor = BitConverter.ToSingle(datos, 0);

            Console.WriteLine("Valor float: " + valor);
            lbl_rtb_agua.Text = valor.ToString();




            Result = Client.DBRead(1, 5204, 4, Buffer);
            ShowResult(Result);


            Array.Copy(Buffer, 0, datos, 0, 4); // Copia desde posicion 0, 4 bytes
            //Invertir para formato Little Endian (s7-200 usa big Endian)
            Array.Reverse(datos);

            valor = BitConverter.ToSingle(datos, 0);

            Console.WriteLine("Valor float: " + valor);
            lbl_rtb_cem.Text = valor.ToString();






        }
    }
}
