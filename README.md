# ContosoUniversity using ASP.NET Core 3.0, Razor Pages and TagHelpers

Based on [Contoso University](https://github.com/jbogard/ContosoUniversityDotNetCore-Pages).

To run, execute the build script (`Build.ps1`). Open the solution and run!

## Primary focus demonstrated

- TagHelpers

## Available Custom Tag Helpers

### `<form-block-display asp-for="Model.Property" />`

This helper was designed to cover the case of displaying a form in read-only without any fields. The HTML markup looks as follow:
```
<div class="form-group">
  <label class="control-label" for="Model_Property">Title</label>
  <div>Value</div>
</div>
```

### `<form-block asp-for="Model.Property" />`

This helper was designed to cover the case of an editable form. The HTML markup as follow:
```
<div class="form-group ">
  <label class="control-label" for="Data_Title">Title</label>
  <input class="form-control valid"
    data-val="true"
    data-val-length="'Title' must be between 3 and 50 characters."
    data-val-length-max="50"
    data-val-length-min="3"
    data-val-required="'Title' must not be empty."
    id="Data_Title" name="Data.Title"
    type="text"
    value="Laboris eiusmod ulla"
  />
  <span class="text-danger field-validation-valid"
    data-valmsg-for="Data.Title" data-valmsg-replace="true">
  </span>
</div>
```
Note that this helper will try to resolve common data types such as: decimal, int, string, date; including their nullable equivalent. Also, note that it will add a label, data validation attributes and span element.

Select fields work the same way but technically require more complexity to resolve the source based on a C# attribute in the propery, for example:
```
[SelectList(typeof(DepartmentSelectListOptionsProvider))]
public Department Department { get; set; }
```

### `<tl name-for="Model.PropertyName><tl/>`

This attribute helper is allowed in any HTML element. It will try to resolve the **title** by using the Display Name attribute first, or then break down the word into separate words if is a multi-word properly. For example, if `Model.HomeAddress`, then "Home Address" will be used. The attribute will return the following HTML markup:
```
<lt>Propery Name</tl>
```

### `<td value-for="Model.Property><td/>`

This attribute helper is allowed in any HTML element. It will try to resolve the **value** by using the propery data type. The attribute will return the following HTML markup:
```
<lt>Value</tl>
```
Note that the following patterns are in place depending on the data type:
* Decimal will use currency format e.g. `$1,234.45`
* Date will use the following format e.g. `3/11/1995`