using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEGO.AsyncAPI.Surface.Stubs
{
    using Json.Schema;

    public class ValidatorResult
    {
        public bool IsValid { get; init; }
        public ValidationResults ValidationResults { get; init; }
    }
}
