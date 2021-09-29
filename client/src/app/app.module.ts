import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { SharedModule } from './_modules/shared.module';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextInputComponent } from './forms/text-input/text-input.component';
import { DateInputComponent } from './forms/date-input/date-input.component';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { DonorsListComponent } from './donors/donors-list/donors-list.component';
import { BanksListComponent } from './banks/banks-list/banks-list.component';
import { ModerateComponent } from './moderator/moderate/moderate.component';
import { AdminComponent } from './moderator/admin/admin.component';
import { AboutComponent } from './about/about.component';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { TextFormatPipe } from './_services/text-format.pipe';
import { BankUpdateComponent } from './moderator/bank-update/bank-update.component';
import { BankEditComponent } from './moderator/bank-edit/bank-edit.component';
import { FormInputComponent } from './forms/form-input/form-input.component';
import { AdministrationComponent } from './moderator/administration/administration.component';
import { BankRegisterComponent } from './moderator/bank-register/bank-register.component';
import { UserNameValidatorDirective } from './directives/user-name-validator.directive';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { HasRoleDirective } from './directives/has-role.directive';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { FormDateInputComponent } from './forms/form-date-input/form-date-input.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    TextInputComponent,
    DateInputComponent,
    DonorsListComponent,
    BanksListComponent,
    ModerateComponent,
    AdminComponent,
    AboutComponent,
    TextFormatPipe,
    BankUpdateComponent,
    BankEditComponent,
    FormInputComponent,
    AdministrationComponent,
    BankRegisterComponent,
    UserNameValidatorDirective,
    ServerErrorComponent,
    TestErrorsComponent,
    HasRoleDirective,
    EditProfileComponent,
    FormDateInputComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
