import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { IndexComponent } from './index/index.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { ConfirEmailComponent } from './confir-email/confir-email.component';
import { RegisterComponent } from './register/register.component';
import { ForgotpassComponent } from './forgotpass/forgotpass.component';
import { ForgotpassconfirmComponent } from './forgotpassconfirm/forgotpassconfirm.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { ResetpassconfirmComponent } from './resetpassconfirm/resetpassconfirm.component';

const routes: Routes = [
  { path: '', component: IndexComponent },
  { path: 'api/photo', component: IndexComponent },
  { path: 'Account/Confirm', component: ConfirEmailComponent },
  { path: 'Account/ForgotPassword', component: ForgotpassComponent },
  { path: 'Account/ForgotPasswordConfirmation', component: ForgotpassconfirmComponent },
  { path: 'Account/ResetPassword', component: ResetpasswordComponent },
  { path: 'Account/ResetPasswordConfirmation', component: ResetpassconfirmComponent },
  { path: 'Account/Register', component: RegisterComponent },
  { path: 'Account/:Login', component: LoginComponent },
  { path: '**', component: NotfoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
