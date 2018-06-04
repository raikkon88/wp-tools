using System;
namespace wpbroker
{
    public class Parameter
    {

		// Attributes ---------------------------------------------------------------------------------
        private string _name;
        private string _value;
		private bool _active;
        private bool _needsValue;

		// Constructors ---------------------------------------------------------------------------------
		public Parameter(string name, bool needsValue){
			_name = name;
			_needsValue = needsValue;
		}

		public Parameter(string name, string value, bool needsValue, bool active)
        {
			_name = name;
			_value = value;
			_needsValue = needsValue;
			_active = active;
        }


        // Properties ---------------------------------------------------------------------------------
		public string Name
        {
            get
            {
				return _name;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

		public bool NeedsValue
        {
            get
            {
				return _needsValue;
            }
        }

		public bool Active{
			get{
				return _active;
			}
			set{
				_active = value;
			}
		}

		// Public Methods ---------------------------------------------------------------------------------

		// Private Methods ---------------------------------------------------------------------------------

		// Overrides ---------------------------------------------------------------------------------
        public override string ToString()
        {
			return "NOM            : " + _name + " \n" +
				   "VALOR          : " + _value + "\n" +
				   "ACTIVE         : " + _active  + "\n" + 
				   "REQUIRED VALUE : " + _needsValue;
        }



    }
}
