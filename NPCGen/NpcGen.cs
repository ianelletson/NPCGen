using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Formatting = Newtonsoft.Json.Formatting;

namespace NPCGen
{
    static class NpcGen
    {
        private static NameValueCollection _appSettings;
        private static Values _values;
        private static Randomizer _randomizer;
        private static Generator _generator;
        private static List<Npc> _npcsToSave;
        
        static void Main(string[] args)
        {
            ReadSettings();
            Initialize();
            var verbosity = 0;
            var oneNoteEnabled = false;
            var maxVerbosity = 4; // TODO
            var saveAll = false;
            var prevGeneratedNpc = GenerateNpc();

            PrintNpcToConsole(prevGeneratedNpc, verbosity);
            var k = Console.ReadKey(true);
            do
            {
                if (k.Key == ConsoleKey.Enter)
                {
                    verbosity = 0;
                    prevGeneratedNpc = GenerateNpc();
                    PrintNpcToConsole(prevGeneratedNpc);
                }

                if (k.Key == ConsoleKey.Spacebar)
                {
                    if (prevGeneratedNpc == null)
                    {
                        prevGeneratedNpc = GenerateNpc();
                        PrintNpcToConsole(prevGeneratedNpc, verbosity);
                    }
                    else
                    {
                        PrintNpcToConsole(prevGeneratedNpc, ++verbosity);
                        if (verbosity == maxVerbosity)
                        {
                            verbosity = 0;
                            prevGeneratedNpc = null;
                        }
                    }
                }

                if (k.Key == ConsoleKey.S)
                {
                    if (prevGeneratedNpc != null)
                    {
                        WriteNpcToFile(prevGeneratedNpc);
                        Console.WriteLine($"Adding {prevGeneratedNpc.Name} to {GetJsonOutputPath()} (written on exit)");
                    }
                }

                if ((k.Modifiers & ConsoleModifiers.Control) != 0 && k.Key == ConsoleKey.S)
                {
                    saveAll = true;
                    Console.WriteLine($"Will save all NPCs to {GetJsonOutputPath()} (on exit)");
                }

                if (k.Key == ConsoleKey.O)
                {
                    if (prevGeneratedNpc != null)
                    {
                        if (!oneNoteEnabled) // TODO
                        {
                            OneNoteWriter.Notebook = GetDefaultNotebook();
                            OneNoteWriter.Section = GetDefaultSection();
                            oneNoteEnabled = true;
                        }

                        OneNoteWriter.Execute(prevGeneratedNpc);
                        Console.WriteLine($"Writing {prevGeneratedNpc.Name} to OneNote {OneNoteWriter.Notebook} / {OneNoteWriter.Section}");
                    }
                }

                k = Console.ReadKey(true);
            } while (k.Key != ConsoleKey.Q);

            if (!saveAll && _npcsToSave.Count > 0)
                WriteAllNpcsToFile(_npcsToSave, GetJsonOutputPath());
            if (saveAll)
                WriteAllNpcsToFile(_generator.GeneratedNpcs, GetJsonOutputPath());
        }

        private static Npc GenerateNpc()
        {
            return _generator.CreateNpc();
        }

        private static void PrintNpcToConsole(Npc npc, int verbosity = 0)
        {
            Console.Clear();
            Console.WriteLine(npc.Print(verbosity));
        }

        private static void WriteNpcToFile(Npc npc)
        {
            if (!_npcsToSave.Contains(npc))
                _npcsToSave.Add(npc);
        }

        private static void WriteAllNpcsToFile(IReadOnlyCollection<Npc> npcs, string filePath)
        {
            var jo = JsonConvert.SerializeObject(npcs, Formatting.Indented);
            File.WriteAllText(filePath, jo);
        }

        private static void Initialize()
        {
            if (_values == null)
                _values = new Values(GetValuesPath());
            if (_randomizer == null)
                _randomizer = new Randomizer(_values);
            if (_generator == null)
                _generator = new Generator(_randomizer);
            if (_npcsToSave == null || _npcsToSave.Count == 0)
                _npcsToSave = new List<Npc>();
        }

        private static void ReadSettings()
        {
            try
            {
                _appSettings = ConfigurationManager.AppSettings;
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine($"Couldn't read settings \n {e}");
            }
        }

        private static string GetValuesPath()
        {
            var vp = _appSettings["values"];
            return string.IsNullOrWhiteSpace(vp) ? @"c:\temp\values.json" : Path.Combine(Directory.GetCurrentDirectory(), vp);
        }

        private static string GetJsonOutputPath()
        {
            var jop = _appSettings["jsonOutFile"];
            return string.IsNullOrWhiteSpace(jop) ? @"c:\temp\npcs.json" : Path.Combine(Directory.GetCurrentDirectory(), jop);
        }

        private static string GetDefaultNotebook()
        {
            var dnb = _appSettings["defaultNotebook"];
            return string.IsNullOrWhiteSpace(dnb) ? string.Empty : dnb;
        }

        private static string GetDefaultSection()
        {
            var ds = _appSettings["defaultSection"];
            return string.IsNullOrWhiteSpace(ds) ? string.Empty : ds;
        }
    }
}
