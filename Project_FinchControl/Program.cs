using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Description: An app that allows you to play with the 
    // finch robot with a menu.
    // Application Type: Console
    // Author: Dell-Smith, Zach
    // Dated Created: 2/17/2021
    // Last Modified: 3/28/2021
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        TalentShowDisplayMenuScreen(finchRobot);
                        break;

                    case "c":
                        
                        break;

                    case "d":
                        AlarmSystemDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        DisplaySetTheme(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region Persistance

        
           


            //**************************************************
            //*                  Set Theme                     *
            //**************************************************

            static void DisplaySetTheme(Finch finchRobot)
            {
                (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
                bool themeChosen = false;

                //
                // set current theme
                //
                themeColors = ReadThemeData();

                Console.ForegroundColor = themeColors.foregroundColor;
                Console.BackgroundColor = themeColors.backgroundColor;
                Console.Clear();

                DisplayScreenHeader("Set Application Theme");

                Console.WriteLine($"Current foreground color is: {Console.ForegroundColor}");
                Console.WriteLine($"Current background color is: {Console.BackgroundColor}");
                Console.WriteLine();

                Console.Write("Would you like to change the current theme [ yes | no ]?");
                if (Console.ReadLine().ToLower() == "yes")
                {
                    do
                    {
                        themeColors.foregroundColor = GetConsoleColorFromUser("foreground");
                        themeColors.backgroundColor = GetConsoleColorFromUser("background");

                        //
                        // set new theme
                        //
                        Console.ForegroundColor = themeColors.foregroundColor;
                        Console.BackgroundColor = themeColors.backgroundColor;
                        Console.Clear();

                        DisplayScreenHeader("Set Application Theme");
                        Console.WriteLine($"New foreground color: {Console.ForegroundColor}");
                        Console.WriteLine($"New background color: {Console.BackgroundColor}");

                        Console.WriteLine();
                        Console.Write("Is this theme what you would like?");
                        if (Console.ReadLine().ToLower() == "yes")
                        {
                            themeChosen = true;
                            WriteThemeData(themeColors.foregroundColor, themeColors.backgroundColor);
                        }

                    } while (!themeChosen);
                }
                DisplayContinuePrompt();
            }

            // get a console color from the user
            static ConsoleColor GetConsoleColorFromUser(string property)
            {
                ConsoleColor consoleColor;
                bool validConsoleColor;

                do
                {
                    Console.Write($"Enter a value for the {property}:");
                    validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                    if (!validConsoleColor)
                    {
                        Console.WriteLine("Please provide a valid console color!\n");
                    }
                    else
                    {
                        validConsoleColor = true;
                    }

                } while (!validConsoleColor);

                return consoleColor;
            }

            // read theme info from data file
            static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeData()
            {
                string dataPath = @"Data/Theme.txt";
                string[] themeColors;

                ConsoleColor foregroundColor;
                ConsoleColor backgroundColor;

                themeColors = File.ReadAllLines(dataPath);

                Enum.TryParse(themeColors[0], true, out foregroundColor);
                Enum.TryParse(themeColors[1], true, out backgroundColor);

                return (foregroundColor, backgroundColor);
            }

            // write theme info to data
            static void WriteThemeData(ConsoleColor foreground, ConsoleColor background)
            {
                string dataPath = @"Data/Theme.txt";

                File.WriteAllText(dataPath, foreground.ToString() + "\n");
                File.AppendAllText(dataPath, background.ToString());
            }
        #endregion

        #region Alarm System

        static void AlarmSystemDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;
            int minMaxThresholdValue = 0;
            string sensorsToMonitor = "";
            string rangeType = "";
           
            int timeToMonitor = 0;

            do
            {
                DisplayScreenHeader("Alarm System Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set sensors to monitor");
                Console.WriteLine("\tb) Set range type");
                Console.WriteLine("\tc) Set min/max threshold value");
                Console.WriteLine("\td) Set time to monitor");
                Console.WriteLine("\te) Set alarm");
                Console.WriteLine("\tq) ");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = AlarmSystemDisplaySetSensors();
                        break;

                    case "b":
                        rangeType = AlarmSystemDisplayRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = AlarmSystemDisplayThresholdValue(rangeType, finchRobot);
                        break;

                    case "d":
                        timeToMonitor = AlarmSystemDisplayTimeToMonitor();
                        break;
                    case "e":
                        AlarmSystemSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;

                    case "q":
                        
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void AlarmSystemSetAlarm(Finch finchRobot, string sensorsToMonitor, string rangeType, int minMaxThresholdValue, int timeToMonitor)
        {
            int secondsElapsed = 0;
            bool thresholdExeeded = false;
            int currentLightSensorValue = 0;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"Sensors to monitor {sensorsToMonitor}");
            Console.WriteLine("Range typr {0}", rangeType);
            Console.WriteLine("Min/Max threshold value: " + minMaxThresholdValue);
            Console.WriteLine($"Time to monitor: {timeToMonitor}");
            Console.WriteLine();

            Console.WriteLine("Press any key to begin monitoring");
            Console.ReadKey();
            Console.WriteLine();

            while ((secondsElapsed < timeToMonitor) && !thresholdExeeded)
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightSensorValue = finchRobot.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightSensorValue = finchRobot.getRightLightSensor();
                        break;

                    case "both":
                        currentLightSensorValue = (finchRobot.getRightLightSensor() + finchRobot.getLeftLightSensor()) / 2;
                        break;
                    
                }
                switch (rangeType)
                {
                    case "mimimum":
                        if (currentLightSensorValue < minMaxThresholdValue)
                        {
                            thresholdExeeded = true;
                        }
                        break;

                    case "maximum":
                        if (currentLightSensorValue > minMaxThresholdValue)
                        {
                            thresholdExeeded = true;
                        }
                        break;

                  
                }
                finchRobot.wait(1000);
                secondsElapsed++;

            }

            if (thresholdExeeded)
            {
                Console.WriteLine($"The {rangeType} threshold value was exeeded by the current light sensor value of {currentLightSensorValue}");
            }
            else
            {
                Console.WriteLine($"The {rangeType} threshold value of {minMaxThresholdValue} was not exeeded.");
            }


            DisplayMenuPrompt("Alarm System");
        }

        static int AlarmSystemDisplayTimeToMonitor()
        {
            int timeToMonitor = 0;

            DisplayScreenHeader("Time to Monitor");

            Console.Write("Enter Time to Monitor");
            timeToMonitor = int.Parse(Console.ReadLine());

            Console.WriteLine($"Time to monitor is {timeToMonitor} seconds");

            DisplayMenuPrompt("Alarm System");

            return timeToMonitor;
        }

        static int AlarmSystemDisplayThresholdValue(string rangeType, Finch finchRobot)
        {
            int minMaxThresholdValue = 0;
           
            DisplayScreenHeader("Minimum/Maaximum Threshold Value");

            Console.WriteLine($"Left light sensor ambiant value: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"Right light sensor ambiant value: {finchRobot.getRightLightSensor()}");

            Console.WriteLine($"Enter the {rangeType} light sensor value");
            int.TryParse(Console.ReadLine(), out minMaxThresholdValue);

            Console.WriteLine($"The threshold is {minMaxThresholdValue}");

            DisplayMenuPrompt("Alarm System");

            return minMaxThresholdValue;
        }



        static string AlarmSystemDisplayRangeType()
        {
            string rangeType;

            DisplayScreenHeader("Range Type");

            Console.Write("Enter Range Type [minimum/maximum]:");
            rangeType = Console.ReadLine();

            Console.WriteLine($"The range type is {rangeType}");

            DisplayMenuPrompt("Alarm System");
            return rangeType;
        }

        static string AlarmSystemDisplaySetSensors()
        {
            string setSensorsToMonitor;

            DisplayScreenHeader("Sensors to Monitor");

            Console.Write("Enter sensors to Monitor [Left, Right, both]:");
            setSensorsToMonitor = Console.ReadLine();

            Console.WriteLine($"The {setSensorsToMonitor} sensor/sensor's are being moitored");

            DisplayMenuPrompt("Alarm System");

            return setSensorsToMonitor;
        }
         
        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void TalentShowDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Play A Song");
                Console.WriteLine("\tc) Movement");
                Console.WriteLine("\td) Choose your own color");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        TalentShowDisplayLightAndSound(finchRobot);
                        break;

                    case "b":
                        TalentShowPlayASong(finchRobot);
                        break;

                    case "c":
                        TalentShowMovement(finchRobot);
                        break;

                    case "d":
                        TalentShowUserChoice(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void TalentShowDisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
            DisplayContinuePrompt();


            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 100);

            }
           
            LightsandSound(finchRobot);

            DisplayContinuePrompt();

            DisplayMenuPrompt("Talent Show Menu");
        }

        static void TalentShowPlayASong(Finch finchRobot)
        {
            finchRobot.noteOn(660);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(150);

            finchRobot.noteOn(660);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(300);

            finchRobot.noteOn(660);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(300);

            finchRobot.noteOn(510);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(100);

            finchRobot.noteOn(660);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(300);

            finchRobot.noteOn(770);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(550);

            finchRobot.noteOn(380);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(575);
            //===========================
            finchRobot.noteOn(510);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(450);

            finchRobot.noteOn(300);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(400);

            finchRobot.noteOn(320);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(500);

            finchRobot.noteOn(440);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(300);

            finchRobot.noteOn(480);
            finchRobot.wait(80);
            finchRobot.noteOff();
            finchRobot.wait(330);

            finchRobot.noteOn(450);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(150);

            finchRobot.noteOn(430);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(150);

            finchRobot.noteOn(380);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(200);

            finchRobot.noteOn(660);
            finchRobot.wait(80);
            finchRobot.noteOff();
            finchRobot.wait(200);

            finchRobot.noteOn(760);
            finchRobot.wait(50);
            finchRobot.noteOff();
            finchRobot.wait(150);

            finchRobot.noteOn(860);
            finchRobot.wait(100);
            finchRobot.noteOff();
            finchRobot.wait(300);

            finchRobot.noteOn(700);
            finchRobot.wait(80);
            finchRobot.noteOff();
            finchRobot.wait(150);

            finchRobot.noteOn(760);
            finchRobot.wait(50);
            finchRobot.noteOff();
            finchRobot.wait(350);

            finchRobot.noteOn(660);
            finchRobot.wait(80);
            finchRobot.noteOff();
            finchRobot.wait(300);

            finchRobot.noteOn(520);
            finchRobot.wait(80);
            finchRobot.noteOff();
            finchRobot.wait(150);

            finchRobot.noteOn(580);
            finchRobot.wait(80);
            finchRobot.noteOff();
            finchRobot.wait(150);

            finchRobot.noteOn(480);
            finchRobot.wait(80);
            finchRobot.noteOff();
            finchRobot.wait(500);

            DisplayContinuePrompt();
        }

        // Mybe one day ill get the whole song :/
