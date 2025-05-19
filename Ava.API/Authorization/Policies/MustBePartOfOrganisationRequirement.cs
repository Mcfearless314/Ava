using Microsoft.AspNetCore.Authorization;

namespace Ava.API.Authorization.Policies;

public class MustBePartOfOrganisationRequirement : IAuthorizationRequirement
{
}
