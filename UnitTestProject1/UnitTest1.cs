using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading;
using lk1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
   
    //1.Надо сделать очередь с операциями push(T) и T pop(). Операции должны поддерживать
    //обращение с разных потоков. Операция push всегда вставляет и выходит. Операция pop ждет пока
    //не появится новый элемент. В качестве контейнера внутри можно использовать только
    //стандартную очередь (Queue) .
    [TestClass]
    public class UnitTest1
    {
        static object workerLocker = new object();
        static int runningPushers = 10;
        static int runningPopers = 5;
        static readonly Class1<string> cl = new Class1<string>();

        [TestMethod]
        public void TestMethod1()
        {
            Trace.WriteLine("Запуск пула...");

            for (int i = 0; i < runningPushers; i++)
                ThreadPool.QueueUserWorkItem(PushMethod, i);
            for (int i = 0; i < runningPopers; i++)
                ThreadPool.QueueUserWorkItem(PopThread, i);

            Trace.WriteLine("Ожидаем завершения работы потоков...");

            lock (workerLocker)
                while (runningPushers + runningPopers > 0)
                {
                    Monitor.Wait(workerLocker);
                    Trace.WriteLine("Осталось потоков Push: " + runningPushers.ToString());
                    Trace.WriteLine("Осталось потоков Pop: " + runningPopers.ToString());
                }
            Trace.WriteLine("Готово!");
        }

        private void PushMethod(object instance)
        {
            if (cl == null) throw new ArgumentNullException("cl");

            for (int i = 0; i < 100; i++)
            {
                cl.Push(String.Format("push{0}:{1}", instance, i));
            }

            lock (workerLocker)
            {
                runningPushers--;
                Monitor.Pulse(workerLocker);
            }

            Trace.WriteLine(String.Format("push{0}: поток завершен", instance));
        }

        private void PopThread(object instance)
        {
            if (cl == null) throw new ArgumentNullException("cl");
            try
            {
                int count = 0;
                lock(workerLocker)
                {
                    count = runningPushers;
                    Monitor.Pulse(workerLocker);
                }
                while (cl.Count > 0 || count > 0)
                {
                    var item = cl.Pop();
                    Trace.WriteLine(String.Format("pop{0}: {1}", instance, item));
                }

                Trace.WriteLine(String.Format("pop{0}: поток завершен", instance));
            }
            catch (InvalidOperationException)
            {
                Trace.WriteLine(instance + ": Очередь пуста");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(instance + ": Another exception: " + ex.Message);
            }

            lock (workerLocker)
            {
                runningPopers--;
                Monitor.Pulse(workerLocker);
            }
        }
    }
}
