using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WorkModel
{
    public class WorkerExecutionControl
    {
        List<Thread> threads = new List<Thread>();

        public void Run(Worker worker)
        {
            Thread t = new Thread(worker.work);
            t.Name = worker.jobName;
            threads.Add(t);
            t.Start();
        }

        public bool isWorkerFinished(Worker worker)
        {
            String workerName = worker.jobName;
            foreach (Thread t in threads)
            {
                if (t.Name == workerName)
                {
                    return t.IsAlive;
                }
            }
            return false;
        }

        public void removeWorker(String name)
        {
            string workerName = name;
            foreach (Thread t in threads)
            {
                if (t.Name == workerName)
                {
                    t.Abort();
                    threads.Remove(t);
                    return;
                }
            }
        }

        public void removeWorker(Worker worker)
        {
            string workerName = worker.jobName;
            foreach (Thread t in threads)
            {
                if (t.Name == workerName)
                {
                    
                    t.Join();
                    threads.Remove(t);
                    return;
                }
            }
        }

        public String[] executingWorkers()
        {
            String[] returnable = new String[threads.Count];
            int count=0;
            foreach (Thread t in threads)
            {
                returnable[count] = t.Name + ";" + (t.IsAlive ? "alive" : "finished");
                count++;
            }
            return returnable;
        }

        public void ExecuteQueue(WorkerQueue queue, int maxAliveProcess)
        {
            int count=0;
            try
            {
                do
                {
                    if (count < maxAliveProcess && queue.getSize() > 0)
                    {
                        Worker w = queue.pop();
                        Console.WriteLine("Starting: {0}", w.jobName);
                        this.Run(w);
                        count++;
                    }
                    else
                    {
                        foreach (String workStatus in this.executingWorkers())
                        {
                            String nome = workStatus.Split(new Char[] { ';' })[0];
                            String status = workStatus.Split(new Char[] { ';' })[1];
                            if (status == "finished")
                            {
                                Console.WriteLine("Finished: {0}", nome);
                                this.removeWorker(nome);
                                count--;
                            }
                        }
                    }
                    Thread.Sleep(2);
                } while (queue.getSize() > 0 || this.threads.Count() > 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
