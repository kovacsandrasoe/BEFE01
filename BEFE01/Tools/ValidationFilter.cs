using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BEFE01.Tools
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid) 
            {
                var error = new ErrorModel(
                    String.Join(',', context.ModelState.Values.SelectMany(t => 
                    t.Errors.Select(z => z.ErrorMessage)).ToArray()
                    ));
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new JsonResult(error);
            }

        }
    }
}
