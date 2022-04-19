# AbdullahCodes.PagingTagHelper
[![](https://img.shields.io/nuget/v/AbdullahCodes.PagingTagHelper.svg)](https://www.nuget.org/packages/AbdullahCodes.PagingTagHelper)
[![](https://img.shields.io/nuget/dt/AbdullahCodes.PagingTagHelper.svg)](https://www.nuget.org/packages/AbdullahCodes.PagingTagHelper)

A simple Asp.Net Core paging tag helper that supports adding custom Url values, styled with Bootstrap 4 and is very easy to use.<br><br>
Compatible with:
- ASP.NET CORE 2.0
- ASP.NET CORE 2.1
- ASP.NET CORE 2.2
- ASP.NET CORE 3.0
- ASP.NET CORE 3.1

![image](https://user-images.githubusercontent.com/6898829/92983625-3ee5f980-f4ad-11ea-8f57-611579f779a8.png)

## Prerequisites
Make sure you have installed Bootstrap 4

## Installation
Use the following command to install the nuget package:
```cmd
PM> Install-Package AbdullahCodes.PagingTagHelper
```

## Setup
Add the following code to _ViewImports.cshtml:
```razor
@addTagHelper *, AbdullahCodes.PagingTagHelper
```

## Usage

### Controller
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

### View
Create a paging tag in your view and pass the paging data that we calculated earlier in the controller as shown below:
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords">
</paging>
````

## Optional

### Page Action
You can specify if you want the page links to point to a different action.<br>
(If it is unspecified it will point to the same page)
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords"
        page-action="Result">
</paging>
````

### Align Center
You can specify if you want it to be aligned in center or not.<br>
(Default value is true)
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords"
        align-center="false">
</paging>
````
Not centered
![image](https://user-images.githubusercontent.com/6898829/92983659-84a2c200-f4ad-11ea-8200-916555c66489.png)

Centered
![image](https://user-images.githubusercontent.com/6898829/92983792-3c37d400-f4ae-11ea-90c3-0dbfbce92a5d.png)

### Responsive
You can specify if you want it to be responsive or not.<br>
(Default value is true)
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords"
        responsive="false">
</paging>
````
Responsive
![image](https://user-images.githubusercontent.com/6898829/163982403-11282011-6bd7-4423-b1dd-cf12fafe0804.png)

### Max Displayed Pages
You can specify how many pages you want to be displayed at once.<br>
(Default value is 10)
````html
<paging page-number="Model.PageNumber" 
        page-size="Model.PageSize"
        total-records="Model.TotalRecords"
        max-displayed-pages="5">
</paging>
````

![image](https://user-images.githubusercontent.com/6898829/92983697-b9167e00-f4ad-11ea-9b56-94a4172f0d74.png)

### Additional Url Values
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
