﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

#if DEBUG
namespace LiteDB.Shell.Commands
{
    /// <summary>
    /// Delete database e open again - used for tests only
    /// </summary>
    internal class Reset : ConsoleCommand
    {
        public override bool IsCommand(StringScanner s)
        {
            return s.Scan(@"reset\s+").Length > 0;
        }

        public override void Execute(ref LiteDatabase db, StringScanner s, Display display, InputCommand input)
        {
            var filename = s.Scan(@".+");

            if (db != null)
            {
                db.Dispose();
            }

            File.Delete(filename);

            db = new LiteDatabase(filename);
        }
    }
}
#endif