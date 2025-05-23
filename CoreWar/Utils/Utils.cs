﻿namespace CoreWar {

    /// <summary>
    /// Egyéb segédfüggvények
    /// </summary>
    public class Utils {


        /// <summary>
        /// MemFight logó ASCII art formájában
        /// </summary>
        public static string GetLogo() {
            return """

  __  __                ______ _       _     _   
 |  \/  |              |  ____(_)     | |   | |  
 | \  / | ___ _ __ ___ | |__   _  __ _| |__ | |_ 
 | |\/| |/ _ \ '_ ` _ \|  __| | |/ _` | '_ \| __|
 | |  | |  __/ | | | | | |    | | (_| | | | | |_ 
 |_|  |_|\___|_| |_| |_|_|    |_|\__, |_| |_|\__|
                                  __/ |          
                                 |___/           

""";
        }


        /// <summary>
        /// Névjegy
        /// </summary>
        public static string GetCredits() {
            return "Készítette: Kovács Bence\nSZTE-TTIK Informatikai Intézet\nprogramtervező informatikus BSc szakdolgozat - 2024/25/II. félév\nSzakdolgozat témája: CoreWar játék létrehozása .NET keretrendszerben\nTémavezető: dr. Kiss Ákos";
        }
    }
}
