using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkModel
{
    public class WorkerQueue
    {
        Queue<Worker> executionQueue = new Queue<Worker>();

        public void push(Worker worker)
        {
            executionQueue.Enqueue(worker);
        }
        public Worker pop()
        {
            return (executionQueue.Dequeue());
        }

        public int getSize()
        {
            return executionQueue.Count;
        }
    }
}
