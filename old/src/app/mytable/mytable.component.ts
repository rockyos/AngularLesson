import { Component, Input } from '@angular/core';

export let Products = [{ id: 1, name: "product 1", price: 100, category: "Category_1"},
    { id: 2, name: "product 2", price: 200, category: "Category_1" },
    { id: 3, name: "product 3", price: 300, category: "Category_2" },
    { id: 4, name: "product 4", price: 400, category: "Category_3" },
    { id: 5, name: "product 5", price: 500, category: "Category_3" },
    { id: 6, name: "product 6", price: 600, category: "Category_2" },
    { id: 7, name: "product 7", price: 700, category: "Category_3" },
    { id: 8, name: "product 8", price: 800, category: "Category_1" },
    { id: 9, name: "product 9", price: 900, category: "Category_3" },
    { id: 10, name: "product 10", price: 1000, category: "Category_1" }
    ];



@Component({
   // moduleId: module.id,
    selector: 'my-table',
    inputs:['rows'],
    styles: [`.redcolor{
        background-color: red;
    }
    .greencolor{
        background-color: green;
    }
    td{
        padding: 5px 20px;
    }`],
    
    templateUrl: 'src/app/mytable/mytable.component.html'

   // styleUrls: ["src/app/mytable/mytable.component.css"]

})
export class MyTableComponent {
    title: string = "My Test";
    rows: number = 10000;
    del: boolean = true;
    prodList = Products;
    categoryList = ["All","Category_1","Category_2","Category_3"];
    categoryNow: string = " ";

    showRow(category: string)
    {
        if (this.categoryNow == category || this.categoryNow == " " || this.categoryNow == "All" ){
            return true;
        } else {
            return false;
        }
    }
    isMore(rowsId: number): boolean{
        if(rowsId <= this.rows)
        {
            return true;
        }else{
            return false;
        }
    }
    more500(price: number): boolean{
         if(price > 500){
             return true;
         } else {
             return false;
         }
    }

    less300(price: number): boolean{
        if(price < 300){
            return true;
        } else {
            return false;
        }
   }


    deleteRow(item: { id: number; name: string; price: number; category: string; }){
        console.log(item.id);
        let index: number =  this.prodList.indexOf(item);
        this.prodList.splice(index,1);
    }
}