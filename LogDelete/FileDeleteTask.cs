/***************************************************************************
 * 
 * 创建时间：   2018/3/9 星期五 12:38:04
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

namespace LogDelete
{
    /// <summary>
    /// 文件删除管理器
    /// </summary>
    class FileDeleteTask
    {
        static MessageQueueMultiple<string> deleteFilePath = new MessageQueueMultiple<string>();
        static MessageQueueMultiple<string> deleteDirPath = new MessageQueueMultiple<string>(1);

        static ConcurrentQueue<string> waitDeleteDirQueue = new ConcurrentQueue<string>();


        static FileDeleteTask()
        {
            deleteFilePath.MessageComing = deleteFilePath_MessageComing;
            deleteDirPath.MessageComing = deleteDirPath_MessageComing;
        }



        private static void deleteDirPath_MessageComing(MessageQueueMultiple<string> sender, string path)
        {

        }

        private static void deleteFilePath_MessageComing(MessageQueueMultiple<string> sender, string path)
        {


        }




        public static void DeleteFilePath(string path)
        {
            //将需要删除的文件路径排入任务对列
            deleteFilePath.Enqueue(path);
        }
        public static void DeleteDirPath(string path)
        {
            waitDeleteDirQueue.Enqueue(path);
        }
        public static void StartDeleteDir()
        {
            string path;

            while (!waitDeleteDirQueue.IsEmpty)
            {
                while (waitDeleteDirQueue.TryDequeue(out path))
                {
                    deleteDirPath.Enqueue(path);
                }
            }
        }

    }
}
