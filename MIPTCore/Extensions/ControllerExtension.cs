using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MIPTCore.Extensions
{
    public static class ControllerExtension
    {
        public static void CheckIdViaModel(this Controller context, int id)
        {
            if(id <= 0)
                context.ModelState.AddModelError(nameof(id), $"'{nameof(id)}' must be positive");
        }
    }
}