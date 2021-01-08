using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ITL.Framework
{
    public class UdpListener
    {
        private object f_Lock = new object();
        private string f_LocalIP = "";
        private int f_ListenPort;
        private readonly List<UdpClient> f_UDPClients = new List<UdpClient>();
        private Task f_RecvTask;
        private bool f_IsOpen = false;
        private bool f_IsStop;

        protected List<UdpClient> UDPClients
        {
            get
            {
                lock (f_Lock)
                {
                    return f_UDPClients;
                }
            }
        }
        protected Task RecvTask
        {
            get
            {
                lock (f_Lock)
                {
                    return f_RecvTask;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_RecvTask = value;
                }
            }
        }


        /// <summary>
        /// 监听是否已打开
        /// </summary>
        protected bool IsOpen
        {
            get
            {
                lock (f_Lock)
                {
                    return f_IsOpen;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_IsOpen = value;
                }
            }
        }

        /// <summary>
        /// 是否已停止
        /// </summary>
        protected bool IsStop
        {
            get
            {
                lock (f_Lock)
                {
                    return f_IsStop;
                }
            }
            set
            {
                lock (f_Lock)
                {
                    f_IsStop = value;
                }
            }
        }

        /// <summary>
        /// 数据接收回调
        /// </summary>
        public event Action<UdpClient, IPEndPoint, byte[]> RecvCallback;

        /// <summary>
        /// 根据ip和port启动udp
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public UdpListener(string ip, int port) : this(port)
        {
            f_LocalIP = ip;
        }

        /// <summary>
        /// 自动获取本机网卡地址并启动udp（支持多网卡）
        /// </summary>
        public UdpListener(int port)
        {
            f_IsStop = false;
            f_ListenPort = port;
        }


        //接收数据线程执行函数
        private void UdpClientReceive(UdpClient client)
        {
            try
            {
                if (client == null)
                {
                    return;
                }
                IPEndPoint endpoint = null;
                while (true)
                {
                    byte[] buf = client.Receive(ref endpoint);//获取从网络门禁机返回的数据包
                    if (IsStop)
                    {
                        return;
                    }
                    RecvCallback?.Invoke(client, endpoint, buf);
                }
            }
            catch (Exception e)
            {
                if (IsStop)
                {
                    return;
                }
                RunLog.Log(string.Format(" Exception:{0} StackTrace:{1},InnerException:{2},Message:{3}", e, e.StackTrace, e.InnerException, e.Message));
            }
        }
        //Udp广播发送数据
        private bool SendData(UdpClient client, IPEndPoint endpoint, byte[] data, ref string strErr)
        {
            strErr = "";

            if (endpoint == null)
            {
                strErr = "IPEndPoint cannot be null";
                return false;
            }
            if (!IsOpen)
            {
                strErr = "UdpClient is not opened";
                return false;
            }
            try
            {
                IsStop = false;
                int sendCount = client.Send(data, data.Length, endpoint);
                if (sendCount != data.Length)
                {
                    strErr = string.Format("send data:{0} falied!{2}", StrUtils.BytesToHexStr(data), UtilityTool.GetSysErrMsg());
                    return false;
                }

            }
            catch (Exception e)
            {
                strErr = string.Format("UdpClient({0}) send data failed:{1}", client?.Client?.LocalEndPoint, e.Message);
                return false;
            }
            return true;
        }

        //获取本机ip列表
        private List<string> GetLocalIPs()
        {
            List<string> ips = new List<string>();
            try
            {
                string name = Dns.GetHostName();
                IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
                foreach (IPAddress ipa in ipadrlist)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ips.Add(ipa.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                RunLog.Log(string.Format("Get local ip list failed! ex:{0}", RunLog.GetExceptionInfo(ex)));
            }
            return ips;
        }

        /// <summary>
        /// 启动UDP，可重复调用，不影响已启动Udp监听的网卡
        /// </summary>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public bool StartUdp(ref string strErr)
        {
            strErr = "";
            if (IsOpen)
            {
                return true;
            }
            try
            {
                List<string> localIps = this.GetLocalIPs();
                //IPEndPoint endpoint = null;
                //IPAddress address = string.IsNullOrWhiteSpace(f_LocalIP) ? IPAddress.Any : IPAddress.Parse(f_LocalIP);
                //endpoint = new IPEndPoint(address, f_ListenPort);
                localIps.ForEach((ip) =>
                {
                    UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), f_ListenPort));
                    UDPClients.Add(client);
                    ITL.ParamsSettingTool.AppEnv.Singleton.UdpCount += 1;

                    //启动相应UdpClient Receive监控线程
                    RecvTask = new Task(() =>
                    {
                        this.UdpClientReceive(client);

                    }, TaskCreationOptions.LongRunning);
                    RecvTask.Start();
                });
            }
            catch (Exception e)
            {
                strErr = string.Format("UDP client create failed: {0}", e.Message);
                return false;
            }
            IsOpen = true;
            return true;
        }

        /// <summary>
        /// 停止Udp监听
        /// </summary>
        public void StopUdp()
        {
            IsStop = true;
            IsOpen = false;
            UDPClients?.ForEach((udp) =>
            {
                udp.Client.Shutdown(SocketShutdown.Both);
                udp.Close();
            });
            UDPClients.Clear();
            RecvTask = null;
        }
        /// <summary>
        /// Udp发送数据
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public bool SendData(IPEndPoint endpoint, byte[] data, ref string strErr)
        {
            foreach (var client in UDPClients)
            {
                if (!SendData(client, endpoint, data, ref strErr))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Udp发送数据（使用utf-8编码）
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public bool SendData(IPEndPoint endpoint, string hexData, ref string strErr)
        {
            strErr = "";
            if (!StrUtils.CheckIsHEXStr(hexData))
            {
                strErr = string.Format("{0} is not valid hex string!", hexData);
                return false;
            }
            byte[] buf = StrUtils.HexStrToBytes(hexData);

            foreach (var client in UDPClients)
            {
                if (!SendData(client, endpoint, buf, ref strErr))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
