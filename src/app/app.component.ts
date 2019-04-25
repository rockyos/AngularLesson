import { Component } from '@angular/core';
     
 
@Component({
    selector: 'root-component',
    template: `
    <div>
       <h2>Lessons</h2>
       <my-table rows="10"></my-table><br/>
       <adder></adder>
    </div>`
})
export class AppComponent { 
  
}

