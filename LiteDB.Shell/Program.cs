﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LiteDB.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            LiteDatabase db = null;
            var input = new InputCommand();
            var display = new Display();

            display.TextWriters.Add(Console.Out);

            // show welcome message
            display.WriteWelcome();

            // if has a argument, its database file - try open
            if (args.Length > 0)
            {
                try
                {
                    db = new LiteDatabase(args[0]);
                }
                catch (Exception ex)
                {
                    display.WriteError(ex.Message);
                }
            }

            while (true)
            {
                // read next command from user
                var cmd = input.ReadCommand();

                if (string.IsNullOrEmpty(cmd)) continue;

                try
                {
                    var isConsoleCommand = ConsoleCommand.TryExecute(cmd, ref db, display, input);

                    if (isConsoleCommand == false)
                    {
                        if(db == null) throw LiteException.NoDatabase();

                        var result = db.Run(cmd);

                        display.WriteResult(result);
                    }
                }
                catch (Exception ex)
                {
                    display.WriteError(ex.Message);
                }
            }
        }
    }
}
