/***************************************************************************
 * 
 * 创建时间：   2018/3/9 星期五 12:30:37
 * 创建人员：   沈瑞
 * CLR版本号：  4.0.30319.42000
 * 备注信息：   未填写备注信息
 * 
 * *************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LogDelete
{

    /// <summary>
    /// 提供一种一致的消息处理能力，这个可以控制处理消息的线程数量，任务可以在多个线程中同步执行，一般消息互不干扰的情况下可以使用该类
    /// 该类是线程安全的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageQueueMultiple<T>
    {

        ConcurrentQueue<T> mq = new ConcurrentQueue<T>();
        //状态维持对象
        private long _isDisposed = 0;
        Semaphore _semaphore;

        #region 构造方法

        /// <summary>
        /// 实例化对象
        /// </summary>
        public MessageQueueMultiple()
            : this(Environment.ProcessorCount * 2)
        {
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        public MessageQueueMultiple(int maxRunThreadCount)
        {
            MessageComing = (S, E) => { };
            _semaphore = new Semaphore(maxRunThreadCount, maxRunThreadCount);
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        public EventHandler Disposed = (S, E) => { };
        /// <summary>
        /// 
        /// </summary>
        public Action<MessageQueueMultiple<T>, T> MessageComing { get; set; } = (s, e) => { };

        public bool IsRunning => true;

        /// <summary>
        /// 触发实例被释放前通知事件
        /// </summary>
        protected virtual void OnDisposing(object sender, EventArgs e)
        {
            Disposed(sender, e);
        }
        /// <summary>
        /// 触发新消息入队处理处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">所包含的事件参数对象</param>
        protected virtual void OnMessageComing(object sender, T e)
        {
            this.MessageComing(this, e);
        }

        //提供给线程池调用的方法
        void RunHandleProcess(object obj)
        {
            try
            {
                TryRunHandleProcess();
            }
            catch (Exception ex)
            {
                //UyiSystemErrorProvide.OnUyiSystemErrorHandleEvent(this, ex);
            }
            finally
            {
                _semaphore.Release();
                if (!mq.IsEmpty)
                {
                    StartProcess();
                }
            }
        }

        private void TryRunHandleProcess()
        {
            while (!mq.IsEmpty)
            {
                T msg;
                while (mq.TryDequeue(out msg))
                {
                    OnMessageComing(this, msg);
                }
            }
        }

        private void StartProcess()
        {
            //启动消息队列的消息处理线程 
            if (!_semaphore.WaitOne(0)) return;
            System.Threading.ThreadPool.UnsafeQueueUserWorkItem(RunHandleProcess, null);
        }

        /// <summary>
        /// 将对象添加到集合结尾处
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            if (item == null) return;
            if (Interlocked.CompareExchange(ref _isDisposed, 1, 1) != 0) return;
            mq.Enqueue(item);
            StartProcess();
        }
        /// <summary>
        /// 清理消息队列上的所有未处理消息
        /// </summary>
        public void Clear()
        {
            T msg;
            while (mq.TryDequeue(out msg)) ;
        }
        /// <summary>
        /// 释放消息队列
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
                return;
            OnDisposing(this, EventArgs.Empty);
        }

        public bool SetFailRequestCurrentMesage()
        {
            return false;
        }

        public void Restart()
        {
        }

        public int Count => mq.Count;
    }


}
