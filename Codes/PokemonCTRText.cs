﻿using CommandLine;
using System;

namespace PokemonCTR
{
    class PokemonCTRText
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                CharTable charTable = new CharTable(o.ChartablePath);
                Narc msg = new Narc(o.MessagePath);
                Generation.Gen gen;
                Text text;
                switch (msg.Files.Count)
                {
                    case 610: // DP
                    case 709: // Pt
                    case 814: // HGSS
                        gen = Generation.Gen.Gen4;
                        text = new Text_4(msg, charTable);
                        break;
                    case 273:
                    case 472:
                        gen = Generation.Gen.Gen5;
                        text = new Text_5(msg, charTable);
                        break;
                    default:
                        throw new FormatException();
                }
                if (o.ExtractPath != null)
                {
                    text.Extract(o.ExtractPath);
                }
                if (o.ImportPath != null && o.OutputPath != null)
                {
                    text.Import(o.ImportPath);
                    switch (gen)
                    {
                        case Generation.Gen.Gen4:
                            ((Text_4)text).Save(ref msg, charTable);
                            break;
                        case Generation.Gen.Gen5:
                            ((Text_5)text).Save(ref msg, charTable);
                            break;
                    }
                    msg.Save(o.OutputPath);
                }
                if (charTable.NoCode.Count > 0)
                {
                    Console.WriteLine("以下字符未在码表中：" + string.Join("", charTable.NoCode));
                }
                if (charTable.NoChar.Count > 0)
                {
                    Console.WriteLine("以下编码未在码表中：" + string.Join(", ", charTable.NoChar));
                }
            });
        }
    }
}
