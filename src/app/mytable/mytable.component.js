var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from '@angular/core';
var Products = [{ id: 1, name: "product 1", price: 100, Category: "Category_1" },
    { id: 2, name: "product 2", price: 200, Category: "Category_1" },
    { id: 3, name: "product 3", price: 300, Category: "Category_2" },
    { id: 4, name: "product 4", price: 400, Category: "Category_3" },
    { id: 5, name: "product 5", price: 500, Category: "Category_3" },
    { id: 6, name: "product 6", price: 600, Category: "Category_2" },
    { id: 7, name: "product 7", price: 700, Category: "Category_3" },
    { id: 8, name: "product 8", price: 800, Category: "Category_1" },
    { id: 9, name: "product 9", price: 900, Category: "Category_3" },
    { id: 10, name: "product 10", price: 1000, Category: "Category_1" }
];
var MyTableComponent = /** @class */ (function () {
    function MyTableComponent() {
        this.title = "My Test";
        this.myUrl = "www.google.com";
        this.rows = 10000;
        this.del = true;
        this.prodList = Products;
        this.categoryList = ["Category_1", "Category_2", "Category_3"];
        this.categoryNow = " ";
    }
    MyTableComponent.prototype.showRow = function (category) {
        if (this.categoryNow == category || this.categoryNow == " ") {
            return true;
        }
        else {
            return false;
        }
    };
    MyTableComponent.prototype.isMore = function (rowsId) {
        if (rowsId <= this.rows) {
            return true;
        }
        else {
            return false;
        }
    };
    MyTableComponent.prototype.more500 = function (price) {
        if (price > 500) {
            return true;
        }
        else {
            return false;
        }
    };
    MyTableComponent.prototype.less300 = function (price) {
        if (price < 300) {
            return true;
        }
        else {
            return false;
        }
    };
    MyTableComponent.prototype.deleteRow = function (item) {
        console.log(item.id);
        var index = this.prodList.indexOf(item);
        this.prodList.splice[index];
    };
    MyTableComponent = __decorate([
        Component({
            // moduleId: module.id,
            selector: 'my-table',
            inputs: ['rows'],
            styles: [".redcolor{\n        background-color: red;\n    }\n    .greencolor{\n        background-color: green;\n    }\n    td{\n        padding: 5px 20px;\n    }"],
            templateUrl: 'src/app/mytable/mytable.component.html'
            // styleUrls: ["src/app/mytable/mytable.component.css"]
        })
    ], MyTableComponent);
    return MyTableComponent;
}());
export { MyTableComponent };
//# sourceMappingURL=mytable.component.js.map