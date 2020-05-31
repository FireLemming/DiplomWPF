using System;
using System.Collections.Generic;
using System.Text;

namespace WpfDip
{
    public class IssueWork
    {
        public IssueWork(string summary, string key, string priority, string status, string statusChangeCount, string type, string created, string environment, string project, string assigneeUser, string reporterUser, string description)
        {
            Summary = summary;
            Key = key;
            Priority = priority;
            Status = status;
            StatusChangeCount = statusChangeCount;
            Type = type;
            Created = created;
            Environment = environment;
            Project = project;
            AssigneeUser = assigneeUser;
            ReporterUser = reporterUser;
            Description = description;
        }
        public string Summary { get; set; }
        public string Key { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string StatusChangeCount { get; set; }
        public string Type { get; set; }
        public string Created { get; set; }
        public string Environment { get; set; }
        public string Project { get; set; }
        public string ReporterUser { get; set; }
        public string AssigneeUser { get; set; }
        public string Description { get; set; }
    }
}
