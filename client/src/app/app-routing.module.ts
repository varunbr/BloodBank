import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { BanksListComponent } from './banks/banks-list/banks-list.component';
import { DonorsListComponent } from './donors/donors-list/donors-list.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AdminComponent } from './moderator/admin/admin.component';
import { ModerateComponent } from './moderator/moderate/moderate.component';
import { RegisterComponent } from './register/register.component';
import { BankUpdateComponent } from './moderator/bank-update/bank-update.component';
import { BankEditComponent } from './moderator/bank-edit/bank-edit.component';
import { AdministrationComponent } from './moderator/administration/administration.component';
import { BankRegisterComponent } from './moderator/bank-register/bank-register.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { AuthGuard } from './guards/auth.guard';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { PageNotFoundComponent } from './errors/page-not-found/page-not-found.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    data: { roles: ['Member'] },
    children: [
      { path: 'donors', component: DonorsListComponent },
      { path: 'banks', component: BanksListComponent },
      { path: 'profile', component: EditProfileComponent },
      {
        path: 'moderate',
        canActivate: [AuthGuard],
        data: { roles: ['BankAdmin', 'BankModerator'] },
        children: [
          { path: 'bank/:id', component: BankUpdateComponent },
          { path: '', component: ModerateComponent },
        ],
      },
      {
        path: 'admin',
        canActivate: [AuthGuard],
        data: { roles: ['Admin', 'Moderator'] },
        children: [
          { path: 'bank-register', component: BankRegisterComponent },
          { path: 'bank/:id', component: BankEditComponent },
          { path: '', component: AdminComponent },
        ],
      },
      {
        path: 'administration',
        component: AdministrationComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'] },
      },
      {
        path: 'test',
        component: TestErrorsComponent,
        canActivate: [AuthGuard],
        data: { roles: ['Admin'] },
      },
    ],
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'about', component: AboutComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
