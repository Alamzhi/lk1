using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lk1
{
    //1.Надо сделать очередь с операциями push(T) и T pop(). Операции должны поддерживать
    //обращение с разных потоков. Операция push всегда вставляет и выходит. Операция pop ждет пока
    //не появится новый элемент. В качестве контейнера внутри можно использовать только
    //стандартную очередь (Queue) .
    public class Class1<T>
    {
        private readonly object locker = new object();
        private Queue<T> queue;

        static AutoResetEvent autoEvent = new AutoResetEvent(false);

        public Class1()
        {
            queue = new Queue<T>();
        }

        public void Push(T item)
        {
            lock (locker)
            {
                queue.Enqueue(item);
                Monitor.Pulse(locker);
            }
            autoEvent.Set();
        }

        public T Pop()
        {
            lock (locker)
            {
                if (queue.Count == 0)
                {
                    // если очередь пуста, то ждем максимум 1 секунду
                    Monitor.Pulse(locker);
                    Monitor.Wait(locker, 1000);
                }
                if (queue.Count > 0)
                {
                    var t = queue.Dequeue();
                    Monitor.Pulse(locker);
                    return t;
                }
            }
            throw new InvalidOperationException();
        }

        public int Count
        {
            get { return queue.Count; }
        }
    }
}
