using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkModel
{
    public abstract class Worker
    {
        public String jobName { get; set; }
        public String jobId { get; set; }

        public Worker(String jobName)
        {
            this.jobName = jobName;
            this.jobId = generateJobId();
        }

        private String generateJobId()
        {
            String final = DateTime.Now.ToLongDateString() + jobName;
            return (final.GetHashCode().ToString());
        }
        public abstract void work();
    }
}
