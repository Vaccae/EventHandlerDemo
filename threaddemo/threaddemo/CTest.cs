using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace threaddemo
{
    public class CTest : Inftest,IDisposable
    {
        public int id;

        private AutoResetEvent exitEvent;

        private Thread thread;

        private int waitTime = 100;

        private bool disposed = false;

        private bool IsRunning;

        public int cs = 0;
        public CTest(int _id)
        {
            id = _id;
            exitEvent = new AutoResetEvent(false);
            thread = new Thread(ReadThreadRun);
            thread.IsBackground = true;
        }


        public event EventHandler<testEvent> DataReceived;

        public void Dispose()
        {
           // Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Stop();

                    }
                    catch (Exception)
                    {

                    }
                }
                disposed = true;
            }
        }

        public void Start()
        {
            IsRunning = true;
            thread.Start();
        }

        public void Stop()
        {
            IsRunning = false;
            exitEvent.Reset();
            exitEvent.Set();
            RaiseDataReceived("手动停止");
        }


        public void Reset()
        {
            exitEvent.Reset();
            exitEvent.Set();
        }

        public void setcs(int _cs)
        {
            cs = _cs;

        }


        private void RaiseDataReceived(string msg)
        {
            DataReceived?.Invoke(this, new testEvent(msg, id));
        }

        /// <summary>
        /// 接收线程
        /// </summary>
        private void ReadThreadRun()
        {
            while (IsRunning)
            {
                try
                {
                    if (exitEvent.WaitOne(waitTime))
                    {
                        if (IsRunning) {
                            Thread.Sleep(1000);
                            id++;
                            if (id % 5 == 0)
                            {
                                throw new Exception("余数：0");
                            }else if (id % 5 == 1)
                            {
                                try
                                {
                                    throw new Exception("故意出错");
                                }
                                catch(Exception ex)
                                {
                                    RaiseDataReceived(ex.Message);
                                    Reset();
                                }
                            }
                            RaiseDataReceived("状态：重启");
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            RaiseDataReceived("停止");
                        }
                    }

                    Random rd = new Random();
                    int count = rd.Next(0, 13);
                    if (count < 10)
                    {
                        RaiseDataReceived(count.ToString());   
                    }
                    else if (count == 10)
                    {
                        throw new Exception("throw 数字：" + count);
                    }
                    else
                    {
                        RaiseDataReceived("数字：" + count + " Reset");
                        Reset();
                    }
                }
                catch (Exception ex)
                {
                    RaiseDataReceived("error " + ex.Message);
                    Reset();
                }

                //Thread.Sleep(100);
            }
        }
    }
}
