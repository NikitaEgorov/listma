using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Text;

namespace Listma
{
    /// <summary>
    /// Statechart cashe
    /// </summary>
    internal sealed class StatechartCache
    {
        const string STATE_CHARTS_FOLDER_APPLICATION_SETTING = "StatechartFolder";
        const string STATE_CHARTS_LOAD_ERROR = "Workflow initialization failed. Statechart file '{0}' does not exist.";
        
        private Hashtable table = new Hashtable();
        string _dir;

        public StatechartCache(string statechartDir)
        {
            if (statechartDir.IsNullOrEmpty())
                _dir = AppDomain.CurrentDomain.BaseDirectory;
            else 
                _dir = statechartDir;
        }

        public Statechart GetStatechart(string _statechartId)
        {
            Statechart result;
            lock (table)
            {
                if (table.Contains(_statechartId))
                {
                    result = table[_statechartId] as Statechart;
                }
                else
                {
                    string path = GetPath(_statechartId);
                    if (File.Exists(path))
                    {
                        using (TextReader reader = new StreamReader(path))
                        {
                            result = Utils.XmlUtility.XmlStr2Obj<Statechart>(reader.ReadToEnd());
                            reader.Close();
                        }
                        table.Add(_statechartId, result);
                    }
                    else
                        throw new WorkflowException(STATE_CHARTS_LOAD_ERROR, path);
                }
            }
            return result;
        }

        string GetPath(string _statechartId)
        {
            if (_dir.IsNullOrEmpty())
                _dir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(_dir, _statechartId + ".xml");
        }



        internal void AddStatechart(Statechart statechart)
        {
            lock (table)
            {
                table.Add(statechart.Id, statechart);
            }
        }
    }
}
