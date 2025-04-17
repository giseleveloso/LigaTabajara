using System;
using System.Globalization;
using System.Web.Mvc;

public class DecimalModelBinder : IModelBinder
{
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueResult == null)
            return null;

        string value = valueResult.AttemptedValue;

        if (string.IsNullOrEmpty(value))
            return null;

        // Substituir ponto por vírgula ou vice-versa, dependendo da cultura
        if (value.Contains(","))
            value = value.Replace(",", ".");

        decimal result;

        // Usar invariant culture para garantir que o ponto seja aceito como separador decimal
        if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
        {
            return result;
        }

        // Se não conseguir converter, adiciona um erro
        bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Valor decimal inválido");
        return null;
    }
}