//:beep frequency = 510 length = 100ms;
//:delay 450ms;
//:beep frequency = 380 length = 100ms;
//:delay 400ms;
//:beep frequency = 320 length = 100ms;
//:delay 500ms;
//:beep frequency = 440 length = 100ms;
//:delay 300ms;
//:beep frequency = 480 length = 80ms;
//:delay 330ms;
//:beep frequency = 450 length = 100ms;
//:delay 150ms;
//:beep frequency = 430 length = 100ms;
//:delay 300ms;
//:beep frequency = 380 length = 100ms;
//:delay 200ms;
//:beep frequency = 660 length = 80ms;
//:delay 200ms;
//:beep frequency = 760 length = 50ms;
//:delay 150ms;
//:beep frequency = 860 length = 100ms;
//:delay 300ms;
//:beep frequency = 700 length = 80ms;
//:delay 150ms;
//:beep frequency = 760 length = 50ms;
//:delay 350ms;
//:beep frequency = 660 length = 80ms;
//:delay 300ms;
//:beep frequency = 520 length = 80ms;
//:delay 150ms;
//:beep frequency = 580 length = 80ms;
//:delay 150ms;
//:beep frequency = 480 length = 80ms;
//:delay 500ms;

//:beep frequency = 500 length = 100ms;
//:delay 300ms;

