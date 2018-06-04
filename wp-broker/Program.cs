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
       
		private static Dictionary<string, Parameter> parameters = new Dictionary<string, Parameter>();
         
        /**
         * Initializes and configure the accepeted parameters. 
         **/
		public static void InitParameters(){

			Parameter f = new Parameter(PARAM_VERIFY, false);
			Parameter w = new Parameter(PARAM_WEB, true);

			parameters.Add(f.Name, f);
			parameters.Add(w.Name, w);         
		}


        /**
         * parse all parameters received from standard input
         */
		private static void ParseParameters(string[] args){
			for (int i = 0; i < args.Length; i += 2)
			{
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
					}
					else
					{
						paramStored.Active = true;
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

			if(parameters.ContainsKey(PARAM_VERIFY)){
				Checker chk = new Checker(parameters[PARAM_WEB].Value);
				IEnumerator<Conclusion> conclusions = chk.Check(Checker.CMS.WORDPRESS);
				while(conclusions.MoveNext()){
					Conclusion conclusion = (Conclusion)conclusions.Current;
					Console.WriteLine(conclusion);
				}
			}

        }

    }
}
