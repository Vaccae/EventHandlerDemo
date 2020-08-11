using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace threaddemo
{
    public class htTest
    {
        private ListBox listBox;
      
        private Hashtable _hstest = new Hashtable();


        public htTest(ListBox lv)
        {
            listBox = lv;
            listBox.Items.Clear();
        }

        /// <summary>
        /// 检测是否频繁读卡
        /// </summary>
        /// <param name="cardno"></param>
        /// <param name="waittime"></param>
        /// <returns></returns>
        public bool checkHashTest(CItem item, int waittime)
        {
            bool res = false;
            string name = item.name;
            //判断是否有卡号
            lock (_hstest)
            {
                if (_hstest.ContainsKey(name))
                {
                    item = (CItem)_hstest[name];
                    //判断是否在设置时间内
                    if (checkDatetime(item.lastdatetime, waittime))
                    {
                        res = true;
                    }
                    else
                    {
                        item.lastdatetime = DateTime.Now;
                        _hstest[name] = item;
                    }
                   
                }
                else
                {
                    item.lastdatetime = DateTime.Now;
                    _hstest.Add(name, item);
                    listBox.Items.Add(name);
                }
            }
            return res;
        }


        /// 检测时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public bool checkDatetime(DateTime timestamp, int millisecond)
        {
            bool res = false;
            //判断时间相减
            TimeSpan ts = timestamp - DateTime.Now;
            //判断通讯时间差值
            double ds = Math.Abs(ts.TotalMilliseconds);

            if (ds <= millisecond)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 删除会员哈希表信息
        /// </summary>
        /// <param name="carNumber"></param>
        public void removeHashCardInfo(string name)
        {
            lock (_hstest)
            {
                if (_hstest.ContainsKey(name))
                {
                    _hstest.Remove(name);
                    listBox.Items.Remove(name);
                }
            }
        }

        public CItem getHashitem(string name)
        {
            CItem item = null;
            lock (_hstest)
            {
                if (_hstest.ContainsKey(name))
                {
                    item = (CItem)_hstest[name];
                }
            }
            return item;
        }


        /// <summary>
        /// 清理HashTable
        /// </summary>
        /// <param name="millisecond"></param>
        public void RemoveHashTableFromTime(int millisecond)
        {
            List<string> removelst = new List<string>();
            lock (_hstest)
            {
                foreach (DictionaryEntry de in _hstest)
                {
                    CItem item = (CItem)de.Value;
                    if (checkDatetime(item.lastdatetime, millisecond))
                    {
                        removelst.Add((string)de.Key);
                    }
                }
                removelst.ForEach(p =>
                {
                    _hstest.Remove(p);
                    listBox.Items.Remove(p);
                });
            }



          
        }


    }
}
