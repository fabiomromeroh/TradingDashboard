using Microsoft.AspNetCore.Mvc;
using TradingDashboard.Application.Common;

namespace TradingDashboard.API.Extensions
{
    public static class ResultExtensions
    {

        //For Result<T> - success returns Ok with the value
        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            return new ObjectResult(new ProblemDetails()
            {
                Status = (int)result.StatusCode,
                Title = result.StatusCode.ToString(),
                Extensions = { ["errors"] = result.Errors }
            })
            { StatusCode = (int)result.StatusCode };
        }

        //For Result<T> - success returns a custom response
        public static ActionResult ToActionResult<T>(this Result<T> result, Func<T, ActionResult> onSuccess)
        {
            if (result.IsSuccess)
            {
                return onSuccess(result.Value!);
            }

            return new ObjectResult(new ProblemDetails()
            {
                Status = (int)result.StatusCode,
                Title = result.StatusCode.ToString(),
                Extensions = { ["errors"] = result.Errors }
            })
            { StatusCode = (int)result.StatusCode };
        }

        //For Result (no value) - success returns a fixed response
        public static ActionResult ToActionResult(this Result result, Func<ActionResult> onSuccess)
        {
            if (result.IsSuccess)
            {
                return onSuccess();
            }

            return new ObjectResult(new ProblemDetails()
            {
                Status = (int)result.StatusCode,
                Title = result.StatusCode.ToString(),
                Extensions = { ["errors"] = result.Errors }

            })
            { StatusCode = (int)result.StatusCode };
        }


    }
}
