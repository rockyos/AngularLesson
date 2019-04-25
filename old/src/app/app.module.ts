import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule }   from '@angular/forms';
import { MyTableComponent }   from './mytable/mytable.component';
import { AdderComponent }   from './adder/adder.component';
import { AppComponent }   from './app.component';

@NgModule({
    imports:      [ BrowserModule, FormsModule ],
    declarations: [ AppComponent, MyTableComponent,AdderComponent ],
    bootstrap:    [ AppComponent ]
})
export class AppModule { }