//:beep frequency = 760 length = 100ms;
//:delay 100ms;
//:beep frequency = 720 length = 100ms;
//:delay 150ms;
//:beep frequency = 680 length = 100ms;
//:delay 150ms;
//:beep frequency = 620 length = 150ms;
//:delay 300ms;

//:beep frequency = 650 length = 150ms;
//:delay 300ms;
//:beep frequency = 380 length = 100ms;
//:delay 150ms;
//:beep frequency = 430 length = 100ms;
//:delay 150ms;

//:beep frequency = 500 length = 100ms;
//:delay 300ms;
//:beep frequency = 430 length = 100ms;
//:delay 150ms;
//:beep frequency = 500 length = 100ms;
//:delay 100ms;
//:beep frequency = 570 length = 100ms;
//:delay 220ms;

//:beep frequency = 500 length = 100ms;
//:delay 300ms;

//:beep frequency = 760 length = 100ms;
//:delay 100ms;
//:beep frequency = 720 length = 100ms;
//:delay 150ms;
//:beep frequency = 680 length = 100ms;
//:delay 150ms;
//:beep frequency = 620 length = 150ms;
//:delay 300ms;

//:beep frequency = 650 length = 200ms;
//:delay 300ms;

//:beep frequency = 1020 length = 80ms;
//:delay 300ms;
//:beep frequency = 1020 length = 80ms;
//:delay 150ms;
//:beep frequency = 1020 length = 80ms;
//:delay 300ms;

