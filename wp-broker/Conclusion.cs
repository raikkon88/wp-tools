using System;
namespace wpbroker
{
    public class Conclusion
    {
		private string _conclusionName;
        private bool _booleanConclusion;
        public Conclusion(string name, bool booleanConclusion)
        {
            _conclusionName = name;
            _booleanConclusion = booleanConclusion;
        }

        public override string ToString()
        {
            return _conclusionName.PadRight(60) + " : " + _booleanConclusion;
        }
    }
}
