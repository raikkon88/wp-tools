using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace wpbroker
{

    class MainClass
    {

		public static string PARAM_VERIFY   = "-f";
		public static string PARAM_WEB = "-w";
        public static string PARAM_HELP = "-h";
       
		private static Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>();
         
        /**
         * Initializes and configure the accepeted parameters. 
         **/
		public static void InitParameters(){

			Parameter f = new Parameter(PARAM_VERIFY, false);
			Parameter w = new Parameter(PARAM_WEB, true);
            Parameter h = new Parameter(PARAM_HELP, false);

			parameters.Add(f.Name, f);
			parameters.Add(w.Name, w);
            parameters.Add(h.Name, h);
		}


        /**
         * parse all parameters received from standard input
         */
		private static void ParseParameters(string[] args){

            int i = 0;
            while(i < args.Length){
                if (!parameters.ContainsKey(args[i]))
                {
                    throw new Exception("[EXCEPTION!!] -> BAD PARAMETERS ... ");
                }
                else
                {
                    Parameter paramStored = parameters[args[i]];
                    if (paramStored.NeedsValue)
                    {
                        if (args.Length > i + 1)
                        {
                            paramStored.Value = args[i + 1];
                            paramStored.Active = true;
                        }
                        else
                        {
                            throw new Exception("[EXCEPTION!!!] -> BAD PARAMTERS ...");
                        }
                        i += 2;
                    }
                    else
                    {
                        paramStored.Active = true;
                        i++;
                    }

                }                
            }

		}

        /**
         * Main program
         **/
        public static void Main(string[] args)
        {

            InitParameters();
            ParseParameters(args);

            foreach (string s in parameters.Keys)
            {
                Console.WriteLine("----------------------------------");
                Console.WriteLine(parameters[s]);
            }


            Console.WriteLine("**************************************************");
            Console.WriteLine("* CHECKING                                       *");
            Console.WriteLine("**************************************************");
            if (parameters[PARAM_HELP].Active)
            {
                ShowHelp();
            }
            else if (!parameters[PARAM_WEB].Active)
            {
                Console.WriteLine("[ERROR] -> This method needs -w paramter with a valid value. ");
                ShowHelp();
            }
            else if (Parameter.HasWrongParameters(parameters.Values.GetEnumerator()))
            {
                Console.WriteLine("[ERROR] -> Parameters required are invalid or inexistent. ");
                ShowHelp();
            }
            else {
                try{
                    // Paramters verified, continue with the execution.   
                    Checker chk = new Checker(parameters[PARAM_WEB].Value);
                    Console.WriteLine(chk.Check(Checker.CMS.WORDPRESS));    
                }
                catch(Exception e){
                    Console.WriteLine("[ERROR] -> An error has occurred with the execution ... ");
                    Console.WriteLine("********************************************************");
                    Console.WriteLine(e);
                }
            }
        }



        /**
         * Exposes all help througth console screen
         */
        public static void ShowHelp(){
            Console.WriteLine("**************************************************");
            Console.WriteLine("* HELP FOR WP-BROKER                             *");
            Console.WriteLine("**************************************************");
            Console.WriteLine(" -h : to consult this help page");
            Console.WriteLine(" -w : to set the wordpress website");
            Console.WriteLine(" -f : to call function verify ");
            Console.WriteLine("**************************************************");
            Console.WriteLine(" USAGE : wp-broker -w <website> [options]");
            Console.WriteLine("**************************************************");
        }

    }
}