//:beep frequency = 380 length = 100ms;
//:delay 300ms;
//:beep frequency = 500 length = 100ms;
//:delay 300ms;

//:beep frequency = 760 length = 100ms;
//:delay 100ms;
//:beep frequency = 720 length = 100ms;
//:delay 150ms;
//:beep frequency = 680 length = 100ms;
//:delay 150ms;
//:beep frequency = 620 length = 150ms;
//:delay 300ms;

//:beep frequency = 650 length = 150ms;
//:delay 300ms;
//:beep frequency = 380 length = 100ms;
//:delay 150ms;
//:beep frequency = 430 length = 100ms;
//:delay 150ms;

//:beep frequency = 500 length = 100ms;
//:delay 300ms;
//:beep frequency = 430 length = 100ms;
//:delay 150ms;
//:beep frequency = 500 length = 100ms;
//:delay 100ms;
//:beep frequency = 570 length = 100ms;
//:delay 420ms;

//:beep frequency = 585 length = 100ms;
//:delay 450ms;

//:beep frequency = 550 length = 100ms;
//:delay 420ms;

//:beep frequency = 500 length = 100ms;
//:delay 360ms;

//:beep frequency = 380 length = 100ms;
//:delay 300ms;
//:beep frequency = 500 length = 100ms;
//:delay 300ms;
//:beep frequency = 500 length = 100ms;
//:delay 150ms;
//:beep frequency = 500 length = 100ms;
//:delay 300ms;

//:beep frequency = 500 length = 100ms;
//:delay 300ms;

//:beep frequency = 760 length = 100ms;
//:delay 100ms;
//:beep frequency = 720 length = 100ms;
//:delay 150ms;
//:beep frequency = 680 length = 100ms;
//:delay 150ms;
//:beep frequency = 620 length = 150ms;
//:delay 300ms;

//:beep frequency = 650 length = 150ms;
//:delay 300ms;
//:beep frequency = 380 length = 100ms;
//:delay 150ms;
//:beep frequency = 430 length = 100ms;
//:delay 150ms;

