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

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'donors', component: DonorsListComponent },
  { path: 'banks', component: BanksListComponent },
  { path: 'moderate/:id', component: BankUpdateComponent },
  { path: 'moderate', component: ModerateComponent },
  { path: 'admin', component: AdminComponent },
  { path: 'about', component: AboutComponent },
  { path: '**', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
