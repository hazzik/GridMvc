/***
* This script demonstrates how you can build you own custom filter widgets:
* 1. Specify widget type for column:
*       columns.Add(o => o.Customers.CompanyName)
*           .SetFilterWidgetType("CustomCompanyNameFilterWidget")
* 2. Register script with custom widget on the page:
*       <script src="@Url.Content("~/Scripts/gridmvc.customwidgets.js")" type="text/javascript"> </script>
* 3. Register your own widget in Grid.Mvc:
*       GridMvc.addFilterWidget(new CustomersFilterWidget());
*
* For more documentation see: http://gridmvc.codeplex.com/documentation
*/

/***
* CustomersFilterWidget - Provides filter user interface for customer name column in this project
* This widget renders select list with avaliable customers.
*/
CustomersFilterWidget = (function () {
    function customersFilterWidget() { }
    /***
    * This method must return type of registered widget type in 'SetFilterWidgetType' method
    */
    customersFilterWidget.prototype.getAssociatedTypes = function () { return ["CustomCompanyNameFilterWidget"]; };
    /***
    * This method specify whether render 'Clear filter' button for this widget.
    */
    customersFilterWidget.prototype.showClearFilterButton = function () { return true; };
    /***
    * This method will invoke when user was clicked on filter button.
    * container - html element, which must contain widget layout;
    * lang - current language settings;
    * typeName - current column type (if widget assign to multipile types, see: getAssociatedTypes);
    * filterType - current filter type (like equals, starts with etc);
    * filterValue - current filter value;
    * cb - callback function that must invoked when user want to filter this column. Widget must pass filter type and filter value.
    */
    customersFilterWidget.prototype.render = function (container, lang, typeName, filterType, filterValue, cb) {
        //store parameters:
        this.cb = cb;
        this.container = container;
        this.lang = lang;
        this.filterValue = filterValue;
        this.filterType = filterType;

        this.renderWidget(); //render filter widget
        this.loadCustomers(); //load customer's list from the server
        this.registerEvents(); //handle events
    };
    /***
    * Internal method that build widget layout and append it to the widget container
    */
    customersFilterWidget.prototype.renderWidget = function () {
        var html = '<div class="grid-filter-type-label"><i>This is custom filter widget demo.</i></div>\
                    <div class="grid-filter-type-label">Select customer to filter:</div>\
                    <select style="width:250px;" class="grid-filter-type customerslist">\
                    </select>';
        this.container.append(html);
    };

    /***
    * Method loads all customers from the server via Ajax:
    */
    customersFilterWidget.prototype.loadCustomers = function () {
        var $this = this;
        $.post("Home/GetCustomersNames", function (data) {
            $this.fillCustomers(data.Items);
        });
    };
    /***
    * Method fill customers select list by data
    */
    customersFilterWidget.prototype.fillCustomers = function (items) {
        var customerList = this.container.find(".customerslist");
        for (var i = 0; i < items.length; i++) {
            customerList.append('<option ' + (items[i] == this.filterValue ? 'selected="selected"' : '') + ' value="' + items[i] + '">' + items[i] + '</option>');
        }
    };
    /***
    * Internal method that register event handlers for 'apply' button.
    */
    customersFilterWidget.prototype.registerEvents = function () {
        //get list with customers
        var customerList = this.container.find(".customerslist");
        //save current context:
        var $context = this;
        //register onclick event handler
        customerList.change(function () {
            //invoke callback with selected filter values:
            $context.cb("1"/* Equals */, $(this).val());
        });
    };

    return customersFilterWidget;
})();