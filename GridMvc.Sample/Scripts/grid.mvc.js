/*
* Grid.Mvc
*
* Examples and documentation at: http://gridmvc.codeplex.com
* 
* Version: 1.0.1 (06/06/2012)
* Requires: jQuery v1.3+
*
* LGPL license:
*   http://www.gnu.org/licenses/lgpl.html
*/

$.fn.extend({
    grid: function(options) {
        var a_obj = [];
        $(this).each(function() {
            if (!$(this).hasClass("grid-done")) {
                a_obj.push(new MvcGrid(this, options));
                $(this).addClass("grid-done");
            }
        });
        if (a_obj.length == 1)
            return a_obj[0];
        return a_obj;
    }
});

MvcGrid = (function() {

    function MvcGrid(container, options) {
        this.options = $.extend({ }, this.defaults(), options);
        this.jqContainer = $(container);
        this.init();
    }

    MvcGrid.prototype.init = function() {
        this.events = [];
        if (this.options.selectable)
            this.initGridRowsEvents();
    };

    MvcGrid.prototype.initGridRowsEvents = function() {
        var $this = this;
        this.jqContainer.find(".grid-row").click(function() {
            $this.rowClicked.call(this, $this);
        });
    };

    MvcGrid.prototype.rowClicked = function($context) {
        var row = $(this).closest(".grid-row");
        if (row.length <= 0)
            return;

        var gridRow = { };
        row.find(".grid-cell").each(function() {
            var columnName = $(this).attr("data-name");
            gridRow[columnName] = $(this).text();
        });
        $context.markRowSelected(row);
        $context.notifyOnRowSelect(gridRow);
    };
    MvcGrid.prototype.markRowSelected = function(row) {
        this.jqContainer.find(".grid-row.grid-row-selected").removeClass("grid-row-selected");
        row.addClass("grid-row-selected");
    };
    MvcGrid.prototype.defaults = function() {
        return {
            selectable: true
        };
    };
    /*  EVENTS */
    MvcGrid.prototype.onRowSelect = function(func) {
        this.events.push({ name: "onRowSelect", callback: func });
    };

    MvcGrid.prototype.notifyOnRowSelect = function(row) {
        this.notifyEvent("onRowSelect", row);
    };

    MvcGrid.prototype.notifyEvent = function(eventName, e) {
        for (var i = 0; i < this.events.length; i++) {
            if (this.events[i].name == eventName)
                if (!this.events[i].callback(e)) break;
        }
    };

    return MvcGrid;
})();