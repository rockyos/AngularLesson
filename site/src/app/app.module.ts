import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainComponent } from './main/main.component';
import { HttpService } from './http.service';
import { HttpClientModule, HTTP_INTERCEPTORS} from "@angular/common/http";
import { FormComponent } from './form/form.component';
import { FormsModule }   from '@angular/forms';
import { IndexComponent } from './index/index.component';
import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule }   from '@angular/forms';
import { ConfirEmailComponent } from './confir-email/confir-email.component';
import { RegisterComponent } from './register/register.component';
import { ForgotpassComponent } from './forgotpass/forgotpass.component';
import { ForgotpassconfirmComponent } from './forgotpassconfirm/forgotpassconfirm.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { ResetpassconfirmComponent } from './resetpassconfirm/resetpassconfirm.component';
import { HttpConfigInterceptor } from './http.config.interceptor';
import { AuthGuard } from './guard.services';

@NgModule({
  declarations: [
    AppComponent,
    MainComponent,
    FormComponent,
    IndexComponent,
    LoginComponent,
    ConfirEmailComponent,
    RegisterComponent,
    ForgotpassComponent,
    ForgotpassconfirmComponent,
    ResetpasswordComponent,
    ResetpassconfirmComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [HttpService, AuthGuard, {provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
