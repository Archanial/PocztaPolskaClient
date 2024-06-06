using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace poczta;

public sealed class HATEOSFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        try
        {
            if(context.Result is not OkObjectResult result)
                return;
            
            if(result.Value is not IResponseBase response)
                throw new Exception("Invalid response type");

            ModifyResponse(response, context);
        }
        catch
        { 
            //ignored
        }
    }
    
    private void ModifyResponse(IResponseBase response, ActionContext context)
    {
        var linkGenerator = context.HttpContext.RequestServices.GetService<LinkGenerator>();

        var action = context.ActionDescriptor.RouteValues["action"]!;
        var address = $"{context.HttpContext.Request.Scheme}://" +
                      $"{context.HttpContext.Request.Host.Value}";
        var link = linkGenerator!.GetPathByAction(
            action: nameof(PocztaController.GetWelcomeMessage),
            controller: "Poczta");
        switch (action)
        {
            case nameof(PocztaController.GetWelcomeMessage):
                response.SetLinks([
                    new LinkObject
                    {
                        Link = $"{address}/{link}",
                        Rel = "self"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetVersion),
                            controller: "Poczta")}",
                        Rel = "current API version"
                    }
                ]);
                break;
            case nameof(PocztaController.GetVersion):
                response.SetLinks([
                    new LinkObject
                    {
                        Link = $"{address}/{link}",
                        Rel = "self"
                    }
                ]);
                break;
            case nameof(PocztaController.CheckSingleLocalShipment):
            case nameof(PocztaController.CheckSingleShipment): 
                response.SetLinks([
                    new LinkObject
                    {
                        Link = $"{address}/{link}",
                        Rel = "self"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckShipments),
                            controller: "Poczta")}",
                        Rel = "checks many shipments at once"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetVersion),
                            controller: "Poczta")}",
                        Rel = "current API version"
                    }
                ]);
                break;
            case nameof(PocztaController.CheckShipments):
                response.SetLinks([
                    new LinkObject
                    {
                        Link = $"{address}/{link}",
                        Rel = "self"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetMaxShipments),
                            controller: "Poczta")}",
                        Rel = "max shipments possible"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckSingleShipment),
                            controller: "Poczta")}",
                        Rel = "check single shipment"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckShipmentsByDate),
                            controller: "Poczta")}",
                        Rel = "check shipments by date"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetVersion),
                            controller: "Poczta")}",
                        Rel = "current API version"
                    }
                ]);
                break;
            case nameof(PocztaController.CheckLocalShipments):
                response.SetLinks([
                    new LinkObject
                    {
                        Link = $"{address}/{link}",
                        Rel = "self"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetMaxShipments),
                            controller: "Poczta")}",
                        Rel = "max shipments possible"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckSingleLocalShipment),
                            controller: "Poczta")}",
                        Rel = "check single local shipment"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckLocalShipmentsByDate),
                            controller: "Poczta")}",
                        Rel = "check local shipments by date"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetVersion),
                            controller: "Poczta")}",
                        Rel = "current API version"
                    }
                ]);
                break;
            case nameof(PocztaController.CheckShipmentsByDate):
                response.SetLinks([
                    new LinkObject
                    {
                        Link = $"{address}/{link}",
                        Rel = "self"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetMaxShipments),
                            controller: "Poczta")}",
                        Rel = "max shipments possible"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckShipments),
                            controller: "Poczta")}",
                        Rel = "check shipments without a date"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetVersion),
                            controller: "Poczta")}",
                        Rel = "current API version"
                    }
                ]);
                break;
            case nameof(PocztaController.CheckLocalShipmentsByDate):
                response.SetLinks([
                    new LinkObject
                    {
                        Link = $"{address}/{link}",
                        Rel = "self"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetMaxShipments),
                            controller: "Poczta")}",
                        Rel = "max shipments possible"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckLocalShipments),
                            controller: "Poczta")}",
                        Rel = "check local shipments without a date"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetVersion),
                            controller: "Poczta")}",
                        Rel = "current API version"
                    }
                ]);
                break;
            case nameof(PocztaController.GetMaxShipments):
                response.SetLinks([
                    new LinkObject
                    {
                        Link = $"{address}/{link}",
                        Rel = "self"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckLocalShipments),
                            controller: "Poczta")}",
                        Rel = "local shipments max possible"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckShipmentsByDate),
                            controller: "Poczta")}",
                        Rel = "shipments by date max possible"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.CheckShipments),
                            controller: "Poczta")}",
                        Rel = "shipments max possible"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetVersion),
                            controller: "Poczta")}",
                        Rel = "current API version"
                    }
                ]);
                break;
            case nameof(PocztaController.GetSingleShipmentByBarCode):
                response.SetLinks([
                    new LinkObject
                    {
                        Link = $"{address}/{link}",
                        Rel = "self"
                    },
                    new LinkObject
                    {
                        Link = $"{address}/{linkGenerator!.GetPathByAction(
                            action: nameof(PocztaController.GetVersion),
                            controller: "Poczta")}",
                        Rel = "current API version"
                    }
                ]);
                break;
        }
    }
}