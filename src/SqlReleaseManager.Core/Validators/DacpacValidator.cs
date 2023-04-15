using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SqlReleaseManager.Core.Models;

namespace SqlReleaseManager.Core.Validators
{
    internal class CreateDacpacValidator :AbstractValidator<CreateOrUpdateDacpac>
    {
        public CreateDacpacValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Stream).NotEmpty();

        }
    }
}
