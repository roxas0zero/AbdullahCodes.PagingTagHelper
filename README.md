# AbdullahCodes.PagingTagHelper
A simple Asp.Net Core paging tag helper that supports adding custom Url values, styled with Bootstrap 4 and is very easy to use.

# Prerequisites
Make sure you have installed Bootstrap 4

# Install
Use the following command to install the nuget package:
```cmd
PM> Install-Package AbdullahCodes.PagingTagHelper
```

# Setup
Add the following code to _ViewImports.cshtml:
```razor
@addTagHelper *, AbdullahCodes.PagingTagHelper
```

# Usage

## Controller
First specify the page size which represents the number of records that should be displayed in one page.<br>
Then, retrieve the data from the database using the Skip and Take methods to retrieve only the current page data.<br>
And lastly retrieve the total number of records as shown below:
```c#
public IActionResult Index(int page = 1)
{
    var pageSize = 10;          
    var products = repository.Products
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize);
    var totalRecords = repository.Products.Count();

    var model = new ProductIndexViewModel 
    {
      Products = products,
      PageNumber = page,
      PageSize = pageSize,
      TotalRecords = totalRecords
    };

    return View(model);
}
```

## View
Create a paging tag in your view and pass the paging data that we calculated earlier in the controller as shown below:
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords">
</paging>
````

# Optional

## Page Action
You can specify if you want the page links to point to a different action.<br>
(If it is unspecified it will point to the same page)
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords"
        page-action="Result">
</paging>
````


## Align Center
You can specify if you want it to be aligned in center or not.<br>
(Default value is true)
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords"
        align-center="false">
</paging>
````

## Max Displayed Pages
You can specify how many pages you want to be displayed at once.<br>
(Default value is 10)
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords"
        max-displayed-pages="5">
</paging>
````

## Additional Url Values
You can specify if you want to send additional Url values (Url query)<br>
(Add page-url-<name of value> and it will be sent to the action as a route value)
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords"
        page-url-user="Model.User"
        page-url-date="Model.Date">
</paging>
````
