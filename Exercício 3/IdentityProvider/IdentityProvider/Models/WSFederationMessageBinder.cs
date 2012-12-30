using System;
using System.Web.Mvc;
using Microsoft.IdentityModel.Protocols.WSFederation;

namespace IdentityProvider.Models
{
    public class WSFederationMessageBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            try
            {
                var message = WSFederationMessage.CreateFromUri(controllerContext.HttpContext.Request.Url);

                if (!bindingContext.ModelType.IsInstanceOfType(message))
                {
                    throw new WSFederationMessageException();
                }

                return message;
            }
            catch (WSFederationMessageException ex)
            {
                bindingContext.ModelState.AddModelError("", ex);
                return null;
            }
        }
    }
}