//:beep frequency = 500 length = 100ms;
//:delay 300ms;
//:beep frequency = 430 length = 100ms;
//:delay 150ms;
//:beep frequency = 500 length = 100ms;
//:delay 100ms;
//:beep frequency = 570 length = 100ms;
//:delay 220ms;

//:beep frequency = 500 length = 100ms;
//:delay 300ms;

//:beep frequency = 760 length = 100ms;
//:delay 100ms;
//:beep frequency = 720 length = 100ms;
//:delay 150ms;
//:beep frequency = 680 length = 100ms;
//:delay 150ms;
//:beep frequency = 620 length = 150ms;
//:delay 300ms;

//:beep frequency = 650 length = 200ms;
//:delay 300ms;

//:beep frequency = 1020 length = 80ms;
//:delay 300ms;
//:beep frequency = 1020 length = 80ms;
//:delay 150ms;
//:beep frequency = 1020 length = 80ms;
//:delay 300ms;

//:beep frequency = 380 length = 100ms;
//:delay 300ms;
//:beep frequency = 500 length = 100ms;
//:delay 300ms;

//:beep frequency = 760 length = 100ms;
//:delay 100ms;
//:beep frequency = 720 length = 100ms;
//:delay 150ms;
//:beep frequency = 680 length = 100ms;
//:delay 150ms;
//:beep frequency = 620 length = 150ms;
//:delay 300ms;

//:beep frequency = 650 length = 150ms;
//:delay 300ms;
//:beep frequency = 380 length = 100ms;
//:delay 150ms;
//:beep frequency = 430 length = 100ms;
//:delay 150ms;

//:beep frequency = 500 length = 100ms;
//:delay 300ms;
//:beep frequency = 430 length = 100ms;
//:delay 150ms;
//:beep frequency = 500 length = 100ms;
//:delay 100ms;
//:beep frequency = 570 length = 100ms;
//:delay 420ms;

//:beep frequency = 585 length = 100ms;
//:delay 450ms;

//:beep frequency = 550 length = 100ms;
//:delay 420ms;

//:beep frequency = 500 length = 100ms;
//:delay 360ms;

//:beep frequency = 380 length = 100ms;
//:delay 300ms;
//:beep frequency = 500 length = 100ms;
//:delay 300ms;
//:beep frequency = 500 length = 100ms;
//:delay 150ms;
//:beep frequency = 500 length = 100ms;
//:delay 300ms;

//:beep frequency = 500 length = 60ms;
//:delay 150ms;
//:beep frequency = 500 length = 80ms;
//:delay 300ms;
//:beep frequency = 500 length = 60ms;
//:delay 350ms;
//:beep frequency = 500 length = 80ms;
//:delay 150ms;
//:beep frequency = 580 length = 80ms;
//:delay 350ms;
//:beep frequency = 660 length = 80ms;
//:delay 150ms;
//:beep frequency = 500 length = 80ms;
//:delay 300ms;
//:beep frequency = 430 length = 80ms;
//:delay 150ms;
//:beep frequency = 380 length = 80ms;
//:delay 600ms;

//:beep frequency = 500 length = 60ms;
//            :delay 150ms;
//            :beep frequency = 500 length = 80ms;
//            :delay 300ms;
//            :beep frequency = 500 length = 60ms;
//            :delay 350ms;
//            :beep frequency = 500 length = 80ms;
//            :delay 150ms;
//            :beep frequency = 580 length = 80ms;
//            :delay 150ms;
//            :beep frequency = 660 length = 80ms;
//            :delay 550ms;

//            :beep frequency = 870 length = 80ms;
//            :delay 325ms;
//            :beep frequency = 760 length = 80ms;
//            :delay 600ms;

