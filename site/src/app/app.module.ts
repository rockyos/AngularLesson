import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainComponent } from './main/main.component';
import { HttpService } from './http.service';
import { HttpClientModule} from "@angular/common/http";
import { FormComponent } from './form/form.component';
import { FormsModule }   from '@angular/forms';
import { IndexComponent } from './index/index.component';
import { LoginComponent } from './login/login.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { ReactiveFormsModule }   from '@angular/forms';
import { ConfirEmailComponent } from './confir-email/confir-email.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    FormComponent,
    IndexComponent,
    LoginComponent,
    NotfoundComponent,
    ConfirEmailComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [HttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }
