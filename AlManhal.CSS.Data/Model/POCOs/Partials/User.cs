using PWC.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Data.Model.POCOs
{
    public partial class User : DomainEntity<int>
    {
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Username) && UserTypeID <0 && string.IsNullOrEmpty(Password))
            {
                yield return new ValidationResult("value can't be empty or Zero.", new[] { "Username", "UserTypeID", "Password" });
            }
        }
    }
}