//            :beep frequency = 500 length = 60ms;
//            :delay 150ms;
//            :beep frequency = 500 length = 80ms;
//            :delay 300ms;
//            :beep frequency = 500 length = 60ms;
//            :delay 350ms;
//            :beep frequency = 500 length = 80ms;
//            :delay 150ms;
//            :beep frequency = 580 length = 80ms;
//            :delay 350ms;
//            :beep frequency = 660 length = 80ms;
//            :delay 150ms;
//            :beep frequency = 500 length = 80ms;
//            :delay 300ms;
//            :beep frequency = 430 length = 80ms;
//            :delay 150ms;
//            :beep frequency = 380 length = 80ms;
//            :delay 600ms;
            
//            :beep frequency = 660 length = 100ms;
//            :delay 150ms;
//            :beep frequency = 660 length = 100ms;
//            :delay 300ms;
//            :beep frequency = 660 length = 100ms;
//            :delay 300ms;
//            :beep frequency = 510 length = 100ms;
//            :delay 100ms;
//            :beep frequency = 660 length = 100ms;
//            :delay 300ms;
//            :beep frequency = 770 length = 100ms;
//            :delay 550ms;
//            :beep frequency = 380 length = 100ms;
//            :delay 575ms;
        
        static void TalentShowUserChoice(Finch finchRobot)
        {
    
            Console.WriteLine();
            Console.WriteLine("\tPlease enter a value for red (0-255).");
            int red = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("\tPlease enter a value for green (0-255).");
            int green = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("\tPlease enter a value for blue (0-255).");
            int blue = int.Parse(Console.ReadLine());

            finchRobot.setLED(red, green, blue);
            finchRobot.wait(10000);
            finchRobot.setLED(0, 0, 0);

            DisplayContinuePrompt();

        }
      
        static void LightsandSound(Finch finchRobot)
        {
            finchRobot.setLED(255, 0, 0);
            finchRobot.noteOn(150);
            finchRobot.wait(1000);
            finchRobot.noteOff();

            finchRobot.setLED(0, 255, 0);
            finchRobot.noteOn(100);
            finchRobot.wait(1000);
            finchRobot.noteOff();

            finchRobot.setLED(0, 0, 255);
            finchRobot.noteOn(200);
            finchRobot.wait(1000);
            finchRobot.noteOff();
        }

        static void TalentShowMovement(Finch finchRobot)
        {

            //int forwardTime = int.Parse(Console.ReadLine());

            //int left = int.Parse(Console.ReadLine());
            //int leftTime = int.Parse(Console.ReadLine());

            //int right = int.Parse(Console.ReadLine());
            //int rightTime = int.Parse(Console.ReadLine());

            Forward(finchRobot);

            LightsandSound(finchRobot);

            DisplayContinuePrompt();

            TurnLeft(finchRobot);

            LightsandSound(finchRobot);

            DisplayContinuePrompt();

            TurnRight(finchRobot);

            LightsandSound(finchRobot);

            DisplayContinuePrompt();
        }
      
        static void TurnRight(Finch finchRobot)
        {
            Console.WriteLine();
            Console.WriteLine("Tell me how fast I should go right (0-255)");
            int right = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("And for how long? (in ms)");
            int rightTime = int.Parse(Console.ReadLine());

            finchRobot.setMotors(0, right);
            finchRobot.wait(rightTime);
            finchRobot.setMotors(0, 0);
        }

        static void TurnLeft(Finch finchRobot)
        {
            Console.WriteLine();
            Console.WriteLine("Tell me how fast I should go left (0-255)");
            int left = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("And for how long? (in ms)");
            int leftTime = int.Parse(Console.ReadLine());

            finchRobot.setMotors(left, 0);
            finchRobot.wait(leftTime);
            finchRobot.setMotors(0, 0);
        }
        #endregion

        static void Forward (Finch finchRobot)
        {
            Console.WriteLine();
            Console.WriteLine("Tell me how fast I should go forward (0-255)");
            int forward = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("And for how long? (in ms)");
            int forwardTime = int.Parse(Console.ReadLine());

            finchRobot.setMotors(forward, forward);
            finchRobot.wait(forwardTime);
            finchRobot.setMotors(0, 0);
        }
        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // TODO test connection and provide user feedback - text, lights, sounds

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}

