using System.Collections.Generic;

namespace CanalDenunciaWeb.Helper
{
    public class jsonReturn
    {
        public bool respuesta { get; set; }
        public string redirect { get; set; }
        public List<string> errorSumary { get; set; }
        public List<string> errorControl { get; set; }
        public string errorMessage { get; set; }
    }
}