using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;


namespace WpfDip
{
    public class Program
    {
       
        public Program(string URL, string login, string APItoken)
        {
            jiraLog = JiraLogin(URL, login, APItoken);
        }
        public Program()
        {
        }
        static Atlassian.Jira.Jira jiraLog;
        /// <summary>
        /// Метод для подключения к Jira
        /// </summary>
        public Atlassian.Jira.Jira JiraLogin(string URL, string login, string APItoken)
        {
            bool errLogFlag = false;
            Atlassian.Jira.Jira jiraLog = null;
            jiraLog = Atlassian.Jira.Jira.CreateRestClient(new Atlassian.Jira.Remote.JiraRestClient(URL, login, APItoken));
            var URLserv = jiraLog.ServerInfo.GetServerInfoAsync().Result.BaseUrl;
            return jiraLog;
        }
        /// <summary>
        /// 
        /// Метод для формирования списка задач на экспорт
        /// </summary>
        public List<IssueWork> CreateIssuesList(Dictionary<string, List<string>> filt)
        {
            var issues = IssuesFilter(filt);//вызов метода фильрации
            string[] parMas;//объявление массива, куда будут записываться значения из Jira
            List<IssueWork> issueList = new List<IssueWork>();//Создания списка, в который будут записываться объекты класса
            IssueWork IssWork;
            foreach (var c in issues)
            {
                parMas = FillingOutputArray(c, filt);//Вызов метода проверки на null
                IssWork = new IssueWork(parMas[0], parMas[1], parMas[2], parMas[3], parMas[4], parMas[5], parMas[6], parMas[7], parMas[8], parMas[9], parMas[10], parMas[11]);
                issueList.Add(IssWork);//Добавление объекта в список
            }
            return issueList;
        }
        /// <summary>
        /// Метод для фильтрации задач
        /// </summary>
        static List<Atlassian.Jira.Issue> IssuesFilter(Dictionary<string, List<string>> filt)
        {
            var issues = jiraLog.Issues.Queryable.ToList();
            List<Atlassian.Jira.Issue> issuesList = new List<Atlassian.Jira.Issue>();
            if (filt.ContainsKey("summary"))
            {
                issuesList.AddRange(
                    issues.Where(c =>//если условие выполняется - нужная задача записывается список
                    {
                        if (filt["summary"].Where(t => t == c.Summary.ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))//Возвращает все задачи, где проект в Jira равен пользовательскому и задача не записана в список
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();
            }

            if (filt.ContainsKey("key"))
            {
                issuesList.AddRange(
                    issues.Where(c =>
                    {
                        if (filt["key"].Where(t => t == c.Key.ToString().ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();
            }

            if (filt.ContainsKey("priority"))
            {
                issuesList.AddRange(
                    issues.Where(c =>
                    {
                        if (filt["priority"].Where(t => t == c.Priority.ToString().ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();

            }

            if (filt.ContainsKey("status"))
            {
                issuesList.AddRange(
                    issues.Where(c =>
                    {
                        if (filt["status"].Where(t => t == c.Status.ToString().ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();
            }

            if (filt.ContainsKey("type"))
            {
                issuesList.AddRange(
                    issues.Where(c =>
                    {
                        if (filt["type"].Where(t => t == c.Type.ToString().ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();
            }

            if (filt.ContainsKey("created"))
            {
                issuesList.AddRange(
                    issues.Where(c =>
                    {
                        if (filt["created"].Where(t => t == c.Created.ToString().ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();
            }

            if (filt.ContainsKey("environment"))
            {
                issuesList.AddRange(
                    issues.Where(c =>
                    {
                        if (filt["environment"].Where(t => t == c.Environment.ToString().ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();
            }

            if (filt.ContainsKey("project"))
            {
                issuesList.AddRange(
                    issues.Where(c =>
                    {
                        if (filt["project"].Where(t => t == c.Project.ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();
            }

            if (filt.ContainsKey("assigneeuser"))
            {
                issuesList.AddRange(
                    issues.Where(c =>
                    {
                        if (filt["assigneeuser"].Where(t => t == c.AssigneeUser.DisplayName.ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();
            }

            if (filt.ContainsKey("reporteruser"))
            {
                issuesList.AddRange(
                    issues.Where(c =>
                    {
                        if (filt["reporteruser"].Where(t => t == c.ReporterUser.DisplayName.ToLower().Replace(" ", "")).Count() > 0 && !issuesList.Contains(c))
                            return true;
                        else return false;
                    }).ToList());
                issues.Clear();
                issues.AddRange(issuesList);
                issuesList.Clear();
            }



            issuesList.AddRange(issues);
            return issuesList;
        }
        /// <summary>
        /// Метод для проверки на Null значений из Jira
        /// </summary>
        static string[] FillingOutputArray(Atlassian.Jira.Issue c, Dictionary<string, List<string>> filt)
        {
            string[] parMas = new string[12];//создание массива, в котором будут храниться значения параметров из Jira
            if (c.Summary != null)
            {
                parMas[0] = c.Summary;
            }
            else parMas[0] = "Аннотация не задана";

            if (c.Key != null)
            {
                parMas[1] = c.Key.ToString();
            }
            else parMas[1] = "Ключ не задан";

            if (c.Priority != null)
            {
                parMas[2] = c.Priority.ToString();
            }
            else parMas[2] = "Приоритет не задан";

            if (c.Status != null)
            {
                parMas[3] = c.Status.ToString();
            }
            else parMas[3] = "Статус не задан";

            if (c.Type != null)
            {
                parMas[4] = c.Type.ToString();
            }
            else parMas[4] = "Тип не задан";

            if (c.Created != null)
            {
                parMas[5] = c.Created.ToString();
            }
            else parMas[5] = "Время создания не задано";

            if (c.Environment != null)
            {
                parMas[6] = c.Environment;
            }
            else parMas[6] = "Окружение не задано";

            if (c.Project != null)
            {
                parMas[7] = c.Project;
            }
            else parMas[7] = "Принадлежность к проекту не задана";

            if (c.AssigneeUser != null)
                parMas[8] = c.AssigneeUser.DisplayName;
            else
                parMas[8] = "Исполнитель не задан";

            if (c.ReporterUser != null)
            {
                parMas[9] = c.ReporterUser.DisplayName;
            }
            else parMas[9] = "Создатель не задан";

            if (c.Description != null)
            {
                parMas[10] = c.Description;
            }
            else parMas[10] = "Описание не задано";

            parMas[11] = StatusChangeCountWork(c, filt);

            return parMas;
        }
        /// <summary>
        /// Метод для подсчёта изменений статуса
        /// </summary>
        static string StatusChangeCountWork (Atlassian.Jira.Issue c, Dictionary<string, List<string>> filt)
        {
            var changeLog = jiraLog.Issues.GetChangeLogsAsync(c.Key.ToString()).Result;
            int count = 0;
            var list = changeLog.ToList();
            list.Reverse();
            List<string> ListFromValue = new List<string>();
            List<string> ListToValue = new List<string>();
            List<string> ListStatusValue = new List<string>();
            if (filt.ContainsKey("statusChangeCount"))
            {
                foreach (var e in list)
                {
                    e.Items.ToList().ForEach(s =>
                    {
                        if (s.FieldName.Contains("status"))
                        {
                            ListFromValue.Add(s.FromValue.ToLower()); //Заполнение списка изначальными значениями
                            ListToValue.Add(s.ToValue.ToLower()); //Заполнение списка конечными значениями
                        }
                    });
                }
                filt["statusChangeCount"].ForEach(t =>
                {
                    ListStatusValue.AddRange(t.Split('-'));//записываем изначальное и конечное значения в список
                    for (int i = 0; i < ListFromValue.Count(); i++)
                    {
                        if (ListFromValue[i].Contains(ListStatusValue[0].ToLower().Trim()) && ListToValue[i].Contains(ListStatusValue[1].ToLower().Trim()))//проверка на то, что происходил переход статуса из изначального пользовательского значения в конечное
                            count++;
                    }
                    ListStatusValue.Clear();
                });
                return count.ToString();
            }
            else
                return "Пользователь не задал значения для подсчёта изменений статуса задачи";
        }
        /// <summary>
        /// Метод для экспорта CSV
        /// </summary>
        public void CSVWork(List<IssueWork> issueList, string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//прописать в инструментарий

            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Summary" + ";" +
                    "Key" + ";" +
                    "Priority" + ";" +
                    "Status" + ";" +
                    "Type" + ";" +
                    "Created" + ";" +
                    "Environment" + ";" +
                    "Project" + ";" +
                    "AssigneeUser" + ";" +
                    "ReporterUser" + ";" +
                    "Description" + ";" +
                    "StatusChangeCount");
            foreach (var s in issueList)
            {
                csv.AppendLine(Regex.Replace(s.Summary, @"\s+", " ") + ";" +
                    Regex.Replace(s.Key, @"\s+", " ") + ";" +
                    Regex.Replace(s.Priority, @"\s+", " ") + ";" +
                    Regex.Replace(s.Status, @"\s+", " ") + ";" +
                    Regex.Replace(s.Type, @"\s+", " ") + ";" +
                    Regex.Replace(s.Created, @"\s+", " ") + ";" +
                    Regex.Replace(s.Environment, @"\s+", " ") + ";" +
                    Regex.Replace(s.Project, @"\s+", " ") + ";" +
                    Regex.Replace(s.AssigneeUser, @"\s+", " ") + ";" +
                    Regex.Replace(s.ReporterUser, @"\s+", " ") + ";" +
                    Regex.Replace(s.Description, @"\s+", " ") + ";" +
                    Regex.Replace(s.StatusChangeCount, @"\s+", " "));
            }

            File.WriteAllText(path, csv.ToString(), Encoding.GetEncoding(1251));//не работает кодировка

        }
        /// <summary>
        /// 
        /// Метод для экспорта в JSON
        /// </summary>
        public void JsonWork(List<IssueWork> issueList, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(issueList));
        }
    }
}
