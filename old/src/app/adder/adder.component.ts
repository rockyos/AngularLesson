import { Component } from '@angular/core';
import { Products } from '../mytable/mytable.component';


@Component({
     selector: 'adder',
    
     templateUrl: 'src/app/adder/adder.component.html'
 
    // styleUrls: ["src/app/mytable/mytable.component.css"]
 
})


export class AdderComponent {
    myName: string;
    myPrice: number;
    myCategory: string;
    myItem: {id: number; name: string; price: number; category: string}

    Add(){
        this.myItem.id = Products[Products.length-1].id + 1;
        this.myItem.name = this.myName;
        this.myItem.price = this.myPrice;
        this.myItem.category = this.myCategory;
        Products.push(this.myItem);
        this.myName="";
        this.myPrice = 0;
        this.myCategory = "";
    }

}