using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;
using SIS.Http.Enums;
using SIS.Http.Extensions;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Api;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SIS.Framework.Routes
{
    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var controllerName = "";
            var requestMethod = "";
            var actionName = "";

            if (request.Path=="/") //should be Path not URL ??
            {
                controllerName = "Home";
                actionName = "Index";
                requestMethod = "GET";
            }
            else
            {
                var requestUrlSplit = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                controllerName =  requestUrlSplit[0].Capitalize(); //TODO chech will it work this way
                requestMethod = request.RequestMethod.ToString();
                actionName = requestUrlSplit[1].Capitalize(); //Same check
            }

            //Controller
            var controller = this.GetController(controllerName, request);
            //Action
            var action = this.GetMethod(requestMethod, controller, actionName);


            if (controller==null|| action==null)
            {
                throw new NullReferenceException();
            }
            object[] actionParameters = this.MapActionParameters(action, request, controller);
            IActionResult actionResult = this.InvokeAction(controller, action, actionParameters);

            var response = this.PrepareResponse(actionResult);

            return response;
        }

        private IHttpResponse PrepareResponse(IActionResult actionResult)
        {
            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationResult, HttpResponseStatusCode.Ok);
            }
            else if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationResult);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supporeted");
            }            
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            if (!string.IsNullOrEmpty(controllerName))
            {
                string controllerTypeName = string.Format(
                    "{0}.{1}.{2}{3}, {0}",
                    MvcContext.Get.AssemblyName,
                    MvcContext.Get.ControllersFolder,
                    controllerName,
                    MvcContext.Get.ControllersSuffix);

                var controllerType = Type.GetType(controllerTypeName);
                var controller = (Controller)Activator.CreateInstance(controllerType);

                if (controller!=null)
                {
                    controller.Request = request;
                }

                return controller;
            }

            return null;
        }

        private MethodInfo GetMethod(string requestMethod, Controller controller, string actionName)
        {
            MethodInfo method = null;

            foreach (var methodInfo in GetSuitableMethods(controller, actionName))
            {
                var attributes = methodInfo
                    .GetCustomAttributes()
                    .Where(x => x is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!attributes.Any() && requestMethod.ToUpper()=="GET")
                {
                    return methodInfo;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.IsValid(requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return method;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller==null)
            {
                return new MethodInfo[0];
            }

            return controller
                .GetType()
                .GetMethods()
                .Where(mi=>mi.Name.ToLower()==actionName.ToLower());
        }

        private object[] MapActionParameters(MethodInfo action, IHttpRequest request, Controller controller)
        {
            ParameterInfo[] actionParametersInfo = action.GetParameters();
            object[] mappedActionParameters = new object[actionParametersInfo.Length];

            for (int i = 0; i < actionParametersInfo.Length; i++)
            {
                ParameterInfo currentParameterInfo = actionParametersInfo[i];

                if (currentParameterInfo.ParameterType.IsPrimitive
                    || currentParameterInfo.ParameterType==typeof(string))
                {
                    if (currentParameterInfo==null)
                    {
                        continue;
                    }
                    mappedActionParameters[i] = ProcessPrimitiveParameter(currentParameterInfo, request);
                }
                else
                {
                    object bindingModel= ProcessBindingModelParameters(currentParameterInfo, request);
                    controller.ModelState.IsValid = this.IsValidModel(bindingModel, currentParameterInfo.ParameterType);
                    mappedActionParameters[i] = bindingModel;
                }
            }

            return mappedActionParameters;
        }

        private bool? IsValidModel(object bindingModel, Type bindingModelType)
        {
            var properties = bindingModelType.GetProperties();
            foreach (var property in properties)
            {
                var propertyValidationAttributes= property.GetCustomAttributes()
                    .Where(ca => ca is ValidationAttribute)
                    .Cast<ValidationAttribute>()
                    .ToList();

                foreach (var validationAttribute in propertyValidationAttributes)
                {
                    var propertyValue = property.GetValue(bindingModel);
                    if (!validationAttribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private object ProcessBindingModelParameters(ParameterInfo param, IHttpRequest request)
        {
            Type bindingModelType = param.ParameterType;

            //ViewModels must have empty ctor
            var bindingModelInstance = Activator.CreateInstance(bindingModelType);
            var bindingModelProperties = bindingModelType.GetProperties();

            foreach (var propertyInfo in bindingModelProperties)
            {
                try
                {
                    object value = this.GetParametersFromRequestData(request, propertyInfo.Name);
                    propertyInfo.SetValue(bindingModelInstance, Convert.ChangeType(value, propertyInfo.PropertyType));
                }
                catch (Exception)
                {

                    Console.WriteLine($"The {propertyInfo.Name} field could not be mapped");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private object GetParametersFromRequestData(IHttpRequest request, string paramName)
        {
            if (request.QueryData.Any(x=>x.Key.ToLower()==paramName.ToLower()))
            {
                return request.QueryData.First(x => x.Key.ToLower() == paramName.ToLower());
            }
            else if (request.FormData.Any(x => x.Key.ToLower() == paramName.ToLower()))
            {
                return request.FormData.First(x => x.Key.ToLower() == paramName.ToLower());
            }

            return null;
        }

        private object ProcessPrimitiveParameter(ParameterInfo param, IHttpRequest request)
        {
            object value = this.GetParametersFromRequestData(request, param.Name);
            return Convert.ChangeType(value, param.ParameterType);
        }

        private IActionResult InvokeAction(Controller controller, MethodInfo action, object[] actionParameters)
        {
            return (IActionResult)action.Invoke(controller, actionParameters);
        }

    }
}
